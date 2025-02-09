using Verse;

namespace ReviaRace
{
	public class HediffComp_EntropyIncrease : HediffComp
	{
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
			var pawn = parent.pawn;
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				pawn.psychicEntropy.TryAddEntropy(1f, null, true, true);
			}
			if (pawn.psychicEntropy.EntropyValue >= pawn.psychicEntropy.MaxEntropy)
			{
				pawn.MentalState.RecoverFromState();
			}
		}
    }
}