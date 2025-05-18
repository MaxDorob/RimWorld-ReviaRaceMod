using ReviaRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ReviaRace.JobGiver
{
    public class JobGiver_LayDownAwake : RimWorld.JobGiver_LayDownAwake
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var result = base.TryGiveJob(pawn);
            if (result != null)
            {
                result.def = ReviaDefOf.Revia_LayDownAwake;
            }
            return result;
        }
    }
}
