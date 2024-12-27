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
using Ability = VFECore.Abilities.Ability;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_BloodyExplosion_Effecter : AbilityExtension_MultipliedEffecterOnTarget
    {
        protected override float Multiplier(Ability ability, Thing target)
        {
            var result = base.Multiplier(ability, target) * ((target as Pawn)?.health.hediffSet.BleedRateTotal ?? 1f);
            return Mathf.Min(result, Mathf.Pow(result, 0.5f));
        }
        protected override int SpawnTimes(Ability ability, Thing target)
        {
            if (target is Pawn pawn)
            {
                var bleedRate = pawn.health.hediffSet.BleedRateTotal;
                int count = Mathf.CeilToInt(bleedRate / ratePerEffecter);
                return count;
            }
            return base.SpawnTimes(ability, target);
        }
        
        public float ratePerEffecter = 0.5f;
    }
}
