using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeEffectWorker_SkarnesBlessing : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_SkarnesBlessing() : base() { }
        public RitualOutcomeEffectWorker_SkarnesBlessing(RitualOutcomeEffectDef def) : base(def) { }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);

            var sacrificer = jobRitual.PawnWithRole("sacrificer");

            var thingDefToCarry = Defs.Bloodstone;

            var totalCountToRemove = 1;
            var comp = def.comps.OfType<RitualOutcomeComp_BloodstonesCount>().FirstOrDefault();
            if (comp != null)
            {
                totalCountToRemove = (int)((RitualOutcomeComp_DataBloodstonesCount)jobRitual.Ritual.outcomeEffect.DataForComp(comp)).selectedCount;
            }
            else
            {
                Log.Error($"{nameof(RitualOutcomeComp_DataBloodstonesCount)} is null");
            }

            var carriedThing = sacrificer.carryTracker.CarriedThing;
            if (carriedThing?.def == thingDefToCarry)
            {
                var countToRemove = Mathf.Min(totalCountToRemove, carriedThing.stackCount);
                carriedThing.stackCount -= countToRemove;
                if (carriedThing.stackCount <= 0)
                {
                    carriedThing.Destroy();
                }
                totalCountToRemove -= countToRemove;
            }
            while (totalCountToRemove > 0)
            {
                var thing = sacrificer.inventory.innerContainer.FirstOrDefault(x => x.def == thingDefToCarry);
                if (thing == null)
                {
                    Log.Error($"Can't find enough {thingDefToCarry}. Left to remove {totalCountToRemove}");
                    break;
                }
                var countToRemove = Mathf.Min(totalCountToRemove, thing.stackCount);
                thing.stackCount -= countToRemove;
                if (thing.stackCount <= 0)
                {
                    thing.Destroy();
                }
                totalCountToRemove -= countToRemove;
            }

            if (outcome.Positive)
            {
                if (sacrificer.IsRevia())
                {
                    var currentLvl = sacrificer.GetSoulReapTier();
                    if (currentLvl < 9)
                    {
                        sacrificer.RemoveSoulReapHediffs();
                        sacrificer.AddSoulReapTier(currentLvl + 1);
                        Find.HistoryEventsManager.RecordEvent(new HistoryEvent(ReviaDefOf.Revia_BlessedWithTail, sacrificer.Named(HistoryEventArgsNames.Doer)));
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
