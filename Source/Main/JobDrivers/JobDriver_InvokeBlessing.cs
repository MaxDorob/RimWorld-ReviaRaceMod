using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ReviaRace.JobDrivers
{
    public class JobDriver_InvokeBlessing : JobDriver_GotoAndStandSociallyActive
    {
        public override Toil StandToil
        {
            get
            {
                var toil = base.StandToil;
                toil.AddPreInitAction(() =>
                {
                    Thing thing = this.pawn.inventory.innerContainer.FirstOrDefault((Thing t) => t.def == this.job.thingDefToCarry && t.stackCount >= this.job.count);
                    if (thing != null)
                    {
                        this.pawn.carryTracker.TryStartCarry(thing, this.job.count, true);
                    }
                });
                toil.AddFinishAction(() =>
                {
                    var carriedThing = this.pawn.carryTracker.CarriedThing;
                    if (carriedThing.def == job.thingDefToCarry && carriedThing.stackCount >= job.count)
                    {
                        Log.Message($"{carriedThing} ({carriedThing.stackCount}) {job.count}");
                        carriedThing.stackCount -= this.job.count;
                        if (carriedThing.stackCount <= 0)
                        {
                            carriedThing.Destroy();
                        }
                    }
                });
                return toil;
            }
        }
    }
}
