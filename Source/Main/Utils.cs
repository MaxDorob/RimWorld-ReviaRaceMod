using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public static class Utils
    {
        public static void PostSacrifide(Map map, bool corpse = false)
        {
            foreach (var mapPawn in map.mapPawns.FreeColonistsAndPrisoners)
            {
                var memories = mapPawn.needs.mood.thoughts.memories;
                if (mapPawn.IsPrisoner)
                {
                    memories.TryGainMemory(corpse ? ReviaDefOf.ReviaRaceThoughtSacrificedNegativePrisoner : Defs.SacrificedFear, corpse ? 1 : -1); ;
                }
                else if (mapPawn.IsColonist && (mapPawn.IsRevia() || mapPawn.IsSkarnite()))
                {
                    mapPawn.needs.mood.thoughts.memories.TryGainMemory(Defs.SacrificedPositive, corpse ? 1 : -1);
                }
                else if (mapPawn.IsColonist &&
                         !(mapPawn.IsCannibal() || mapPawn.IsPsychopath() || mapPawn.IsBloodlust()))
                {
                    mapPawn.needs.mood.thoughts.memories.TryGainMemory(Defs.SacrificedNegative, corpse ? 1 : -1);
                }
            }
        }
        public static void TryGainMemory(this MemoryThoughtHandler thoughtHandler, ThoughtDef thoughtDef, int cap = -1)
        {
            if(cap > 0 && thoughtHandler.NumMemoriesOfDef(thoughtDef) >= cap)
            {
                var thought = thoughtHandler.GetFirstMemoryOfDef(thoughtDef);
                thought.Renew();
            }
            else
            {
                thoughtHandler.TryGainMemory(thoughtDef);
            }
        }
    }
}
