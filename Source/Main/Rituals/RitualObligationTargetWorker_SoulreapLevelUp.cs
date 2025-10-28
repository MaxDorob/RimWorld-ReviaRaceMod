using ReviaRace.Comps;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace ReviaRace.Rituals
{
    public class RitualObligationTargetWorker_SoulreapLevelUp : RitualObligationTargetWorker_BloodstonesRequired
    {
        public RitualObligationTargetWorker_SoulreapLevelUp()
        {
        }

        public RitualObligationTargetWorker_SoulreapLevelUp(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
        {
            var pawn = assignments.FirstAssignedPawn("sacrificer");
            if (pawn != null)
            {
                var minimalCount = InvokeGreaterBlessing.GetAdvanceCost(pawn.GetSoulReapTier()) / 3;

                List<Thing> list = target.Map.listerThings.ThingsOfDef(Defs.Bloodstone).Where(thing => !thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger())).ToList();
                int requiredBloodstones = Count(assignments, pawn);
                if (minimalCount > 0 && requiredBloodstones < minimalCount)
                {
                    yield return "ReviaMinimalBloodstonesCount".Translate(pawn, minimalCount);
                } 
            }

            foreach (var issue in base.GetBlockingIssues(target, assignments))
            {
                yield return issue;
            }
        }
    }
}
