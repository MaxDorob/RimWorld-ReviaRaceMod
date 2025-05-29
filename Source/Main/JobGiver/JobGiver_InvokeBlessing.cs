using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using ReviaRace.Helpers;
using ReviaRace.Comps;

namespace ReviaRace.JobGiver
{
    public class JobGiver_InvokeBlessing : JobGiver_GotoAndStandSociallyActive
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            IntVec3 dest = pawn.Position;
            Job job = JobMaker.MakeJob(ReviaDefOf.Revia_InvokeBlessing, dest);
            job.locomotionUrgency = this.locomotionUrgency;
            job.expiryInterval = this.expiryInterval;
            job.checkOverrideOnExpire = true;
            job.thingDefToCarry = def ?? Defs.Bloodstone;
            int count = this.count;
            if (count <= 0)
            {
                count = InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, SoulReaperWorker.GetSoulReapTier(pawn), ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);

            }
            job.count = count;
            return job;
        }
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            var result = (JobGiver_InvokeBlessing)base.DeepCopy(resolve);
            result.count = this.count;
            result.def = this.def;
            return result;
        }

        public int count = -1;
        public ThingDef def;
    }
}
