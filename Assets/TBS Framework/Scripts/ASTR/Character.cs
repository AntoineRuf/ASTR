using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// USELESS CLASS 
public class Character : Alien { // Oui, il faudra changer ce nom de classe, c'est cramé

    public List<Skill> _activeSkillList;
    public List<Skill> _passiveSkillList;
    public Class _firstClass;
    public Class _secondClass;
    public int HP = 0; // health points
    public int ATK = 0; // attack points
    public int DEF = 0; // defense points
    public int MP = 0; // movement points 

    public new List<Buff> Buffs;
	
    /// <summary>
    /// Initialize character attributes with his classes ones
    /// </summary>
	public override void Initialize()
    {
        base.Initialize();
        transform.position += new Vector3(0, 0, -1);

        HP += _firstClass.HP;
        HP += _secondClass.HP;

        ATK += _firstClass.ATK;
        ATK += _secondClass.ATK;

        DEF += _firstClass.DEF;
        DEF += _secondClass.DEF;

        MP += _firstClass.MP;
    }
}
