﻿using FacialAnimation;
using HarmonyLib;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;


namespace ReviaRace.HarmonyPatches
{

    [StaticConstructorOnStartup]
    internal static class HarmonyPatch_FacialAnimation
    {
        static Harmony harmony = new Harmony("ReviaRace");

        static HarmonyPatch_FacialAnimation()
        {
            Log.Message("Something called this patch");
        }
        private static readonly Type faceTypeGenerator = AccessTools.TypeByName("FacialAnimation.FaceTypeGenerator`1");
        internal static void Patch()
        {

            {
                try
                {
                    var methodToPatch = AccessTools.Method("FacialAnimation.DrawFaceGraphicsComp:DrawBodyPart").MakeGenericMethod(typeof(IFacialAnimationController));//Unfortunatly, i cant directly patch ControllerBaseComp.InitIfNeeded
                    harmony.Patch(methodToPatch,
                                     prefix: new HarmonyMethod(typeof(HarmonyPatch_FacialAnimation), nameof(Prefix)));
                    harmony.Patch(AccessTools.Method(typeof(NL_SelectPartWindow), nameof(NL_SelectPartWindow.DrawControl)),
                                    transpiler: new HarmonyMethod(typeof(HarmonyPatch_FacialAnimation), nameof(Transpiler)));
                    Log.Message("Succesfully patched [NL] Facial Animation - WIP for Revia biotech");
                }
                catch (Exception ex)
                {
                    Log.Warning("Something went wrong while patching [NL] Facial Animation - WIP\n" + ex.ToString());
                }
            }
        }

        public static void Prefix(ref int drawCount, ref object controller, bool isBottomLayer, ref Vector3 headOffset, Quaternion quaternion, Rot4 facing, bool portrait, bool headStump, RotDrawMode mode)
        {
            if (controller == null) return;
            var pawn = (controller as ThingComp).parent as Pawn;
            var faceTypeField = new Traverse(controller).Field("faceType");
            if (faceTypeField.GetValue() == null && pawn.IsRevia())
            {
                var typeOfFaceTypeDef = controller.GetType().BaseType.GetGenericArguments().First();
                var type = faceTypeGenerator.MakeGenericType(typeOfFaceTypeDef);

                var method = AccessTools.Method(type, "GetRandomDef");
                object result;
                try
                {
                    result = method.Invoke(null, new object[] { "ReviaRaceAlien", pawn.gender });
                }
                catch (Exception)
                {
                    result = method.Invoke(null, new object[] { "Human", pawn.gender });
                }
                faceTypeField.SetValue(result);

            }

        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Log.Warning("Patching NL_SelectPartWindow");
            var list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (TryReplace<FacialAnimation.HeadTypeDef>(list, i)) continue;
                if (TryReplace<BrowTypeDef>(list, i)) continue;
                if (TryReplace<LidTypeDef>(list, i)) continue;
                if (TryReplace<EyeballTypeDef>(list, i)) continue;
                if (TryReplace<MouthTypeDef>(list, i)) continue;
                if (TryReplace<SkinTypeDef>(list, i)) continue;

            }
            return list.Where(x => x != null);
        }

        private static bool TryReplace<T>(IList<CodeInstruction> instructions, int position)
        {
            if (instructions[position] == null || instructions[position].opcode != OpCodes.Ldloc_0) return false;

            var type =faceTypeGenerator?.MakeGenericType(typeof(T));
            var method = AccessTools.Method(type, "GetFaceTypeDefsForRace");
            if (instructions[position + 5].Calls(method))
            {
                instructions[position + 1] = null;
                instructions[position + 2] = null;
                instructions[position + 3] = null;
                instructions[position + 4] = null;
                instructions[position + 5] = CodeInstruction.Call(typeof(HarmonyPatch_FacialAnimation), nameof(GetFaceTypeDefsForRaceExtended), generics: new Type[] { typeof(T) });
                return true;
            }
            return false;
        }
        public static IEnumerable<C> GetFaceTypeDefsForRaceExtended<C>(Pawn pawn)
        {
            if (pawn.IsRevia())
            {
                try
                {
                    return CallGetFaceTypeDefsForRace<C>("ReviaRaceAlien", pawn.gender).Union(CallGetFaceTypeDefsForRace<C>(pawn.def.defName, pawn.gender));
                }
                catch (Exception)
                {
                }
            }
            return CallGetFaceTypeDefsForRace<C>(pawn.def.defName, pawn.gender);
        }
        private static IEnumerable<T> CallGetFaceTypeDefsForRace<T>(string raceName, Gender gender)
        {
            var result = faceTypeGenerator
                .MakeGenericType(typeof(T))
                .GetMethod("GetFaceTypeDefsForRace", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { raceName, gender });
            return (result as IEnumerable).Cast<T>();
        }

        internal static void ResetFaceType(Pawn pawn)
        {
            foreach (var item in pawn.AllComps.Where(x => x is IFacialAnimationController))
            {
                new Traverse(item).Field("faceType").SetValue(null);
                new Traverse(item).Field("pawn").SetValue(null);
                new Traverse(item).Method("SetDirty").GetValue();
            }
            PortraitsCache.SetDirty(pawn);

        }
    }
}
