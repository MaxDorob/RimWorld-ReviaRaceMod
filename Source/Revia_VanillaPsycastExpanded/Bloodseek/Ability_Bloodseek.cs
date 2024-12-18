using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VanillaPsycastsExpanded.Nightstalker;
using Verse;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    internal class Ability_Bloodseek : Ability_TeleportDark
    {
        protected const float minimalBleedRate = 0.00001f;
        public override bool CanHitTarget(LocalTargetInfo target)
        {
            return target.Thing is Pawn pawn && pawn.health.CanBleed && pawn.health.hediffSet.BleedRateTotal > minimalBleedRate && !target.Cell.Fogged(this.pawn.Map) && target.Cell.Walkable(this.pawn.Map);
        }
        protected override Texture2D MouseAttachment(GlobalTargetInfo target)
        {
            if (!(target.Thing is Pawn pawn) || pawn.health.hediffSet.BleedRateTotal < minimalBleedRate)
            {
                target = GlobalTargetInfo.Invalid;
            }
            return base.MouseAttachment(target);
        }
        public override void ApplyHediffs(params GlobalTargetInfo[] targetInfo)
        {
            
            if (targetInfo.Length > 1)
            {
                targetInfo = targetInfo.Where(x => x.Thing != pawn).ToArray();
            }
            base.ApplyHediffs(targetInfo);
        }
    }
}
