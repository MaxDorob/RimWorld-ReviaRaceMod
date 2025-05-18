using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviaRace.JobDrivers
{
    /// <summary>
    /// The same driver, that ignores reservation of target cell
    /// </summary>
    public class JobDriver_GiveSpeech_NoReservations : RimWorld.JobDriver_GiveSpeech
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
