using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour
{

    public static GameControl control;
    public static List<UnitData> playerData;
    public static List<PlayerData> playerData = new List<PlayerData>();
    public List<UnitData> UnitLayout;
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

    public static void Save(PlayerData pData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        List<PlayerData> data = new List<PlayerData>();
        foreach (var currentPlayerDate in playerData)
        {
            data.Add(currentPlayerDate);
        }
        data.Add(pData);
        playerData.Add(pData);
        
        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            List<PlayerData> data = (List<PlayerData>)bf.Deserialize(file);
            playerData = data;
            file.Close();
        }
    }

    public static void Delete()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        List<PlayerData> data = null;

        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
public class PlayerData
{
    public string squadName;
    public List<UnitData> playerData;
}

[Serializable]
public class UnitData
{
    public string Class;
    public string Name;
    public int Player;
    public List<Skill> Skills;
}