using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using static ReviaRace.PawnRenderNodeProperties_Animated;

namespace ReviaRace
{
    public class PawnRenderNode_Animated : PawnRenderNode
    {
        public PawnRenderNode_Animated(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }
        public new PawnRenderNodeProperties_Animated Props => props as PawnRenderNodeProperties_Animated;
        private int? animationLength;
        public int AnimationLength => animationLength ??= Props.keyframes.Max(x => x.tick);
        public int CurrentAnimationTick => Find.TickManager.TicksAbs % AnimationLength;
        public KeyframeExtended CurrentKeyframe => Props.keyframes.Last(x => x.tick <= CurrentAnimationTick);
        //public KeyframeExtended NextKeyframe => Props.keyframes.FirstOrDefault() ?? Props.keyframes.First();
        public new bool RecacheRequested => CurrentKeyframe.tick == CurrentAnimationTick;

        public new Graphic Graphic => graphics[CurrentKeyframe];//Change it to override when next update releases
        public virtual Graphic GraphicFor(Pawn pawn, string text)
        {
            if (text.NullOrEmpty())
            {
                return null;
            }
            Shader shader = this.ShaderFor(pawn);
            if (shader == null)
            {
                return null;
            }
            return GraphicDatabase.Get<Graphic_Multi>(text, shader, Vector2.one, this.ColorFor(pawn));
        }
        protected override void EnsureMaterialsInitialized()
        {
            base.EnsureMaterialsInitialized();
            var pawn = this.tree.pawn;
            if (graphics == null && HasGraphic(pawn))
            {
                graphics = new Dictionary<KeyframeExtended, Graphic>(Props.keyframes.Count);
                for (int i = 0; i < Props.keyframes.Count; i++)
                {
                    var keyframe = Props.keyframes[i];
                    graphics.Add(keyframe, GraphicFor(pawn, string.IsNullOrWhiteSpace(keyframe.texPath) ? TexPathFor(pawn) + i.ToString() : keyframe.texPath));
                }
            }
        }

        Dictionary<KeyframeExtended, Graphic> graphics;
    }
}
