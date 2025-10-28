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
    public class RitualRoleWithReviaGene : RitualRole
    {
        public RitualRoleWithReviaGene()
        {
        }
        public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
        {
            if (base.AppliesToPawn(p, out reason, selectedTarget, ritual, assignments, precept, skipReason))
            {
                return false;
            }
            bool ritualWillChangeSomething = p?.genes?.Xenotype != Defs.XenotypeDef;
            if (!ritualWillChangeSomething)
            {
                reason = "MessageRitualRolePawnIsAlreadyRevia".Translate();
                return false;
            }
            if (!ReviaRaceMod.Settings.requireReviaGene)
            {
                return true;
            }
            bool pawnCanBeConverted = p?.genes?.GenesListForReading.Any(g => g.def.defName.Contains("Revia")) ?? false;
            if (!pawnCanBeConverted)
            {
                reason = "MessageRitualRoleAnyRevianGeneRequired".Translate();
                return false;
            }
            return true;
        }
        public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn pawn = null, bool skipReason = false)
        {
            reason = null;
            return false;
        }
    }
}
