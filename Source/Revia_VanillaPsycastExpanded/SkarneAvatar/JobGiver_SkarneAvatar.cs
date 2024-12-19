using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Revia_VanillaPsycastExpanded
{
    public class JobGiver_SkarneAvatar : JobGiver_Berserk
    {
        public JobGiver_SkarneAvatar() : base() { }
        private new Thing FindAttackTarget(Pawn forPawn)
        {
            Thing alternativeTarget = null;
            Thing target = (Thing)AttackTargetFinderEx.FindBestReachableMeleeTarget((thing) =>
            {
                if (thing != forPawn && thing is Pawn pawn)
                {
                    if (!pawn.Downed && pawn.HostileTo(forPawn))
                    {
                        return true;
                    }
                    alternativeTarget = ChooseBetterTarget(forPawn, alternativeTarget as Pawn, pawn);
                }
                return false;
            },

            forPawn,
            maxAttackDistance,
            true,
            true
            );
            return target ?? alternativeTarget;
        }
        private Thing ChooseBetterTarget(Pawn targetSearcher, Pawn currentTarget, Pawn newTarget)
        {
            if (currentTarget == null)
            {
                return newTarget;
            }
            if (!currentTarget.HostileTo(targetSearcher) && newTarget.HostileTo(targetSearcher))
            {
                return newTarget;
            }
            if (currentTarget.Downed && !newTarget.Downed)
            {
                return newTarget;
            }
            return currentTarget;
        }
        public override Job TryGiveJob(Pawn pawn)
        {
            var target = FindAttackTarget(pawn);
            
            if (target != null)
            {
                if (pawn.TryGetAttackVerb(null, false, false) == null)
                {
                    return null;
                }

                if (target != null)
                {
                    Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, target as Thing);
                    job2.maxNumMeleeAttacks = 1;
                    job2.expiryInterval = Rand.Range(420, 900);
                    job2.canBashDoors = true;
                    job2.killIncappedTarget = (target as Pawn)?.Downed ?? false;
                    return job2;
                }
            }
            Log.Warning("Call base");
            return base.TryGiveJob(pawn);
        }
    }
}
