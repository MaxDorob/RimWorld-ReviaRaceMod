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
    public class StageFailTrigger_NotEnoughCarriedThings : StageFailTrigger
    {
        public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
        {
            var pawn = ritual.PawnWithRole(roleId);
            if (pawn == null)
            {
                return true;
            }
            var def = this.def ?? Defs.Bloodstone;
            int count;
            if (this.count > 0)
            {
                count = this.count;
            }
            else
            {
                var comp = ritual.Ritual.outcomeEffect.def.comps.OfType<RitualOutcomeComp_BloodstonesCount>().FirstOrDefault();
                if (comp == null)
                {
                    return false;
                }
                count = (int)((RitualOutcomeComp_DataBloodstonesCount)ritual.Ritual.outcomeEffect.DataForComp(comp)).selectedCount;

            }

            count -= pawn.inventory.Count(def);
            count -= pawn.carryTracker.CarriedCount(def);
            if (count <= 0)
            {
                return false;
            }

            return true;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref roleId, nameof(roleId));
            Scribe_Values.Look(ref count, nameof(count));
            Scribe_Defs.Look(ref def, nameof(def));
        }
        [NoTranslate]
        public string roleId = "sacrificer";
        public ThingDef def;
        public int count = -1;
    }
}
