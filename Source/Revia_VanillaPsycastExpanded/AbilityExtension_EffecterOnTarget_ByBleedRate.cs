using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_EffecterOnTarget_ByBleedRate : AbilityExtension_EffecterOnTarget
    {
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            targets = targets.Select(x => x.Pawn).Where(p => p != null && bleedRateRange.Includes(p.health.hediffSet.BleedRateTotal)).Select(p => new GlobalTargetInfo(p)).ToArray();
            if (targets.Any())
            {
                base.Cast(targets, ability);
            }
        }
        public FloatRange bleedRateRange = new FloatRange(0.0f, 99999f);
    }
}
