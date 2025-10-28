using ReviaRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace ReviaRace.JobGiver
{
    public class JobGiver_ProcessXenotypeConversion : JobGiver_GiveSpeechFacingTarget
    {
        public JobGiver_ProcessXenotypeConversion() : base()
        {
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            var result = base.TryGiveJob(pawn);
            if (result != null)
            {
                result.def = ReviaDefOf.Revia_InvokeBlessing;
                result.thingDefToCarry = Defs.Bloodstone;
                Log.Message($"Count: {count}");
                result.count = count;
            }
            return result;
        }
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            var result = (JobGiver_ProcessXenotypeConversion)base.DeepCopy(resolve);
            result.count = this.count;
            return result;
        }
        public int count;
    }
}
