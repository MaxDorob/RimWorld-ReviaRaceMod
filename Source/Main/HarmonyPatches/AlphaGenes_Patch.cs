using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine.Experimental.AI;

namespace ReviaRace.HarmonyPatches
{
    internal static class AlphaGenes_Patch
    {
        internal static void Patch()
        {
            Entry.harmony.Patch(AccessTools.Method("AlphaGenes.Gene_Randomizer:PostAdd"),
                             transpiler: new HarmonyMethod(typeof(AlphaGenes_Patch), nameof(Gene_Randomizer_Transpiler)));
            Entry.harmony.Patch(AccessTools.Method("AlphaGenes.HediffComp_RandomMutation:CompPostTick"),
                             transpiler: new HarmonyMethod(typeof(AlphaGenes_Patch), nameof(Mutations_Transpiler)));
        }
        static bool doLogging = true;
        public static IEnumerable<CodeInstruction> Gene_Randomizer_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo addGeneMI = AccessTools.Method("RimWorld.Pawn_GeneTracker:AddGene", [typeof(GeneDef), typeof(bool)]);
            MethodInfo checkMI = typeof(Entry).GetMethod(nameof(Entry.GeneCanBeAdded));
            if (doLogging) Log.Message($"[AG-Patch] {addGeneMI == null}");

            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.IsStloc() && instruction.operand is LocalVariableInfo info && info.LocalIndex == 7)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return CodeInstruction.LoadField(typeof(Gene), nameof(Gene.pawn));
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return CodeInstruction.Call(typeof(Entry), nameof(Entry.GeneCanBeAdded));
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 7);
                    yield return new CodeInstruction(OpCodes.And);
                    yield return new CodeInstruction(OpCodes.Stloc_S, 7);
                }
            }
        }
        public static IEnumerable<CodeInstruction> Mutations_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Label label = new Label();
            var list = instructions.ToList();
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i-1].opcode == OpCodes.Stloc_2 && list[i].opcode == OpCodes.Br_S)
                {
                    label = (Label)list[i].operand;
                    break;
                }
            }
            bool inserted = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (!inserted && instruction.opcode == OpCodes.Ldloc_1)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return CodeInstruction.LoadField(typeof(HediffComp), nameof(HediffComp.parent));
                    yield return CodeInstruction.LoadField(typeof(HediffWithComps), nameof(HediffWithComps.pawn));
                    yield return new CodeInstruction (OpCodes.Ldloc_3);
                    yield return CodeInstruction.Call(typeof(Entry), nameof(Entry.GeneCanBeAdded));
                    yield return new CodeInstruction(OpCodes.Brfalse_S, label);
                    inserted = true;
                }
                yield return instruction;
            }
            if (!inserted)
            {
                Log.Error("Mutations patch failed");
            }
        }
    }
}
