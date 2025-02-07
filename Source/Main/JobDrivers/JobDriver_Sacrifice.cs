using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReviaRace
{
    public class JobDriver_Sacrifice : RimWorld.JobDriver_Sacrifice
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            Pawn victim = this.Victim;
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false);
            yield return Toils_General.Wait(35, TargetIndex.None);
            Toil execute = ToilMaker.MakeToil("MakeNewToils");
            execute.initAction = delegate ()
            {
                Lord lord = this.pawn.GetLord();
                LordJob_Ritual lordJob_Ritual;
                if (lord != null && (lordJob_Ritual = (lord.LordJob as LordJob_Ritual)) != null)
                {
                    lordJob_Ritual.pawnsDeathIgnored.Add(victim);
                }
                ExecutionUtility.DoExecutionByCut(this.pawn, victim, 0, false);
                ThoughtUtility.GiveThoughtsForPawnExecuted(victim, this.pawn, PawnExecutionKind.GenericBrutal);
                TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
                {
                    this.pawn,
                    victim
                });
                victim.health.killedByRitual = true;
            };
            execute.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return Toils_Reserve.Release(TargetIndex.A);
            yield return execute;
        }
    }
}
