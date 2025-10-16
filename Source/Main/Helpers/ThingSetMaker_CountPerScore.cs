using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace
{
    public class ThingSetMaker_CountPerScore : ThingSetMaker_StackCount
    {
        public const string paramName = "Score";
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {
            if (parms.custom.TryGetValue(paramName, out var score))
            {
                var count = countPerScore.Evaluate((float)score);
                parms.countRange = new IntRange(Mathf.Max(Mathf.RoundToInt(count), 1), Mathf.CeilToInt(count));
            }
            base.Generate(parms, outThings);
        }
        protected override bool CanGenerateSub(ThingSetMakerParams parms)
        {
            if (!base.CanGenerateSub(parms))
            {
                return false;
            }
            if (parms.custom.TryGetValue(paramName, out var value))
            {
                return countPerScore.Evaluate((float)value) > 0.5f;
            }
            else
            {
                return false;
            }
        }
        public SimpleCurve countPerScore = new SimpleCurve()
        {
            {1, 1}
        };
    }
}
