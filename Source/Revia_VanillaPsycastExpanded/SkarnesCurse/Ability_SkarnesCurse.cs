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
using Verse.AI;
using ReviaRace.Helpers;
using VFECore.Abilities;

namespace Revia_VanillaPsycastExpanded
{
    /// <summary>
    /// Skarne's curse ability
    /// </summary>
    /// <remarks>Mostly reworked <see cref="Ability_Killskip"/></remarks>
    public class Ability_SkarnesCurse : VFECore.Abilities.Ability
    {

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            attacksLeft = Mathf.RoundToInt(GetPowerForPawn());
            originalPosition = pawn.Position;
            this.AttackTarget((LocalTargetInfo)targets[0]);
            this.TryQueueAttack((LocalTargetInfo)targets[0]);
        }
        public override float GetPowerForPawn()
        {
            return Mathf.Pow(2, Mathf.Max(pawn.GetSoulReapTier() - 7, 0));
        }
        private void TryQueueAttack(LocalTargetInfo target)
        {
            if (attacksLeft > 0)
            {
                this.attackInTicks = Find.TickManager.TicksGame + this.def.castTime;
            }
            else
            {
                this.attackInTicks = -1;
                TeleportPawnTo(originalPosition);
                VPE_DefOf.VPE_Assassinate_Return.PlayOneShot(this.pawn);
            }
        }

        public override void Tick()
        {
            base.Tick();
            bool flag = this.attackInTicks != -1 && Find.TickManager.TicksGame >= this.attackInTicks;
            if (flag)
            {
                this.attackInTicks = -1;
                Pawn pawn = this.FindAttackTarget();
                bool flag2 = pawn != null;
                if (flag2)
                {
                    this.AttackTarget(pawn);
                    this.TryQueueAttack(pawn);
                }
                else
                {
                    TeleportPawnTo(originalPosition);
                    attacksLeft = 0;
                }
            }
        }

        private void AttackTarget(LocalTargetInfo target)
        {
            TeleportPawnTo(target.Cell);

            VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = true;
            Verb verb = this.pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false).Where(x => x.verb.GetDamageDef().hediff.injuryProps.bleedRate > 0.01f).MaxBy((VerbEntry v) => VerbUtility.DPS(v.verb, this.pawn)).verb;
            this.pawn.meleeVerbs.TryMeleeAttack(target.Pawn, verb, true);
            ApplyHediff(target.Pawn);
            VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = false;
            castSounds.RandomElement<SoundDef>().PlayOneShot(this.pawn);
            FleckMaker.AttachedOverlay(target.Thing, VPE_DefOf.VPE_Slash, Rand.InsideUnitCircle * 0.3f, 1f, -1f);
            attacksLeft--;
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
            this.pawn.stances.SetStance(new Stance_Mobile());
        }

        private Pawn FindAttackTarget()
        {
            TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
            return AttackTargetFinder.BestAttackTarget(this.pawn, flags, delegate (Thing x)
            {
                Pawn pawn = x as Pawn;
                return pawn != null && !pawn.Dead && ValidateTarget(pawn) && !pawn.health.hediffSet.HasHediff(def.GetModExtension<AbilityExtension_Hediff>().hediff);
            }, 0f, 999999f, default(IntVec3), float.MaxValue, false, true, false, false) as Pawn ?? (Pawn)AttackTargetFinder.BestAttackTarget(this.pawn, flags, delegate (Thing x)
            {
                Pawn pawn = x as Pawn;
                return pawn != null && !pawn.Dead && ValidateTarget(pawn);
            }, 0f, 999999f, default(IntVec3), float.MaxValue, false, true, false, false);
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
        public override void CheckCastEffects(GlobalTargetInfo[] targetsInfos, out bool cast, out bool target, out bool hediffApply)
        {
            base.CheckCastEffects(targetsInfos, out cast, out target, out hediffApply);
            hediffApply = false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.attackInTicks, "attackInTicks", -1, false);
            Scribe_Values.Look<int>(ref this.attacksLeft, "attacksLeft", 0, false);
            Scribe_Values.Look<IntVec3>(ref this.originalPosition, "originalPosition", default(IntVec3), false);
        }

        private int attackInTicks = -1;

        private static List<SoundDef> castSounds = new List<SoundDef>
        {
            VPE_DefOf.VPE_Killskip_Jump_01a,
            VPE_DefOf.VPE_Killskip_Jump_01b,
            VPE_DefOf.VPE_Killskip_Jump_01c
        };
        private int attacksLeft;
        private IntVec3 originalPosition;
    }
}
