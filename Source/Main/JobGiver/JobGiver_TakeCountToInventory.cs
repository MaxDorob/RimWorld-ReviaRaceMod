using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ReviaRace.JobGiver
{
    public class JobGiver_TakeCountToInventory : RimWorld.JobGiver_TakeCountToInventory
    {
        public JobGiver_TakeCountToInventory() : base()
        {
        }
        protected virtual int CountFor(Pawn pawn) => base.count;
        protected override Job TryGiveJob(Pawn pawn)
        {
            var def = this.def ?? Defs.Bloodstone;
            int toTake = Math.Max(CountFor(pawn) - pawn.inventory.Count(def), 0);
            if (toTake == 0)
            {
                return null;
            }
            Thing thing =
                GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake))
             ?? GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), validator: x => !x.IsForbidden(pawn));
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
