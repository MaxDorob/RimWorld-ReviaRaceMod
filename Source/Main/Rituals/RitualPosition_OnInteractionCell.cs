using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace.Rituals
{
    /// <summary>
    /// The same ritual position, but pawn able to stay on altar
    /// </summary>
    public class RitualPosition_OnInteractionCell : RimWorld.RitualPosition_OnInteractionCell
    {
        private static List<IntVec3> tmpPotentialCells = new List<IntVec3>(8);
        public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
        {
            tmpPotentialCells.Clear();
            Thing thing = spot.GetThingList(ritual.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
            Map mapHeld = p.MapHeld;
            CellRect rect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(spot, 0);
            this.FindCells(tmpPotentialCells, thing, rect, spot, (thing != null) ? thing.Rotation : Rot4.South);
            //CommonRitualCellPredicates.RemoveLeastDesirableRitualCells(tmpPotentialCells, spot, mapHeld, p, rect);
            Func<IntVec3, bool> validator = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, rect);
            IntVec3 intVec;
            if (tmpPotentialCells.Count != 0)
            {
                intVec = tmpPotentialCells[0];
            }
            else
            {
                intVec = base.GetFallbackSpot(rect, spot, p, ritual, validator);
            }
            Rot4 orientation;
            if (this.faceThing)
            {
                if (this.facing != Rot4.Invalid)
                {
                    Log.Error("Only one of faceThing and facing should be specified.");
                }
                orientation = Rot4.FromAngleFlat((thing.Position - intVec).AngleFlat);
            }
            else
            {
                orientation = this.facing;
            }
            return new PawnStagePosition(intVec, thing, orientation, this.highlight);
        }
    }
}
