using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using Verse;
using Verse.AI;

namespace Revia_VanillaPsycastExpanded.Patches
{
    [HarmonyLib.HarmonyPatch(typeof(JobGiver_AIFightEnemy), nameof(JobGiver_AIFightEnemy.GetAbilityJob))]
    internal static class AIFightEnemy_GetAbilityJob_Patch
    {
        public static bool Prefix(Pawn pawn, ref Job __result)
        {
            if (pawn.TryGetComp<CompAbilities>(out var comp))
            {
                var skarneAvatarAbility = comp.LearnedAbilities.FirstOrDefault(x => x.def == ReviaAbilityDefs.Revia_SkarneAvatar);
                if (skarneAvatarAbility != null && skarneAvatarAbility.IsEnabledForPawn(out _))
                {
                    skarneAvatarAbility.CreateCastJob(new GlobalTargetInfo(pawn));
                    __result = pawn.CurJob;
                    pawn.jobs.curJob = null;
                    return false;
                }
                var bloodbathAbility = comp.LearnedAbilities.FirstOrDefault(x => x.def == ReviaAbilityDefs.Revia_Bloodbath);
                if (bloodbathAbility != null && bloodbathAbility.IsEnabledForPawn(out _) && !pawn.health.hediffSet.HasHediff(bloodbathAbility.AbilityModExtensions.OfType<AbilityExtension_Hediff>().First().hediff))
                {
                    bloodbathAbility.CreateCastJob(new GlobalTargetInfo(pawn));
                    __result = pawn.CurJob;
                    pawn.jobs.curJob = null;
                    return false;
                }
            }
            return true;
        }
    }
}
