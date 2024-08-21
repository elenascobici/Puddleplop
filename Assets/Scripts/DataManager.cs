using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;

[Serializable]
public class Tile
{
    public int x;
    public int y;
}

[Serializable]
public class UserData
{
    public int coins = 0;
    public int xpLevel = 1;
    public int xpPoints = 1;
    public List<Tile> tiles = new List<Tile>();
}

public class DataManager : MonoBehaviour {
    public static DataManager Instance { get; private set;}
    private const string FILEPATH = "Assets/Data/userdata.json";
    private UserData userData;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    void Start() {
        LoadUserData();
    }
    private string ConvertToJson() {
        return JsonUtility.ToJson(userData);
    }
    private UserData ConvertFromJson(string json) {
        return JsonUtility.FromJson<UserData>(json);
    }
    public void SaveUserData() {
        File.WriteAllText(FILEPATH, ConvertToJson());
    }
    private void LoadUserData() {
        if (File.Exists(FILEPATH)) {
            userData = ConvertFromJson(File.ReadAllText(FILEPATH));
        } else {
            userData = new UserData();
        }
    }
    public UserData GetUserData() {
        return userData;
    }
}