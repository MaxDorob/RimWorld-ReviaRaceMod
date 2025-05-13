using ReviaRace.Comps;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualBehaviorWorker_InvokeBlessingForSoulreap : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_InvokeBlessingForSoulreap()
        {
        }

        public RitualBehaviorWorker_InvokeBlessingForSoulreap(RitualBehaviorDef def) : base(def)
        {
        }

        public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
        {
            var result = new StringBuilder(base.GetExplanation(ritual, assignments, quality) ?? "");
            var sacrificer = assignments.FirstAssignedPawn("sacrificer");
            if (sacrificer.IsRevia())
            {
                var srTier = SoulReaperWorker.GetSoulReapTier(sacrificer);
                var requiredBloodstones = InvokeGreaterBlessing.GetAdvanceCost(ReviaRaceMod.Settings.CostGrowthMode, srTier, ReviaRaceMod.Settings.CostBase, ReviaRaceMod.Settings.CostGrowthFactor, ReviaRaceMod.Settings.CostGrowthStartTier);
                result.AppendLine("RitualTargetBloodstonesInfo".Translate(requiredBloodstones));
            }
            return result.ToString();
        }
    }
}
