using ReviaRace;
using ReviaRace.Helpers;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VEF.Abilities;
using RimWorld;
using Verse.Sound;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_ReavingReap : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, VEF.Abilities.Ability ability)
        {
            base.Cast(targets, ability);
            var humans = targets.Select(x => x.Thing).OfType<Corpse>().Select(x => x.InnerPawn).Where(x => x.RaceProps.Humanlike && x.RaceProps.IsFlesh).ToList();
            var animals = targets.Select(x => x.Thing).OfType<Corpse>().Select(x => x.InnerPawn).Where(x => x.RaceProps.Animal && x.RaceProps.IsFlesh).ToList();
            var humansScore = humans.Sum(x => SacrificeHelper.GetScore(x)) * Multiplier(ability.pawn);
            var animalsScore = animals.Sum(x => SacrificeHelper.GetScore(x)) * Multiplier(ability.pawn);
            var cells = GenRadial.RadialCellsAround(targets[0].Thing.Position, ability.GetRadiusForPawn(), true).Where(x => x.InBounds(ability.pawn.Map));
            if (humansScore > float.Epsilon)
            {
                SpawnReward(ability.pawn.Map, cells.RandomElement(), humansScore, true);
            }
            if (animalsScore > float.Epsilon)
            {
                SpawnReward(ability.pawn.Map, cells.RandomElement(), animalsScore, false);
            }
            foreach (var thing in targets.Select(x => x.Thing).OfType<Corpse>().Where(x=>x.InnerPawn.RaceProps.IsFlesh))
            {
                FleckMaker.Static(thing.PositionHeld.ToVector3Shifted(), thing.Map, FleckDefOf.PsycastSkipFlashEntry, 1f);
                SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(thing.PositionHeld, thing.Map, false));
                thing.Destroy(DestroyMode.KillFinalize);
            }
        }
        protected virtual float Multiplier(Pawn pawn) => 0.7f + (0.5f * pawn.GetSoulReapTier()) / 9;
        protected virtual void SpawnReward(Map map, IntVec3 pos, float score, bool human)
        {
            var thingMakerSet = human ? ReviaDefOf.ReviaRaceHumanlikeSacrifice : ReviaDefOf.ReviaRaceAnimalSacrifice;
            var parms = default(ThingSetMakerParams);
            parms.custom ??= [];
            parms.custom[ThingSetMaker_CountPerScore.paramName] = score;
            foreach (var thing in thingMakerSet.root.Generate(parms))
            {
                GenPlace.TryPlaceThing(thing, pos, map, ThingPlaceMode.Near);
            }
        }
    }
}
