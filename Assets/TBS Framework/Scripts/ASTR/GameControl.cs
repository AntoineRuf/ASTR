using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour
{

    public static GameControl control;
    public static AllUnitsData UnitLayout;
    // Use this for initialization
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        AllUnitsData data = new AllUnitsData();
        data.Unitdata = new List<UnitData>();

        for (int i = 0; i < 6; ++i)
        {
            string testunit1name = "Labite";
            string testunit1class = "Warrior";
            int testunit1player = 1;
            List<Skill> testunit1list = new List<Skill>();
            testunit1list.Add(new Galvanize());
            testunit1list.Add(new RagingBull());
            testunit1list.Add(new ShieldBash());
            testunit1list.Add(new Whirlwind());

            UnitData unit1 = new UnitData();
            unit1.Player = testunit1player;
            unit1.Name = testunit1name;
            unit1.Class = testunit1class;
            unit1.Skills = testunit1list;

            data.Unitdata.Add(unit1);
        }


        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            UnitLayout = (AllUnitsData)bf.Deserialize(file);
            file.Close();
        }
    }
}

[Serializable]
public class AllUnitsData
{
    public List<UnitData> Unitdata;
}

[Serializable]
public class UnitData
{
    public string Class;
    public string Name;
    public int Player;
    public List<Skill> Skills;
}