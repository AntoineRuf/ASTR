using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    public class WeaknessTrapSkill : Skill
    {
        public override string Name 
        {
            get { return "Weakness Trap"; }
            set { }
        }     
        public override int MinRange
        {
            get { return 2; }
            set { }
        }
        public override int MaxRange
        {
            get { return 3; }
            set { }
        }
        public override int MinDamage
        {
            get { return 0; }
            set { }
        }
        public override int MaxDamage
        {
            get { return 0; }
            set { }
        }
        public override int Cooldown
        {
            get { return 3; }
            set { }
        }
        public override int AoERange
        {
            get { return 0; }
            set { }
        }
        public override bool SightNeeded
        {
            get { return false; }
            set { }
        }
        public override bool AlignmentNeeded
        {
            get { return false; }
            set { }
        }
        public override bool CanTargetEmptyCell
        {
            get { return true; }
            set { }
        }
        public override bool CanTargetSelf
        {
            get { return false; }
            set { }
        }
        public override bool CanTargetAllies
        {
            get { return false; }
            set { }
        }
        public override bool CanTargetEnemies
        {
            get { return false; }
            set { }
        }
        public override int isAoE
        {
            get { return 0; }
            set { }
        }

        public override void Apply(Unit caster, List<Cell> receiver, CellGrid cellGrid)
        {
            Trap trap = new WeaknessTrap(receiver.First(), cellGrid.trapmanager, caster);
            UnityEngine.Debug.Log("TJe passe par la fonction apply.");
            cellGrid.trapmanager.AddTrap(trap);
            UnityEngine.Debug.Log("Trap cree.");
        }
    }
}
