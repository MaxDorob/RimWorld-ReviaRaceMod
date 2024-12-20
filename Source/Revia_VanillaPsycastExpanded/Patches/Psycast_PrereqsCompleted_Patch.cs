using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaPsycastsExpanded;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded.Patches
{
    [HarmonyPatch(typeof(AbilityExtension_Psycast), nameof(AbilityExtension_Psycast.PrereqsCompleted), new Type[] { typeof(CompAbilities) })]
    internal static class Psycast_PrereqsCompleted_Patch
    {
        static void Postfix(AbilityExtension_Psycast __instance, CompAbilities compAbilities, ref bool __result)
        {
            if (__result && __instance is AbilityExtension_ReviaPsycast reviaPsycast)
            {
                __result = reviaPsycast.PrereqsCompleted(compAbilities);
            }
        }
    }
}
