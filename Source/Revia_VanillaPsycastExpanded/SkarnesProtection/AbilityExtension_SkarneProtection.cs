using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VEF.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_SkarneProtection : AbilityExtension_AbilityMod
    {
        float scarChance = 0.33f;
        FloatRange severityForPermanent = new FloatRange(0.06f, 0.2f);
        FloatRange tendQuality = new FloatRange(0.2f, 0.65f);
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                if (target.Thing is Pawn pawn)
                {
                    foreach (var injury in pawn.health.hediffSet.hediffs.Where(x=>x.Bleeding))
                    {
                        var permanentComp = injury.TryGetComp<HediffComp_GetsPermanent>();
                        if (permanentComp != null && Rand.Chance(scarChance))
                        {
                            injury.Severity = Mathf.Min(injury.Severity, severityForPermanent.RandomInRange);
                            permanentComp.IsPermanent = true;
                        }
                        else
                        {
                            injury.Tended(tendQuality.RandomInRange, tendQuality.max);
                        }
                    }
                }
            }
        }
    }
}
