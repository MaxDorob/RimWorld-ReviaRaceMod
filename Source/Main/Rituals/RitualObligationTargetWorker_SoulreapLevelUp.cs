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
    public class RitualObligationTargetWorker_SoulreapLevelUp : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_SoulreapLevelUp()
        {
        }

        public RitualObligationTargetWorker_SoulreapLevelUp(RitualObligationTargetFilterDef def) : base(def)
        {
        }

        public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
        {
            Dictionary<Thing, int> dictionary = new Dictionary<Thing, int>();
            bool notEnoughBloodstones = true;
            var pawn = assignments.FirstAssignedPawn("sacrificer");
            if (pawn == null)
            {
                yield return "SacrificerNotSelected".Translate();
                yield break;
            }
            List<Thing> list = target.Map.listerThings.ThingsOfDef(Defs.Bloodstone).Where(thing => !thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger())).ToList(); ;
            var srTier = SoulReaperWorker.GetSoulReapTier(pawn);
            var requiredBloodstones = InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, srTier, ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);


            int countToTake = Math.Max(requiredBloodstones - pawn.inventory.Count(Defs.Bloodstone), 0);

            if (list.Sum(x => x.stackCount) < countToTake)
            {
                TaggedString taggedString = "RitualTargetBloodstonesInfo".Translate(requiredBloodstones);
                yield return taggedString;
            }
        }
    }
}
