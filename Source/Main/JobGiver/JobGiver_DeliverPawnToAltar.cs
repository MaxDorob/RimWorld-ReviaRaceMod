using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using ReviaRace.Helpers;

namespace ReviaRace
{
    public class JobGiver_DeliverPawnToAltar : JobGiver_GotoTravelDestination
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
            if (!pawn.CanReach(pawn2.Position, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
            {
                return null;
            }
            Job job = JobMaker.MakeJob(ReviaDefOf.Revia_DeliverToAltar, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
            job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
            job.expiryInterval = this.jobMaxDuration;
            job.count = 1;
            return job;
        }
    }
}
