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
    public class RitualOutcomeEffectWorker_SkarnesBlessing : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_SkarnesBlessing() : base() { }
        public RitualOutcomeEffectWorker_SkarnesBlessing(RitualOutcomeEffectDef def) : base(def) { }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
            if (outcome.Positive)
            {
                var blessedPawn = jobRitual.PawnWithRole("sacrificer");
                if (blessedPawn.IsRevia())
                {
                    var currentLvl = blessedPawn.GetSoulReapTier();
                    if (currentLvl < 9)
                    {
                        blessedPawn.RemoveSoulReapHediffs();
                        blessedPawn.AddSoulReapTier(currentLvl + 1);
                        Find.HistoryEventsManager.RecordEvent(new HistoryEvent(ReviaDefOf.Revia_BlessedWithTail, blessedPawn.Named(HistoryEventArgsNames.Doer)));
                    }
                }
                if (outcome.BestPositiveOutcome(jobRitual) || (outcome.positivityIndex == 1 && Rand.Chance(0.33f)))
                {
                    foreach (var pawn in totalPresence.Keys.Where(x => x.IsRevia()))
                    {
                        pawn.health.AddHediff(ReviaDefOf.ReviaRaceBlessedBySkarne);
                    }
                }
            }
        }
    }
}
