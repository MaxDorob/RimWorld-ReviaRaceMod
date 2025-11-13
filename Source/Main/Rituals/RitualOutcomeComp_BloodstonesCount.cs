using HarmonyLib;
using ReviaRace.Comps;
using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeComp_BloodstonesCount : RitualOutcomeComp_Quality
    {
        private static readonly SimpleCurve bloodstonesCurve = new SimpleCurve(); 
        internal static SimpleCurve BloodstonesCurve
        {
            get
            {
                return bloodstonesCurve;
            }
        }
        internal static void InitCurve()
        {
            var maxCost = InvokeGreaterBlessing.GetAdvanceCost(8);
            var points = new List<CurvePoint>();
            for (int i = 1; i < 8; i++)
            {
                var count = (float)InvokeGreaterBlessing.GetAdvanceCost(i);
                points.Add(new CurvePoint(count, 1f / (maxCost - count)));
            }
            points.Add(new CurvePoint(maxCost, 1f));
            bloodstonesCurve.SetPoints(points);
        }
        public RitualOutcomeComp_BloodstonesCount() : base()
        {
            curve = bloodstonesCurve;
        }
        [HarmonyLib.HarmonyPatch(typeof(Dialog_BeginRitual), nameof(Dialog_BeginRitual.DoRightColumn))]
        internal static class DialogBeginRitual_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var targetMethod = AccessTools.Method(typeof(Dialog_BeginRitual), nameof(Dialog_BeginRitual.DoRoleSelection));
                foreach (var ins in instructions)
                {
                    if (ins.Calls(targetMethod))
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return CodeInstruction.Call(typeof(DialogBeginRitual_Patch), nameof(DialogBeginRitual_Patch.CustomSection));
                    }
                    yield return ins;
                }
            }
            public static void CustomSection(Dialog_BeginRitual dialog, ref RectDivider layout)
            {
                foreach (var compWithSlider in dialog.outcome.comps.OfType<RitualOutcomeComp_BloodstonesCount>())
                {
                    compWithSlider.DrawSlider(dialog.ritual, ref layout);
                }
            }
        }
        public SimpleCurve Curve
        {
            get
            {
                
                return curve;
            }
        }
        public void DrawSlider(Precept_Ritual ritual, ref RectDivider layout)
        {
            var data = (RitualOutcomeComp_DataBloodstonesCount)ritual.outcomeEffect.DataForComp(this);
            var rect = layout.NewRow(32f, VerticalJustification.Top, 28f);
            Widgets.HorizontalSlider(rect, ref data.selectedCount, new FloatRange(1, Curve.Last().x), label, 1f);
        }
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_DataBloodstonesCount();
        }
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return ((RitualOutcomeComp_DataBloodstonesCount)data).selectedCount;
        }
        public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            var count = ((RitualOutcomeComp_DataBloodstonesCount)data).selectedCount;
            return this.Curve.Evaluate(count);
        }
        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            var count = ((RitualOutcomeComp_DataBloodstonesCount)data).selectedCount;
            float num = this.Curve.Evaluate(count);
            return new QualityFactor
            {
                label = this.label,
                count = count.ToString(),
                qualityChange = ExpectedOffsetDesc(num >= 0f, num),
                positive = (num >= 0f),
                quality = num,
                priority = 5.2f
            };
        }
        protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
        {
            var curve = Curve;
            return "QualityOutOf".Translate(quality.ToStringWithSign("0.#%"), curve.Points[curve.PointsCount - 1].y.ToStringWithSign("0.#%"));
        }
    }
    public class RitualOutcomeComp_DataBloodstonesCount : RitualOutcomeComp_Data
    {
        public float selectedCount = 1;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref selectedCount, nameof(selectedCount));
        }
    }
}
