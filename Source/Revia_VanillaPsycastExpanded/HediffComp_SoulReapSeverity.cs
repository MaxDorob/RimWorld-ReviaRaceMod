using ReviaRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Revia_VanillaPsycastExpanded
{
    public class HediffComp_SoulReapSeverity : HediffComp
    {
        public HediffComp_SoulReapSeverity() : base() { }
        public HediffCompProperties_SoulReapSeverity Props => (HediffCompProperties_SoulReapSeverity)props;
        protected float Severity => Pawn.GetSoulReapTier() * Props.severityPerSoulReap;
        public override void CompPostMake()
        {
            base.CompPostMake();
            parent.Severity = Severity;
        }
        public override void CompPostMerged(Hediff other)
        {
            base.CompPostMerged(other);
            parent.Severity = Severity;
        }
    }
    public class HediffCompProperties_SoulReapSeverity : HediffCompProperties
    {
        public HediffCompProperties_SoulReapSeverity() : base()
        {
            compClass = typeof(HediffComp_SoulReapSeverity);
        }
        public float severityPerSoulReap = 0.1f;
    }
}
