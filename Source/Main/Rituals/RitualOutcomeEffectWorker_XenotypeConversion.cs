using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeEffectWorker_XenotypeConversion : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_XenotypeConversion() : base() { }
        public RitualOutcomeEffectWorker_XenotypeConversion(RitualOutcomeEffectDef def) : base(def) { }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);

            var sacrificer = jobRitual.PawnWithRole("sacrificer");

            var thingDefToCarry = Defs.Bloodstone;

            var totalCountToRemove = 10;

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

            var blessedPawn = jobRitual.PawnWithRole("convertee");
            if (outcome.Positive)
            {
                if (blessedPawn != null)
                {
                    blessedPawn.genes.SetXenotype(Defs.XenotypeDef);
                }
            }
            var hediff = blessedPawn.health.AddHediff(HediffDefOf.XenogerminationComa);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = GenDate.TicksPerDay * 2 - GenDate.TicksPerDay * outcome.positivityIndex / 2;
        }
    }
}