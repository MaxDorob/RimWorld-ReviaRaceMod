using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeEffectWorker_Sacrificed : RitualOutcomeEffectWorker_Consumable
    {

        public RitualOutcomeEffectWorker_Sacrificed() : base() { }
        public RitualOutcomeEffectWorker_Sacrificed(RitualOutcomeEffectDef def) : base(def) { }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
            var multiplier = outcome.positivityIndex;
            if (multiplier <= 0)
            {
                return;
            }
            var prisoner = jobRitual.PawnWithRole("prisoner");
            if (!prisoner.Dead)//Vanilla Expanded Archon?
            {
                return;
            }
            var score = (def.GetModExtension<SacrificeBaseScore_Extension>()?.baseScore ?? 1f) * SacrificeHelper.GetScore(prisoner) * multiplier;
            var position = prisoner.Corpse?.PositionHeld ?? prisoner.PositionHeld;
            var map = prisoner.Corpse?.MapHeld ?? prisoner.MapHeld;
            var thingCount = SacrificeHelper.ThingsForScore(score, prisoner.RaceProps.Humanlike && prisoner.RaceProps.IsFlesh);
            var thing = ThingMaker.MakeThing(thingCount.ThingDef);
            thing.stackCount = thingCount.Count;
            GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);
        }
    }
}
