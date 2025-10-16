using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public class CompUsable_DisableableBySettings : CompUsable
    {
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
        {
            if (!ReviaRaceMod.Settings.oldInvokeBlessing)
            {
                return Enumerable.Empty<FloatMenuOption>();
            }
            return base.CompFloatMenuOptions(myPawn);
        }
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            if(!ReviaRaceMod.Settings.oldInvokeBlessing)
            {
                return "DisabledBySettings".Translate();
            }
            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
