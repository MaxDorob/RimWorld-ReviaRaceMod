using ReviaRace.Helpers;
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
    public class CompSkarneStatue : CompStatue
    {
        [HarmonyLib.HarmonyPatch(typeof(Thing), nameof(Thing.Notify_DebugSpawned))]
        internal static class DebugSpawnedPatch
        {
            public static void Postfix(Thing __instance)
            {
                var comp = __instance.TryGetComp<CompSkarneStatue>();
                if (comp != null)
                {
                    comp.Notify_DebugSpawned();
                }
            }
        }
        public void Notify_DebugSpawned()
        {
            JustCreatedBy(Find.WorldPawns.AllPawnsAliveOrDead.RandomElement());
        }
        public override bool Active => false;

        public override void JustCreatedBy(Pawn pawn)
        {
            this.bodyType = Rand.Bool ? BodyTypeDefOf.Female : BodyTypeDefOf.Thin;
            this.headType = DefDatabase<HeadTypeDef>.AllDefs.Where(x => x.gender == Gender.Female && x.randomChosen).RandomElement();
            this.hairDef = DefDatabase<HairDef>.AllDefs.Where(x => x.styleTags.Contains("HairLong")).RandomElement();
            this.hairColor = Color.red;
            this.name = new NameSingle("Skarne".Translate());
            this.gender = Gender.Female;
            this.xenotype = Defs.XenotypeDef;
            this.lifestageIndex = LifeStageDefOf.HumanlikeAdult.index;
            hediffsWhichAffectRendering.Clear();
            hediffsWhichAffectRendering.Add(new SavedHediffProps(null, DefDatabase<HediffDef>.GetNamed("ReviaRaceSoulreapTier9"), 1f));
            InitFakePawn();
        }
        
    }
}
