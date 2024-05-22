using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public class PawnRenderNodeProperties_Animated : PawnRenderNodeProperties
    {
        public PawnRenderNodeProperties_Animated()
        {
            this.workerClass = typeof(PawnRenderNodeWorker_Animated);
            this.nodeClass = typeof(PawnRenderNode_Animated);
        }
        public List<KeyframeExtended> keyframes;
        public class KeyframeExtended : Keyframe
        {
            public string texPath;
        }
    }

}
