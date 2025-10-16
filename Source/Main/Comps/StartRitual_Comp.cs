using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReviaRace
{
    public class StartRitual_Comp : ThingComp
    {
        public StartRitual_CompProperties Props => props as StartRitual_CompProperties;


        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            yield return new Command_Action()
            {
                action = () => Find.WindowStack.Add(ConfirmationDialog(parent, null)),
                defaultLabel = "StartRitual",
            };
        }

        public Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
        {
            Log.Message(parent.Map == null);
            return new Dialog_BeginRitual(Props.ritualDef.LabelCap, null, new TargetInfo(parent), parent.Map, Callback , null, null);
        }
        private bool Callback(RitualRoleAssignments roleAssignments)
        {
            Props.ritualDef.GetInstance().TryExecuteOn(TargetInfo.Invalid, null, null, null, roleAssignments, true);
            return true;
        }
    }

    public class StartRitual_CompProperties : CompProperties
    {
        public StartRitual_CompProperties() : base()
        {
            this.compClass = typeof(StartRitual_Comp);
        }
        public RitualBehaviorDef ritualDef;
    }

}
