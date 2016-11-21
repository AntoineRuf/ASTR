using UnityEngine;
using System.Collections;

public class Warrior : Class
{

    public Warrior()
    {
        Initialize();
    }

    public void Initialize()
    {
        HP = 120;
        ATK = 30;
        MP = 3;
        DEF = 0;
        INIT = 30;
    }

}