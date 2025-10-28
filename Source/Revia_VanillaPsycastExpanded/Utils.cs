using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    internal static class Utils
    {
        /// <summary>
        /// Partial copy of <see cref="Pawn.DoKillSideEffects(DamageInfo?, Hediff, bool)"/>
        /// </summary>
        public static void Notify_KilledPawn(Pawn killer, Pawn victim, DamageInfo? dinfo)
        {
            RecordsUtility.Notify_PawnKilled(victim, killer);
            Pawn_EquipmentTracker pawn_EquipmentTracker = killer.equipment;
            if (pawn_EquipmentTracker != null)
            {
                pawn_EquipmentTracker.Notify_KilledPawn();
            }
            if (victim.RaceProps.Humanlike)
            {
                Pawn_NeedsTracker pawn_NeedsTracker = killer.needs;
                if (pawn_NeedsTracker != null)
                {
                    Need_KillThirst need_KillThirst = pawn_NeedsTracker.TryGetNeed<Need_KillThirst>();
                    if (need_KillThirst != null)
                    {
                        need_KillThirst.Notify_KilledPawn(dinfo);
                    }
                }
            }
            if (killer.health.hediffSet != null)
            {
                for (int i = 0; i < killer.health.hediffSet.hediffs.Count; i++)
                {
                    killer.health.hediffSet.hediffs[i].Notify_KilledPawn(victim, dinfo);
                }
            }
            if (HistoryEventUtility.IsKillingInnocentAnimal(killer, victim))
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, killer.Named(HistoryEventArgsNames.Doer), victim.Named(HistoryEventArgsNames.Victim)), true);
            }
        }
        
    }
}
