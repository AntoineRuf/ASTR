using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rogue : Class
{

    public Rogue()
    {
        Initialize();
    }

    public void Initialize()
    {
        HP = 90;
        ATK = 20;
        MP = 4;
        DEF = 0;
        INIT = 100;
    }

}