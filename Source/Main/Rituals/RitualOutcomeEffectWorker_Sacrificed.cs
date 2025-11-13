using ReviaRace.Comps;
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
            var sacrificer = jobRitual.PawnWithRole("sacrificer");
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

            if (map.GameConditionManager.ConditionIsActive(Defs.Eclipse))
            {
                var missingParts = sacrificer.health.hediffSet.GetMissingPartsCommonAncestors();
                if (missingParts.Count > 0)
                {
                    var missing = missingParts.RandomElement();
                    sacrificer.health.hediffSet.hediffs.Remove(missing);
                    var maxHp = missing.Part.def.GetMaxHealth(sacrificer);

                    var injuryHediff = sacrificer.health.AddHediff(HediffDefOf.Cut, missing.Part, new DamageInfo(DamageDefOf.Rotting, 1.0f, 10000.00f));
                    var permHediffComp = injuryHediff.TryGetComp<HediffComp_GetsPermanent>();
                    injuryHediff.Severity = maxHp / 2.0f;
                    permHediffComp.IsPermanent = true;
                }
                else
                {
                    score *= 1.5f;
                }
            }
            else if (map.GameConditionManager.ConditionIsActive(Defs.SolarFlare))
            {
                var permInjuries = sacrificer.health.hediffSet.hediffs.Where(h => h.IsPermanent() && !(h is Hediff_MissingPart)).ToList();
                if (permInjuries.Count > 0)
                {
                    var randomInjury = permInjuries.RandomElement();
                    randomInjury.Severity -= randomInjury.Part.depth == BodyPartDepth.Inside ? 2.0f : 4.0f;
                }
                else
                {
                    score *= 1.2f;
                }
            }

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
            Find.World.GetComponent<SacrificeTracker>().SacrificePerformed(Faction.OfPlayer);
        }
    }
}
