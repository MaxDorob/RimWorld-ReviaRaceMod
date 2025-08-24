using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace.HarmonyPatches
{
    [HarmonyLib.HarmonyPatch(typeof(Precept_Ritual), nameof(Precept_Ritual.DrawIcon))]
    internal static class NoIconColor_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            foreach (var instruction in instructions)
            {
                if (!patched && instruction.opcode == OpCodes.Ldarg_1)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0); //this
                    yield return CodeInstruction.Call(typeof(NoIconColor_Patch), nameof(NoIconColor_Patch.ChangeColorIfNeeded));
                    patched = true;
                }
                yield return instruction;
            }
        }
        public static void ChangeColorIfNeeded(Precept_Ritual precept)
        {
            if (precept.def.defName.Contains("Revia"))
            {
                GUI.color = Color.white;
            }
        }
    }
    [HarmonyLib.HarmonyPatch(typeof(Command_Ritual), MethodType.Constructor)]
    internal static class CommandIconColor_Patch
    {
        public static void Postfix(Command_Ritual __instance, Precept_Ritual ritual)
        {
            if (ritual.def.defName.Contains("Revia"))
            {
                __instance.defaultIconColor = Color.white;
            }
        }
    }
}
