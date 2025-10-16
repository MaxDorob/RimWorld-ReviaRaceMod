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
            if (outcome.Positive)
            {
                var blessedPawn = jobRitual.PawnWithRole("sacrificer");
                if (blessedPawn.IsRevia())
                {
                    blessedPawn.ChangePsylinkLevel(outcome.positivityIndex);
                }
            }
        }
    }
}
