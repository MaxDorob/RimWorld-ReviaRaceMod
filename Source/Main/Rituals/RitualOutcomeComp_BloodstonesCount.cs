using HarmonyLib;
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
                    compWithSlider.DrawSlider(ref layout);
                }
            }
        }
        public void DrawSlider(ref RectDivider layout)
        {
            var rect = layout.NewRow(32f, VerticalJustification.Bottom, 28f);
            Widgets.HorizontalSlider(rect, ref count, new FloatRange(1, 100), "Some slider", 1f);
        }
        float count;
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return 1;
        }
    }
}
