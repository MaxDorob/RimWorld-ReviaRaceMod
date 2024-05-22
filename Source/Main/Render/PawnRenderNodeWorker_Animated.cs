using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace
{
    public class PawnRenderNodeWorker_Animated : PawnRenderNodeWorker
    {
        protected override Material GetMaterial(PawnRenderNode node, PawnDrawParms parms)
        {
            if (node is PawnRenderNode_Animated animated)
            {
                Graphic graphic = animated.Graphic;
                if (graphic == null)
                {
                    return null;
                }
                if (node.Props.flipGraphic && parms.facing.IsHorizontal)
                {
                    parms.facing = parms.facing.Opposite;
                }
                Material material = graphic.NodeGetMat(parms);
                if (material != null && !parms.Portrait && parms.flags.FlagSet(PawnRenderFlags.Invisible))
                {
                    material = InvisibilityMatPool.GetInvisibleMat(material);
                }
                return material;
            }
            return base.GetMaterial(node, parms);
        }
    }
}
