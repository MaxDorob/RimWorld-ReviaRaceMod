using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace ReviaRace.Rituals
{
    public class StageEndTrigger_CollectedMysticalStone : StageEndTrigger
    {
        public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
        {
            if (ritual.Ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == this.roleId) == null)
            {
                return null;
            }
            Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
            if (pawn == null)
            {
                return null;
            }
            var requiredCount = 1;
            return new Trigger_TickCondition(() => pawn.carryTracker.CarriedCount(ReviaDefOf.Revia_MysticalStone) + pawn.inventory.Count(ReviaDefOf.Revia_MysticalStone) >= requiredCount, 1);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref roleId, nameof(roleId));
        }
        [NoTranslate]
        public string roleId;
    }
}
