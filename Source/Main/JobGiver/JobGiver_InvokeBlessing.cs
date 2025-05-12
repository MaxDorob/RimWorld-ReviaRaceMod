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
            IntVec3 dest = base.GetDest(pawn);
            Job job = JobMaker.MakeJob(ReviaDefOf.Revia_InvokeBlessing, dest);
            job.locomotionUrgency = this.locomotionUrgency;
            job.expiryInterval = this.expiryInterval;
            job.checkOverrideOnExpire = true;
            job.thingDefToCarry = Defs.Bloodstone;
            var srTier = SoulReaperWorker.GetSoulReapTier(pawn);
            var requiredBloodstones = InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, srTier, ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);
            job.count = requiredBloodstones;
            return job;
        }
    }
}
