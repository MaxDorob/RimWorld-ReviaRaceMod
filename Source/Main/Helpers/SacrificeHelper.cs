using ReviaRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace
{
    [StaticConstructorOnStartup]//Not sure if it's necessary 
    public static class SacrificeHelper
    {
        static SacrificeHelper()
        {
            cachedRewards ??= DefDatabase<ThingDef>.AllDefs.Where(x => x.HasModExtension<SacrificeReward_Extension>()).ToList();
        }
        private static List<ThingDef> cachedRewards;
        public static float GetScore(Thing thing)
        {
            if (thing is Pawn pawn)
            {
                if (pawn.RaceProps.IsMechanoid)
                {
                    return 0;
                }
                return pawn.BodySize * (pawn.IsHumanlike() ? 1 : 0.33f);

            }
            return 0f;
        }
        public static ThingDefCount ThingsForScore(float score, bool human)
        {
            IEnumerable<ThingDef> possibleRewards;
            if (human)
            {
                possibleRewards = cachedRewards.Where(x => x.GetModExtension<SacrificeReward_Extension>().human);
            }
            else
            {
                possibleRewards = cachedRewards.Where(x => x.GetModExtension<SacrificeReward_Extension>().animal);
            }

            if (possibleRewards.EnumerableNullOrEmpty())
            {
                Log.ErrorOnce("No rewards for sacrifice", 8926471);
                return new ThingDefCount(Defs.Bloodstone, 1);
            }
#if DEBUG
            var textRewards = possibleRewards.Select(x => $"{x.defName}: count - {x.GetModExtension<SacrificeReward_Extension>().countCurve.Evaluate(score)}, weight - {x.GetModExtension<SacrificeReward_Extension>().chanceWeightCurve.Evaluate(score)}");
            Log.Message($"Human: {human}, score: {score}:\nRewards:\n{string.Join("\n", textRewards)}\n\nWeight sum: {possibleRewards.Sum(x=>x.GetModExtension<SacrificeReward_Extension>().chanceWeightCurve.Evaluate(score))}");
#endif
            var reward = possibleRewards.RandomElementByWeight(x => x.GetModExtension<SacrificeReward_Extension>().chanceWeightCurve.Evaluate(score));
            return new ThingDefCount(reward, Mathf.FloorToInt(reward.GetModExtension<SacrificeReward_Extension>().countCurve.Evaluate(score)));

        }
    }
    public class SacrificeReward_Extension : DefModExtension
    {
        public bool animal = false;
        public bool human = true;
        public SimpleCurve countCurve;
        public SimpleCurve chanceWeightCurve;

    }
}
