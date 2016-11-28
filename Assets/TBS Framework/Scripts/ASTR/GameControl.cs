using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour
{

    public static GameControl control;
    public static List<UnitData> playerData;
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

    public GameControl()
    {
        playerData = new List<UnitData>();
    }

    public static void Save(List<UnitData> pData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        PlayerData data = new PlayerData();
        playerData = pData;
        data.playerData = playerData;


        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            playerData = data.playerData;
            file.Close();
        }
    }
}

[Serializable]
public class PlayerData
{
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