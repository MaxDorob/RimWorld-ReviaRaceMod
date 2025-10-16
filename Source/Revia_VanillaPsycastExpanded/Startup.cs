using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    [StaticConstructorOnStartup]
    internal static class Startup
    {
        static Harmony harmony;
        static Startup()
        {
            harmony = new Harmony("Revia_VPE");
            harmony.PatchAll();
        }
    }
}
