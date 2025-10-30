using ReviaRace.Comps;
using ReviaRace.Helpers;
using ReviaRace.Needs;
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
    public class RitualOutcomeEffectWorker_Sacrificed : RitualOutcomeEffectWorker_FromQuality
    {

        public RitualOutcomeEffectWorker_Sacrificed() : base() { }
        public RitualOutcomeEffectWorker_Sacrificed(RitualOutcomeEffectDef def) : base(def) { }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
            var multiplier = outcome.positivityIndex / 10f;
            if (multiplier <= 0)
            {
                return;
            }
            var prisoner = jobRitual.PawnWithRole("prisoner");
            if (!prisoner.Dead)//Vanilla Expanded Archon?
            {
                return;
            }
            var ext = def.GetModExtension<SacrificeBaseScore_Extension>();
            float score = 1f;
            if (ext != null)
            {
                score = ext.baseScore * ext.randomFactor.RandomInRange;
            }
            score *= SacrificeHelper.GetScore(prisoner) * multiplier;
            var position = prisoner.Corpse?.PositionHeld ?? prisoner.PositionHeld;
            var map = prisoner.Corpse?.MapHeld ?? prisoner.MapHeld;
            var parms = default(ThingSetMakerParams);
            parms.custom ??= [];
            parms.custom[ThingSetMaker_CountPerScore.paramName] = score;
            foreach (var thing in ReviaDefOf.ReviaRaceHumanlikeSacrifice.root.Generate(parms))
            {
                GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);

            }
            var sortedOutcomes = jobRitual.Ritual.outcomeEffect.def.outcomeChances.OrderBy(x => x.positivityIndex).ToList();
            if (outcome.BestPositiveOutcome(jobRitual) || (outcome.positivityIndex == sortedOutcomes[sortedOutcomes.Count - 2].positivityIndex && Rand.Chance(0.33f)))
            {
                foreach (var pawn in totalPresence.Keys.Where(x => x.IsRevia()))
                {
                    pawn.health.AddHediff(ReviaDefOf.ReviaRaceBlessedBySkarne);
                }
            }
            if (outcome.Positive)
            {
                foreach (var pawn in totalPresence.Keys)
                {
                    var need = pawn.needs.AllNeeds.OfType<BloodthirstNeed>().FirstOrDefault();
                    if (need != null && need.PawnAffected)
                    {
                        var amount = 0.03f * outcome.positivityIndex;
                        if (jobRitual.RoleFor(pawn).id == "sacrificer")
                        {
                            amount += 0.5f;
                        }
                        need.CurLevel += amount;
                    }
                }
            }

            Find.World.GetComponent<SacrificeTracker>().SacrificePerformed(Faction.OfPlayer);
        }
    }
}
