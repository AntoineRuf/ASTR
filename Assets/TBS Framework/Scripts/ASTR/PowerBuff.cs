using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    public class AttackBuffSkill : Skill
    {
        public override void Apply(Unit caster, Unit receiver)
        {
            AttackBuff buff = new AttackBuff();
            buff.Apply(receiver);
            caster.ActionPoints--;
        }

        public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid)
        {
            throw new NotImplementedException();
        }
    }
}
