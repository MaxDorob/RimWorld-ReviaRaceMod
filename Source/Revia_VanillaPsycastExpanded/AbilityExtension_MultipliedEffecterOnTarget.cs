using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VEF.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_MultipliedEffecterOnTarget : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                if (target.Thing != null)
                {
                    int spawnTimes = SpawnTimes(ability, target.Thing);
                    for (int i = 0; i < spawnTimes; i++)
                    {
                        var effecter = effecterDef.SpawnAttached(target.Thing, target.Map, Scale(ability, target));
                        effecter.Cleanup();
                    }
                }
            }
        }
        protected float Scale(Ability ability, GlobalTargetInfo target) => scale * Multiplier(ability, target.Thing);
        protected virtual float Multiplier(Ability ability, Thing target) => (target as Pawn)?.BodySize ?? 1f;
        protected virtual int SpawnTimes(Ability ability, Thing target) => 1;
        public EffecterDef effecterDef;
        public float scale = 1f;
    }
}
