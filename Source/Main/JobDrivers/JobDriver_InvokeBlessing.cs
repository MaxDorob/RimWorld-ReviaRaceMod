using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ReviaRace.JobDrivers
{
    public class JobDriver_InvokeBlessing : JobDriver_GotoAndStandSociallyActive
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        public override Toil StandToil
        {
            get
            {
                var toil = base.StandToil;
                toil.AddPreInitAction(() =>
                {
                    Thing thing = this.pawn.inventory.innerContainer.FirstOrDefault((Thing t) => t.def == this.job.thingDefToCarry);
                    if (thing != null)
                    {
                        this.pawn.carryTracker.TryStartCarry(thing, this.job.count, true);
                    }
                });
                toil.AddFinishAction(() =>
                {
                    var totalCountToRemove = this.job.count;
                    var carriedThing = this.pawn.carryTracker.CarriedThing;
                    if (carriedThing.def == job.thingDefToCarry)
                    {
                        var countToRemove = Mathf.Min(totalCountToRemove, carriedThing.stackCount);
                        carriedThing.stackCount -= countToRemove;
                        if (carriedThing.stackCount <= 0)
                        {
                            carriedThing.Destroy();
                        }
                        totalCountToRemove -= countToRemove;
                    }
                    while (totalCountToRemove > 0)
                    {
                        var thing = pawn.inventory.innerContainer.FirstOrDefault(x=>x.def == job.thingDefToCarry);
                        if (thing == null)
                        {
                            Log.Error($"Can't find enough {job.thingDefToCarry}. Left to remove {totalCountToRemove}");
                            break;
                        }
                        var countToRemove = Mathf.Min(totalCountToRemove, thing.stackCount);
                        thing.stackCount -= countToRemove;
                        if (thing.stackCount <= 0)
                        {
                            thing.Destroy();
                        }
                        totalCountToRemove -= countToRemove;
                    }
                });
                return toil;
            }
        }
    }
}
