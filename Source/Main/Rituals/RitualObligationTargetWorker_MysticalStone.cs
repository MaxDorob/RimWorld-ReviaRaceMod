using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualObligationTargetWorker_MysticalStone : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_MysticalStone()
        {
        }

        public RitualObligationTargetWorker_MysticalStone(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
        {
            var pawn = assignments.FirstAssignedPawn("sacrificer");
            if (pawn == null)
            {
                yield return "SacrificerNotSelected".Translate();
                yield break;
            }
            List<Thing> list = target.Map.listerThings.ThingsOfDef(ReviaDefOf.Revia_MysticalStone).Where(thing => !thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger())).ToList(); ;
            var requiredStones = 1;


            int countToTake = Math.Max(requiredStones - pawn.inventory.Count(ReviaDefOf.Revia_MysticalStone), 0);

            if (list.Sum(x => x.stackCount) < countToTake)
            {
                TaggedString taggedString = "RitualTargetMysticalStoneInfo".Translate();
                yield return taggedString;
            }
        }
    }
}
