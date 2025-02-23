using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_DamagePawnsAround : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                var targetsToDamage = target.Map.mapPawns.AllPawnsSpawned.Where(x => x != target.Thing && x.Position.InHorDistOf(target.Cell, radius));
                if (requireLOS)
                {
                    targetsToDamage = targetsToDamage.Where(x => GenSight.LineOfSightToThing(target.Cell, x, x.Map));
                }
                foreach (var toDamage in targetsToDamage)
                {
                    toDamage.TakeDamage(new DamageInfo(damageDef, damageAmount.RandomInRange, penetration, instigator: ability.Caster));
                }
            }
        }
        public float radius;
        public bool requireLOS;
        public DamageDef damageDef;
        public FloatRange damageAmount;
        public float penetration;
    }
}
