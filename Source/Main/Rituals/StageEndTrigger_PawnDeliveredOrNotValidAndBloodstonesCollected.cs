using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReviaRace.Rituals
{
    public class StageEndTrigger_PawnDeliveredOrNotValidAndBloodstonesCollected : StageEndTrigger_PawnDeliveredOrNotValid
    {
        public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
        {
            return new Trigger_TickCondition(delegate ()
            {
                foreach (TargetInfo targetInfo in foci)
                {
                    Pawn pawn = targetInfo.Thing as Pawn;
                    IntVec3 c = (spot.Thing != null) ? spot.Thing.OccupiedRect().CenterCell : spot.Cell;
                    if (!pawn.CanReachImmediate(c, PathEndMode.Touch) && !pawn.Dead)
                    {
                        return false;
                    }
                }
                var sacrificer = ritual.PawnWithRole("sacrificer");
                if (sacrificer.inventory.Count(Defs.Bloodstone) < count)
                {
                    return false;
                }
                return true;
            }, 1);
        }
        public int count;
    }
}
