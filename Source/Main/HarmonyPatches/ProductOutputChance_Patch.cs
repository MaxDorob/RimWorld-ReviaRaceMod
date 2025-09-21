using ReviaRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.HarmonyPatches
{
    [HarmonyLib.HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    internal static class ProductOutputChance_Patch
    {
        public static bool Prefix(RecipeDef recipeDef, ref IEnumerable<Thing> __result)
        {
            if (recipeDef == ReviaDefOf.ReviaRaceSacrificeCorpse)
            {
                if (Rand.Chance(ReviaRaceMod.Settings.bloodstoneFromCorpseChance))
                {
                    __result = Enumerable.Empty<Thing>();
                    return false;
                }
            }
            return true;
        }
    }
}
