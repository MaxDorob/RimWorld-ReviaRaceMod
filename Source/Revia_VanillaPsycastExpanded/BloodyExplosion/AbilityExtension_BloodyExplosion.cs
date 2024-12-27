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
    public class AbilityExtension_BloodyExplosion : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                if (target.Thing is Pawn victim)
                {
                    var bleedRate = victim.health.hediffSet.BleedRateTotal;
                    var amount = bleedRate * multiplier * victim.GetStatValue(StatDefOf.PsychicSensitivity) / victim.BodySize;
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
                    var dropBloodCount = Mathf.CeilToInt(bleedRate / dropBloodPerRate);
                    for (int i = 0; i < dropBloodCount; i++)
                    {
                        victim.health.DropBloodFilth();
                    }
                }
            }
        }


        public float multiplier = 0.20f;
        public float psyfocusPerKill = 0.2f;
        public float multiplierForBloodthirst = 0.85f;
        public float dropBloodPerRate = 0.07f;
    }
}
