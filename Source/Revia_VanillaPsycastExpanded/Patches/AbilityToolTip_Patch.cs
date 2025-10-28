using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VanillaPsycastsExpanded.UI;
using Verse;
using VEF.Abilities;

namespace Revia_VanillaPsycastExpanded.Patches
{
    [HarmonyPatch(typeof(ITab_Pawn_Psycasts), "DoAbility")]
    internal static class PsycastTab_DoAbilityPatch
    {
        static void Postfix(Rect inRect, VEF.Abilities.AbilityDef ability, CompAbilities ___compAbilities)
        {
            var ext = ability.GetModExtension<AbilityExtension_ReviaPsycast>();
            if (ext == null)
            {
                return;
            }
            TooltipHandler.TipRegion(inRect, () => ext.GetTooltipText(___compAbilities, ability), ability.GetHashCode());
        }
    }
}
