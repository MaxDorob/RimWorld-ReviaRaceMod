using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeEffectWorker_Sacrificed : RitualOutcomeEffectWorker_Consumable
    {

        public RitualOutcomeEffectWorker_Sacrificed() : base() { }
        public RitualOutcomeEffectWorker_Sacrificed(RitualOutcomeEffectDef def) : base(def) { }
        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            base.Apply(progress, totalPresence, jobRitual);
            var prisoner = jobRitual.PawnWithRole("prisoner");
            var position = prisoner.Corpse?.PositionHeld ?? prisoner.PositionHeld;
            var map = prisoner.Corpse?.MapHeld ?? prisoner.MapHeld;
            var thing = ThingMaker.MakeThing(Defs.Bloodstone);
            thing.stackCount = 2;
            GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);
        }
    }
}
