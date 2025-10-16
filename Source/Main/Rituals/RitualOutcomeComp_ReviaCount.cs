using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReviaRace.Rituals
{
    public class RitualOutcomeComp_ReviaCount : RitualOutcomeComp_Quality
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_DataThingPresence();
        }
        public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
        {
            base.Tick(ritual, data, progressAmount);
            if (ritual == null)
            {
                Log.Warning("Ritual is null");
            }
            if (ritual.PawnsToCountTowardsPresence == null)
            {
                Log.Warning("PawnsToCountTowardsPresence is null");
                return;
            }
            RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
            foreach (Pawn pawn in ritual.PawnsToCountTowardsPresence.Where(p => p?.IsRevia() ?? true))
            {
                if (GatheringsUtility.InGatheringArea(pawn.Position, ritual.Spot, pawn.MapHeld))
                {
                    Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
                    if (pawn == null || presentForTicks == null)
                    {
                        Log.Message($"{pawn.ToString() ?? "null"},  {presentForTicks?.ToString() ?? "null"}");
                    }
                    if (!presentForTicks.ContainsKey(pawn))
                    {
                        presentForTicks.Add(pawn, 0f);
                    }
                    Thing key = pawn;
                    presentForTicks[key] += progressAmount;
                }
            }
        }
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            int num = 0;
            RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
            float num2 = (ritual.DurationTicks != 0) ? ((float)ritual.DurationTicks) : ritual.TicksPassedWithProgress;
            foreach (KeyValuePair<Thing, float> keyValuePair in ritualOutcomeComp_DataThingPresence.presentForTicks)
            {
                Pawn p = (Pawn)keyValuePair.Key;
                if (this.Counts(ritual.assignments, p) && keyValuePair.Value >= num2 / 2f)
                {
                    num++;
                }
            }
            return (float)((this.curve != null) ? ((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x)) : num);
        }
        protected bool Counts(RitualRoleAssignments assignments, Pawn p)
        {
            if (assignments != null && assignments.Ritual == null && assignments.Required(p))
            {
                return false;
            }
            RitualRole ritualRole = (assignments != null) ? assignments.RoleForPawn(p, true) : null;
            return p.RaceProps.Humanlike && p.IsRevia();
        }
        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            IEnumerable<Pawn> additionalCollection = [];
            var sacrificerRole = assignments.GetRole("sacrificer");
            if (sacrificerRole != null)
            {
                additionalCollection = assignments.AssignedPawns(sacrificerRole);
            }
            
            int num = assignments.Participants.Union(additionalCollection).Count((Pawn p) => this.Counts(assignments, p));
            float quality = this.curve.Evaluate(num);
            return new QualityFactor
            {
                label = "RitualPredictedOutcomeDescReviaCount".Translate(),
                count = num + " / " + Mathf.Max(MaxValue, num),
                qualityChange = this.ExpectedOffsetDesc(true, quality),
                quality = quality,
                positive = true,
                priority = 4.1f //Right after ParticipantCount
            };
        }
    }
}
