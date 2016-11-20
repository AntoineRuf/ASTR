using UnityEngine;
using System.Collections;

public abstract class Class  {

    public int HP = 0; // health points
    public int ATK = 0; // attack points
    public int DEF = 0; // defense points
    public int MP = 0; // movement points 
    public int INIT = 0; // initiative

    private int M_HP;
    private int M_ATK;
    private int M_MP;
    private int M_INIT;

    private int R_HP;
    private int R_ATK;
    private int R_MP;
    private int R_INIT;

    private int W_HP;
    private int W_ATK;
    private int W_MP;
    private int W_INIT;

    public void Initialize(Class firstClass, Class secondClass)
    {
        HP += firstClass.HP + secondClass.HP;
        ATK += firstClass.ATK + secondClass.ATK;
        MP += firstClass.MP;
        INIT += firstClass.INIT + secondClass.INIT;
    } 


}
