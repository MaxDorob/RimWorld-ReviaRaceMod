using ReviaRace.Helpers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public class PreceptComp_KnowsMemoryThought_ExceptRevia : PreceptComp_KnowsMemoryThought
    {
        public override void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
        {
            if (!member.IsRevia())
            {
                base.Notify_MemberWitnessedAction(ev, precept, member);
            }
        }
    }
}
