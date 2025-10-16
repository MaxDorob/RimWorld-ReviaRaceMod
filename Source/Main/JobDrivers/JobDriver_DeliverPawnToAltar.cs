using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ReviaRace
{
    public class JobDriver_DeliverPawnToAltar : RimWorld.JobDriver_DeliverPawnToAltar
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnDestroyedOrNull(TargetIndex.C);
            this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
            Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(() => !this.pawn.CanReach(this.DropAltar, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
            yield return toil;
            Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
            Toil goToAltar = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch, false);
            yield return Toils_Jump.JumpIf(goToAltar, () => this.pawn.IsCarryingPawn(this.Takee));
            yield return startCarrying;
            yield return goToAltar;
            yield return Toils_General.Do(delegate
            {
                IntVec3 c = this.DropAltar.Position;
                if (this.DropAltar.def.hasInteractionCell)
                {
                    IntVec3 interactionCell = this.DropAltar.InteractionCell;
                    IntVec3 b = (this.DropAltar.Position - interactionCell).ClampInsideRect(new CellRect(-1, -1, 3, 3));
                    c = interactionCell + b;
                }
                else if (this.DropAltar.def.Size.z % 2 != 0)
                {
                    c = this.DropAltar.Position + new IntVec3(0, 0, -this.DropAltar.def.Size.z / 2).RotatedBy(this.DropAltar.Rotation);
                }
                this.job.SetTarget(TargetIndex.B, c);
            });
            yield return Toils_Reserve.Release(TargetIndex.C);
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
        }
    }
}
