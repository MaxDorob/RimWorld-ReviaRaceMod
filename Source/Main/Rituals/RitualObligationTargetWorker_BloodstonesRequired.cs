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

namespace ReviaRace.Rituals
{
    public class RitualObligationTargetWorker_BloodstonesRequired : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_BloodstonesRequired()
        {
        }

        public RitualObligationTargetWorker_BloodstonesRequired(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        public virtual int Count(RitualRoleAssignments assignments, Pawn forPawn)
        {
            var comp = assignments.Ritual.outcomeEffect.def.comps.OfType<RitualOutcomeComp_BloodstonesCount>().FirstOrDefault();
            if (comp != null)
            {
                return (int)((RitualOutcomeComp_DataBloodstonesCount)assignments.Ritual.outcomeEffect.DataForComp(comp)).selectedCount;
            }
            return def.woodPerParticipant;
        }

        public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
        {

            var pawn = assignments.FirstAssignedPawn("sacrificer");
            if (pawn == null)
            {
                yield return "SacrificerNotSelected".Translate();
                yield break;
            }
            var minimalCount = InvokeGreaterBlessing.GetAdvanceCost(pawn.GetSoulReapTier()) / 3;

            List<Thing> list = target.Map.listerThings.ThingsOfDef(Defs.Bloodstone).Where(thing => !thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger())).ToList();
            int requiredBloodstones = Count(assignments, pawn);
            if (minimalCount > 0 && requiredBloodstones < minimalCount)
            {
                yield return "ReviaMinimalBloodstonesCount".Translate(pawn, minimalCount);
            }


            int countToTake = Math.Max(requiredBloodstones - pawn.inventory.Count(Defs.Bloodstone), 0);

            if (list.Sum(x => x.stackCount) < countToTake)
            {
                TaggedString taggedString = "RitualTargetBloodstonesInfo".Translate(requiredBloodstones);
                yield return taggedString;
            }
        }
    }
}
