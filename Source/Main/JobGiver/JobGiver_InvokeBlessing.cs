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
using ReviaRace.Rituals;

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
                if (pawn.lord?.LordJob is LordJob_Ritual lordJob_Ritual)
                {
                    var comp = lordJob_Ritual.Ritual.outcomeEffect.def.comps.OfType<RitualOutcomeComp_BloodstonesCount>().FirstOrDefault();
                    if (comp != null)
                    {
                        count = (int)((RitualOutcomeComp_DataBloodstonesCount)lordJob_Ritual.Ritual.outcomeEffect.DataForComp(comp)).selectedCount;
                    }
                    else
                    {
                        count = 1;
                    }
                }
                else
                {
                    count = 1;
                }
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
