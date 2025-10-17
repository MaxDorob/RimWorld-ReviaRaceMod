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
                return toil;
            }
        }
    }
}
