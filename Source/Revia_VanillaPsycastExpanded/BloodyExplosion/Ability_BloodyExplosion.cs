using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    public class Ability_BloodyExplosion : VEF.Abilities.Ability
    {
        public override bool AICanUseOn(Thing target)
        {
            return base.AICanUseOn(target) && target is Pawn pawn && pawn.health.hediffSet.BleedRateTotal > 0.5f;
        }
    }
}
