using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using RimWorld;

namespace Revia_VanillaPsycastExpanded
{
    internal static class AttackTargetFinderEx
    {
        public static IAttackTarget FindBestReachableMeleeTarget(Predicate<IAttackTarget> validator, Pawn searcherPawn, float maxTargDist, bool canBashDoors, bool canBashFences)
        {
            IAttackTarget reachableTarget = null;
            Func<IntVec3, IAttackTarget> bestTargetOnCell = delegate (IntVec3 x)
            {
                List<Thing> thingList = x.GetThingList(searcherPawn.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Thing thing = thingList[i];
                    IAttackTarget attackTarget = thing as IAttackTarget;
                    if (attackTarget != null && validator(attackTarget) && ReachabilityImmediate.CanReachImmediate(x, thing, searcherPawn.Map, PathEndMode.Touch, searcherPawn) && (searcherPawn.CanReachImmediate(thing, PathEndMode.Touch) || searcherPawn.Map.attackTargetReservationManager.CanReserve(searcherPawn, attackTarget)))
                    {
                        return attackTarget;
                    }
                }
                return null;
            };
            searcherPawn.Map.floodFiller.FloodFill(searcherPawn.Position, delegate (IntVec3 x)
            {
                if (!x.WalkableBy(searcherPawn.Map, searcherPawn))
                {
                    return false;
                }
                if ((float)x.DistanceToSquared(searcherPawn.Position) > maxTargDist * maxTargDist)
                {
                    return false;
                }
                Building edifice = x.GetEdifice(searcherPawn.Map);
                if (edifice != null)
                {
                    Building_Door building_Door;
                    if (!canBashDoors && (building_Door = (edifice as Building_Door)) != null && !building_Door.CanPhysicallyPass(searcherPawn))
                    {
                        return false;
                    }
                    if (!canBashFences && edifice.def.IsFence && searcherPawn.def.race.FenceBlocked)
                    {
                        return false;
                    }
                }
                return !PawnUtility.AnyPawnBlockingPathAt(x, searcherPawn, true, false, false);
            }, delegate (IntVec3 x)
            {
                for (int i = 0; i < 8; i++)
                {
                    IntVec3 intVec = x + GenAdj.AdjacentCells[i];
                    if (intVec.InBounds(searcherPawn.Map))
                    {
                        IAttackTarget attackTarget = bestTargetOnCell(intVec);
                        if (attackTarget != null)
                        {
                            reachableTarget = attackTarget;
                            break;
                        }
                    }
                }
                return reachableTarget != null;
            }, int.MaxValue, false, null);
            return reachableTarget;
        }
    }
}
