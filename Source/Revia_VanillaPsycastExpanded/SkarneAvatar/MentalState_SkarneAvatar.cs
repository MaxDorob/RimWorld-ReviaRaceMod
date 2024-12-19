using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using Verse.AI;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    public class MentalState_SkarneAvatar : MentalState_Berserk
    {
        public MentalState_SkarneAvatar() : base() { }
        public override bool ForceHostileTo(Thing t)
        {
            return false;
        }
        public override bool ForceHostileTo(Faction f)
        {
            return false;
        }
        public override void PreStart()
        {
            pawn.health.AddHediff(Defs.SkarneAvatar);
            base.PreStart();
        }
        public override void PostEnd()
        {
            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(Defs.SkarneAvatar));
            base.PostEnd();
            Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.PsychicComa, pawn, null);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(2500 * 48);
            pawn.health.AddHediff(hediff, null, null, null);
        }
    }
}
