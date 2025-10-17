using HarmonyLib;
using ReviaRace.Rituals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaPsycastsExpanded;
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

            RitualOutcomeEffectWorker_PsycastBlessing.BlessedWithPsycast += BlessedWithPsycast;
        }

        private static void BlessedWithPsycast(Pawn pawn)
        {
            var psycasts = pawn.Psycasts();
            if (psycasts != null && !psycasts.unlockedPaths.Contains(Defs.Revia_BloodPath))
            {
                psycasts.UnlockPath(Defs.Revia_BloodPath);
            }
        }
    }
}
