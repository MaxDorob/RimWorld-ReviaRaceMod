using ReviaRace.Helpers;
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
                return pawn.BodySize * (pawn.IsHumanlike() ? 1 : 0.33f);

            }
            return 0f;
        }
    }
}
