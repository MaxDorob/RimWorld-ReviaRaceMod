using RimWorld;
using System;
using Verse.AI;
using Verse;
using ReviaRace.Helpers;
using ReviaRace.Comps;

namespace ReviaRace.JobGiver
{
    class JobGiver_TakeBloodstonesForBlessingRitual : ReviaRace.JobGiver.JobGiver_TakeCountToInventory
    {
        protected override int CountFor(Pawn pawn)
        {
            return InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, SoulReaperWorker.GetSoulReapTier(pawn), ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);
        }
    }
}
