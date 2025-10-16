using Verse;

namespace ReviaRace
{
    public class HediffComp_EntropyIncrease : HediffComp
    {
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);
            var pawn = parent.pawn;
            pawn.psychicEntropy.TryAddEntropy(delta / 10f, null, true, true);

            if (pawn.psychicEntropy.EntropyValue >= pawn.psychicEntropy.MaxEntropy)
            {
                pawn.MentalState.RecoverFromState();
            }
        }
    }
}