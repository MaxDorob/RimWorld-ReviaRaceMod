using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class SacrificeBaseScore_Extension : DefModExtension
    {
        public float baseScore = 1f;
        public FloatRange randomFactor = new FloatRange(0.8f, 1.5f);
    }
}
