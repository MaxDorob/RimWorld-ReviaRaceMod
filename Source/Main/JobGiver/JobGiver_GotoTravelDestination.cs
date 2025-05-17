using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ReviaRace.JobGiver
{
    public class JobGiver_GotoTravelDestination : RimWorld.JobGiver_GotoTravelDestination
    {
        public JobGiver_GotoTravelDestination() : base()
        {
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            var result = base.TryGiveJob(pawn);
            if(result != null)
            {
                result.collideWithPawns = true;
            }

            Log.Message($"{base.GetDestination(pawn)} {result == null} {result?.targetA}");
            return result;
        }
    }
}
