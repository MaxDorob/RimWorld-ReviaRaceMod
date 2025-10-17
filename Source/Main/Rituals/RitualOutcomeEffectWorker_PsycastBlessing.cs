using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeEffectWorker_PsycastBlessing : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_PsycastBlessing()
        {
        }

        public RitualOutcomeEffectWorker_PsycastBlessing(RitualOutcomeEffectDef def) : base(def)
        {
        }

        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
            var sacrificer = jobRitual.PawnWithRole("sacrificer");
            if (sacrificer.carryTracker.CarriedThing?.def == ReviaDefOf.Revia_MysticalStone)
            {
                sacrificer.carryTracker.DestroyCarriedThing();
            }
            else
            {
                var thingToRemove = sacrificer.inventory.innerContainer.FirstOrDefault(x=>x.def == ReviaDefOf.Revia_MysticalStone);
                if (thingToRemove != null)
                {
                    thingToRemove.Destroy();
                }
                else
                {
                    Log.Error("Can't find a mystical stone to remove");
                }
            }


            if (outcome.Positive)
            {
                if (sacrificer.IsRevia())
                {
                    sacrificer.ChangePsylinkLevel(outcome.positivityIndex);
                }
            }
        }
    }
}
