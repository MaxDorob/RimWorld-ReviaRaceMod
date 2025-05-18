using RimWorld;
using System;
using Verse.AI;
using Verse;
using ReviaRace.Helpers;
using ReviaRace.Comps;

namespace ReviaRace.JobGiver
{
    class JobGiver_TakeBloodstonesForBlessingRitual : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var def = Defs.Bloodstone;
            var srTier = SoulReaperWorker.GetSoulReapTier(pawn);
            var count = InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, srTier, ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);
            int toTake = Math.Max(count - pawn.inventory.Count(def), 0);
            if (toTake == 0)
            {
                return null;
            }
            Thing thing =
                GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake))
             ?? GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false));
            if (thing != null)
            {
                Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
                job.count = Math.Min(toTake, thing.stackCount);
                return job;
            }
            return null;
        }
    }
}
