using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using Verse.Sound;

namespace Revia_VanillaPsycastExpanded
{
    /// <summary>
    /// Skarne's curse ability
    /// </summary>
    /// <remarks>Mostly reworked <see cref="VanillaPsycastsExpanded.Nightstalker.Ability_Assassinate"/></remarks>
    public class Ability_SkarnesCurse : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            this.target = (targets.FirstOrDefault((GlobalTargetInfo t) => t.Thing is Pawn).Thing as Pawn);
            bool flag = this.target == null;
            if (!flag)
            {
                this.attacksLeft = Mathf.RoundToInt(this.GetPowerForPawn());
                Map map = this.pawn.Map;
                this.originalPosition = this.pawn.Position;
                this.target.stances.stunner.StunFor(this.attacksLeft * 2, this.pawn, true, true, false);
                this.TeleportPawnTo((from c in GenAdjFast.AdjacentCellsCardinal(this.target.Position)
                                     where c.Walkable(map)
                                     select c).RandomElement<IntVec3>());
            }
        }

        public override void Tick()
        {
            base.Tick();
            bool flag = this.attacksLeft > 0;
            if (flag)
            {
                this.attacksLeft--;
                this.DoAttack();
                bool flag2 = this.attacksLeft == 0;
                if (flag2)
                {
                    VPE_DefOf.VPE_Assassinate_Return.PlayOneShot(this.pawn);
                    this.TeleportPawnTo(this.originalPosition);
                }
            }
        }

        private void DoAttack()
        {
            Verb verb = this.pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false).Where(x=>x.verb.GetDamageDef().hediff.injuryProps.bleedRate > 0.01f).MaxBy((VerbEntry v) => VerbUtility.DPS(v.verb, this.pawn)).verb;
            this.pawn.meleeVerbs.TryMeleeAttack(this.target, verb, true);
            this.pawn.stances.CancelBusyStanceHard();
            FleckMaker.AttachedOverlay(this.target, VPE_DefOf.VPE_Slash, Rand.InsideUnitCircle * 0.3f, 1f, -1f);
        }

        private void TeleportPawnTo(IntVec3 c)
        {
            FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(this.pawn, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero, 1f, -1f);
            dataAttachedOverlay.link.detachAfterTicks = 1;
            this.pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
            TargetInfo source = new TargetInfo(c, this.pawn.Map, false);
            FleckMaker.Static(source.Cell, source.Map, FleckDefOf.PsycastSkipInnerExit, 1f);
            FleckMaker.Static(source.Cell, source.Map, FleckDefOf.PsycastSkipOuterRingExit, 1f);
            SoundDefOf.Psycast_Skip_Entry.PlayOneShot(this.pawn);
            SoundDefOf.Psycast_Skip_Exit.PlayOneShot(source);
            base.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(this.pawn, this.pawn.Map, 1f), this.pawn.Position, 60, null);
            base.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(source.Cell, source.Map, 1f), source.Cell, 60, null);
            this.pawn.Position = c;
            this.pawn.Notify_Teleported(true, true);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            Pawn pawn = target.Pawn;
            if (pawn != null && pawn.health.CanBleed)
            {
                return base.ValidateTarget(target, showMessages);
            }
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.attacksLeft, "attacksLeft", 0, false);
            Scribe_Values.Look<IntVec3>(ref this.originalPosition, "originalPosition", default(IntVec3), false);
            Scribe_References.Look<Pawn>(ref this.target, "target", false);
        }

        private int attacksLeft;

        private IntVec3 originalPosition;

        private Pawn target;
    }
}
