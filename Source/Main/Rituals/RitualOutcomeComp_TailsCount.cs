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
    internal class RitualOutcomeComp_TailsCount : RitualOutcomeComp_Quality
    {
        public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return this.curve.Evaluate(this.Count(ritual, data));
        }

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Pawn pawn = ritual.PawnWithRole(this.roleId);
            if (pawn == null)
            {
                return 1f;
            }
            return pawn.GetSoulReapTier();
        }
        public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
        {
            if (ritual == null)
            {
                return this.labelAbstract;
            }
            Pawn pawn = (ritual != null) ? ritual.PawnWithRole(this.roleId) : null;
            if (pawn == null)
            {
                return null;
            }
            float x = pawn.GetSoulReapTier();
            float num = this.curve.Evaluate(x);
            string str = (num < 0f) ? "" : "+";
            return this.label.CapitalizeFirst().Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num.ToStringPercent()) + ".";
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
            if (pawn == null)
            {
                return null;
            }
            float x = pawn.GetSoulReapTier();
            float num = this.curve.Evaluate(x);
            return new QualityFactor
            {
                label = this.label.Formatted(pawn.Named("PAWN")),
                count = x.ToString(),
                qualityChange = ExpectedOffsetDesc(num >= 0f, num),
                positive = (num >= 0f),
                quality = num,
                priority = 5.1f
            };
        }

        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }

        [NoTranslate]
        public string roleId;

    }
}
