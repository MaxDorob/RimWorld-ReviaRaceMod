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
    public class JobGiver_GiveSpeechFacingTarget : RimWorld.JobGiver_GiveSpeechFacingTarget
    {
        public JobGiver_GiveSpeechFacingTarget() : base()
        {
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            var result = base.TryGiveJob(pawn);
            if (result != null)
            {
                result.def = ReviaDefOf.Revia_GiveSpeech;
                result.targetA = pawn.Position;
            }
            return result;
        }
    }
}
