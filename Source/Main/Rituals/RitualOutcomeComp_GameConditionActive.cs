using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public class RitualOutcomeComp_GameConditionActive : RitualOutcomeComp_QualitySingleOffset
    {
        public override bool DataRequired
        {
            get
            {
                return false;
            }
        }

        public override bool Applies(LordJob_Ritual ritual)
        {
            if (ritual.Map != null && ritual.Map.GameConditionManager.ConditionIsActive(condition))
            {
                return true;
            }
            return false;
        }

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            if (ritual.Map != null && ritual.Map.GameConditionManager.ConditionIsActive(condition))
            {
                return 1f;
            }
            return 0f;
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            float quality = 0f;
            bool active = false;
            if (ritualTarget.Map != null)
            {

                active = ritualTarget.Map.GameConditionManager.ConditionIsActive(condition);
                if (active)
                {
                    quality = this.qualityOffset;
                }
            }
            return new QualityFactor
            {
                label = this.label.CapitalizeFirst(),
                qualityChange = this.ExpectedOffsetDesc(true, quality),
                quality = quality,
                present = active,
                positive = true,
                priority = 0f
            };
        }
        public GameConditionDef condition;
    }
}
