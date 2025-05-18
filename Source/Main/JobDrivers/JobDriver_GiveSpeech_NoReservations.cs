using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReviaRace.JobDrivers
{
    /// <summary>
    /// The same driver, that ignores reservation of target cell
    /// </summary>
    public class JobDriver_GiveSpeech_NoReservations : RimWorld.JobDriver_GiveSpeech
    {
        public override Vector3 ForcedBodyOffset => new Vector3(0f, 0f, 0.55f);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
