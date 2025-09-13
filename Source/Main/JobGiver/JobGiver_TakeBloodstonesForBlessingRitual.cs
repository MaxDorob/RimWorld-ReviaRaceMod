using RimWorld;
using System;
using Verse.AI;
using Verse;
using ReviaRace.Helpers;
using ReviaRace.Comps;
using ReviaRace.Rituals;
using System.Linq;

namespace ReviaRace.JobGiver
{
    class JobGiver_TakeBloodstonesForBlessingRitual : ReviaRace.JobGiver.JobGiver_TakeCountToInventory
    {
        protected override int CountFor(Pawn pawn)
        {
            if (pawn.lord?.LordJob is LordJob_Ritual lordJob_Ritual)
            {
                var comp = lordJob_Ritual.Ritual.outcomeEffect.def.comps.OfType<RitualOutcomeComp_BloodstonesCount>().FirstOrDefault();
                if (comp != null)
                {
                    return (int)((RitualOutcomeComp_DataBloodstonesCount)lordJob_Ritual.Ritual.outcomeEffect.DataForComp(comp)).selectedCount;
                }
            }
            return 0;
        }
    }
}
