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
    public static class SacrificeHelper
    {
        public static float GetScore(Thing thing)
        {
            if (thing is Pawn pawn)
            {
                if (pawn.RaceProps.IsMechanoid)
                {
                    return 0;
                }
                var score = pawn.BodySize;
                if (!pawn.IsHumanlike())
                {
                    score *= 0.33f;
                }
                var corpse = pawn.Corpse;
                if (corpse != null)
                {
                    score /= (float)corpse.CurRotDrawMode;
                }
            }
            return 0f;
        }
    }
}
