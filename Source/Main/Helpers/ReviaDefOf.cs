using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Helpers
{
    [DefOf]
    public static class ReviaDefOf
    {
        public static TraitDef Tough;
        public static ThoughtDef ReviaRaceThoughtSacrificedNegativePrisoner;
        public static JobDef Revia_DeliverToAltar;
        public static JobDef Revia_Sacrifice;
        public static PreceptDef Revia_PrisonerSacrification;
        [MayRequireIdeology]
        public static MemeDef ReviaRaceSkarniteMeme;
        public static JobDef Revia_InvokeBlessing;
        public static JobDef Revia_GiveSpeech;
        public static JobDef Revia_LayDownAwake;
        [MayRequireRoyalty]
        public static ThingDef Revia_MysticalStone;
        public static HistoryEventDef Revia_PrisonerSacrificed;
        public static HistoryEventDef Revia_BlessedWithTail;
        public static HediffDef ReviaRaceBlessedBySkarne;
        public static RecipeDef ReviaRaceSacrificeCorpse;
    }
}
