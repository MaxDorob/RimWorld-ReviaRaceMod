using ReviaRace;
using ReviaRace.Needs;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_VermilionHarvest : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                if (target.Thing is Pawn victim && victim.health.CanBleed)
                {
                    var amount = bleedLoss * victim.GetStatValue(StatDefOf.PsychicSensitivity) / victim.BodySize;
                    SpawnReward(victim);
                    HealthUtility.AdjustSeverity(victim, HediffDefOf.BloodLoss, amount);
                    var bloodLoss = victim.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
                    var bloodthirst = ability.pawn.needs.TryGetNeed<BloodthirstNeed>();
                    if (victim.Dead)
                    {
                        Utils.Notify_KilledPawn(ability.pawn, victim, null);
                        ability.pawn.psychicEntropy.OffsetPsyfocusDirectly(psyfocusPerKill);

                        if (bloodthirst != null)
                        {
                            bloodthirst.CurLevel += multiplierForBloodthirst * victim.BodySize;
                        }
                    }
                    else if (bloodthirst != null)
                    {
                        bloodthirst.CurLevel += amount * 0.4f * multiplierForBloodthirst * victim.BodySize;
                    }
                    var dropBloodCount = filthCount.RandomInRange;
                    for (int i = 0; i < dropBloodCount; i++)
                    {
                        victim.health.DropBloodFilth();
                    }
                }
            }
        }
        protected virtual void SpawnReward(Pawn victim)
        {
            bool isAnimal = victim.RaceProps.Animal;
            var reward = SacrificeHelper.ThingsForScore(score.RandomInRange, !isAnimal);
            if (reward.Count > 0)
            {
                var thing = ThingMaker.MakeThing(reward.ThingDef);
                thing.stackCount = reward.Count;
                GenPlace.TryPlaceThing(thing, victim.Position, victim.Map, ThingPlaceMode.Near);
            }
        }
        public float bleedLoss = 0.75f;
        public float psyfocusPerKill = 0.03f;
        public float multiplierForBloodthirst = 0.4f;
        public FloatRange score = new FloatRange(0.7f, 2.5f);
        IntRange filthCount = new IntRange(2,6);

    }
}
