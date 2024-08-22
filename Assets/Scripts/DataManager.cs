using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;

/*
    A class representing the coordinates of a 2D tile. Used by the
    UserData class to represent an array of tiles which are
    currently soil tiles.
*/
[Serializable]
public class Tile {
    public int x;
    public int y;
    public Tile(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

/*
    A class representing the data specific to the player.
*/
[Serializable]
public class UserData {
    public int coins = 0;
    public int xpLevel = 1;
    public int xpPoints = 1;
    public List<Tile> tiles = new List<Tile>();
}

/*
    A class representing information about one employee frog.
*/
[Serializable]
public class EmployeeData {
    public string id;
    public string name;
    public string desc;
    public int cost;
    public string anim; // Idle animation.
    public bool unlocked = false;
    public bool owned = false;
}

/*
    Stores the list of employee datas. Cannot be done in a more
    succint way since Unity does not support Arrays as JSON
    objects, hence a redundant outermost object is needed.
*/
[Serializable]
public class EmployeesData {
    public List<EmployeeData> employeesList;
}

public class DataManager : MonoBehaviour {
    public static DataManager Instance { get; private set;}
    private const string USER_DATA_FILE_PATH = "Assets/Data/userdata.json";
    private const string EMPLOYEE_DATA_FILE_PATH = "Assets/Data/employeedata.json";
    private UserData userData;
    private EmployeesData employeesData;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    void Start() {
        userData = LoadData<UserData>(USER_DATA_FILE_PATH);
        employeesData = LoadData<EmployeesData>(EMPLOYEE_DATA_FILE_PATH);
        print(employeesData);
    }
    private string ConvertToJson() {
        return JsonUtility.ToJson(userData);
    }
    private T ConvertFromJson<T>(string json) {
        return JsonUtility.FromJson<T>(json);
    }
    public void SaveUserData() {
        File.WriteAllText(USER_DATA_FILE_PATH, ConvertToJson());
    }
    private T LoadData<T>(string filePath) {
        if (File.Exists(filePath)) {
            print("found file: " + filePath);
            return ConvertFromJson<T>(File.ReadAllText(filePath));
        } 
        return Activator.CreateInstance<T>();
    }
    public UserData GetUserData() {
        return userData;
    }

    public List<EmployeeData> GetEmployeesData() {
        return employeesData.employeesList;
    }
}