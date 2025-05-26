using ReviaRace.Comps;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace ReviaRace.Rituals
{
    public class RitualObligationTargetWorker_SoulreapLevelUp : RitualObligationTargetWorker_BloodstonesRequired
    {
        public RitualObligationTargetWorker_SoulreapLevelUp()
        {
        }

        public RitualObligationTargetWorker_SoulreapLevelUp(RitualObligationTargetFilterDef def) : base(def)
        {
        }

        public override int Count(Pawn forPawn) => InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, SoulReaperWorker.GetSoulReapTier(forPawn), ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);
    }
}
