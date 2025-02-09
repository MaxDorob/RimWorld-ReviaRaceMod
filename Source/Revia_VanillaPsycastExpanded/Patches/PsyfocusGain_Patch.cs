using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded.Patches
{
    [HarmonyLib.HarmonyPatch(typeof(Pawn), nameof(Pawn.DoKillSideEffects))]
    internal static class PsyfocusGain_Patch
    {
        static void Postfix(DamageInfo? dinfo)
        {
            var killer = dinfo?.Instigator as Pawn;
            if (killer != null && killer.HasPsylink)
            {
                var value = killer.GetStatValue(Defs.Revia_PsyfocusPerKill);
                if (value > float.Epsilon)
                {
                    killer.psychicEntropy?.OffsetPsyfocusDirectly(value);
                }
            }
        }
    }
}
