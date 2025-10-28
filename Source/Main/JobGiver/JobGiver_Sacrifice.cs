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
    public class JobGiver_Sacrifice : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
            if (!pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
            {
                return null;
            }
            return JobMaker.MakeJob(ReviaDefOf.Revia_Sacrifice, pawn2, pawn.mindState.duty.focus);
        }
    }
}
