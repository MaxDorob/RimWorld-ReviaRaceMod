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
    public class RitualRoleRevia : RitualRole
    {
        public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn pawn = null, bool skipReason = false)
        {
            if (!base.AppliesIfChild(pawn, out reason, skipReason))
            {
                return false;
            }
            if (pawn.IsRevia())
            {
                return true;
            }
            if (!skipReason)
            {
                reason = "MessageRitualRoleMustBeRevia".Translate(base.LabelCap);
            }
            return false;
        }
    }
}
