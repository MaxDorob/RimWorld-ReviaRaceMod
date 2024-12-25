using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    public class HediffComp_SkarnesCurse : HediffComp
    {
        private int ticksToDamage;
        public HediffComp_SkarnesCurse() : base()
        {
        }
        public override void CompPostMake()
        {
            base.CompPostMake();
            ticksToDamage = Props.interval.RandomInRange;
        }
        HediffCompProperties_SkarnesCurse Props => (HediffCompProperties_SkarnesCurse)props;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (--ticksToDamage <= 0)
            {
                Pawn.TakeDamage(DInfo);
                ticksToDamage = Props.interval.RandomInRange;
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref ticksToDamage, nameof(ticksToDamage));
        }
        private DamageInfo DInfo
        {
            get
            {
                var result = new DamageInfo(DamageDefs.RandomElement(), Props.damage.RandomInRange);
                result.SetIgnoreArmor(true);
                result.SetAllowDamagePropagation(false);
                return result;
            }
        }
        private IEnumerable<DamageDef> DamageDefs
        {
            get
            {
                yield return Defs.Revia_MysticalScratch;
                yield return Defs.Revia_MysticalMediumBleedingScratch;
                yield return Defs.Revia_MysticalHeavyBleedingScratch;
            }
        }
    }
    public class HediffCompProperties_SkarnesCurse : HediffCompProperties
    {
        public HediffCompProperties_SkarnesCurse() : base()
        {
            this.compClass = typeof(HediffComp_SkarnesCurse);
        }
        public IntRange interval = new IntRange(150, 450);
        public FloatRange damage = new FloatRange(1, 3);
    }
}
