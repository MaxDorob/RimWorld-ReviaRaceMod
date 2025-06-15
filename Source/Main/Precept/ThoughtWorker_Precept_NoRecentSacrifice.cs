using ReviaRace.Comps;
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
    public class ThoughtWorker_Precept_NoRecentSacrifice : ThoughtWorker_Precept_NoRecentExecution
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!p.IsColonist || p.IsSlave)
            {
                return false;
            }
            int num = Mathf.Max(0, Find.World.GetComponent<SacrificeTracker>().SacrificedTick(p.Faction));
            return Find.TickManager.TicksGame - num > 1800000;
        }
    }
}
