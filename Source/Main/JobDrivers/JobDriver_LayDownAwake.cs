using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReviaRace.JobDrivers
{
    public class JobDriver_LayDownAwake : RimWorld.JobDriver_LayDownAwake
    {
        public override Vector3 ForcedBodyOffset => Vector3.zero;
    }
}
