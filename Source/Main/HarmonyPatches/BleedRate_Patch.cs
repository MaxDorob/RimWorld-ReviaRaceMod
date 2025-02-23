using HarmonyLib;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.HarmonyPatches
{
    [HarmonyPatch()]
    internal static class BleedRate_Patch
    {
        static IEnumerable<MethodInfo> TargetMethods() => typeof(Hediff).AllSubclasses().Select(x => AccessTools.PropertyGetter(x, nameof(Hediff.BleedRate))).Where(x => x.DeclaringType != typeof(Hediff));
        static void Postfix(Hediff __instance, ref float __result)
        {
            float multiplier = __instance.pawn?.GetStatValue(ReviaDefOf.Revia_BleedRate) ?? 1f;
            __result *= multiplier;
        }
    }
}
