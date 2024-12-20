using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using ReviaRace.Helpers;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    public class AbilityExtension_ReviaPsycast : AbilityExtension_Psycast
    {
        public int soulReapLevel = 1;
        public bool PrereqsCompleted(CompAbilities compAbilities)
        {
            return compAbilities.Pawn.IsRevia() && compAbilities.Pawn.GetSoulReapTier() >= soulReapLevel;
        }
        public override bool IsEnabledForPawn(Ability ability, out string reason)
        {
            if (ability.pawn.GetSoulReapTier() < soulReapLevel)
            {
                reason = "ReviaRequiredTailCount".Translate(soulReapLevel);
                return false;
            }
            return base.IsEnabledForPawn(ability, out reason);
        }
        public string GetTooltipText(CompAbilities abilities, AbilityDef ability)
        {
            var pawn = abilities.Pawn;
            bool unlockable = !abilities.HasAbility(ability) && (ability.Psycast().PrereqsCompleted(abilities) && pawn.health.hediffSet.GetFirstHediff<Hediff_PsycastAbilities>().points >= 1);
            var sb = new StringBuilder(string.Format("{0}\n\n{1}{2}", ability.LabelCap, ability.description, unlockable ? ("\n\n" + "VPE.ClickToUnlock".Translate().Resolve().ToUpper()) : ""));
            sb.AppendLine();
            sb.Append("ReviaRequiredTailCount".Translate(soulReapLevel));
            return sb.ToString();
        }
    }
}
