using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Comps
{
    internal class SacrificeTracker : WorldComponent
    {
        public SacrificeTracker(World world) : base(world)
        {
        }
        public void SacrificePerformed(Faction faction) => SacrificePerformed(faction, Find.TickManager.TicksGame);
        public void SacrificePerformed(Faction faction, int tick)
        {
            sacrificeTicks ??= new Dictionary<Faction, int>();
            sacrificeTicks[faction] = tick;
        }
        public int SacrificedTick(Faction faction)
        {
            if (sacrificeTicks?.TryGetValue(faction, out var ticks) ?? false)
            {
                return ticks;
            }
            return -1;
        }
        private Dictionary<Faction, int> sacrificeTicks;
        private List<Faction> tmpFactionsList;
        private List<int> tmpTicksList;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref sacrificeTicks, nameof(sacrificeTicks), LookMode.Reference, LookMode.Value, ref tmpFactionsList, ref tmpTicksList);
        }
    }
}
