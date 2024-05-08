using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;

public class UserData : MonoBehaviour
{
    public Dictionary<string, string> data;
    private string USER_DATA_PATH = "Assets/Data/userdata.txt";
 
    void Start()
    {
        data = new Dictionary<string, string>();
        //data = DeserializeData(USER_DATA_PATH);
        data = ReadFromFile(USER_DATA_PATH);
        // Uncomment the following line to clear out the user's
        // data and set all needed values to their defaults.
        // ResetData();
    }
 
    public void SaveData()
    {
        //SerializeData(data, USER_DATA_PATH);
        Debug.Log("Data saved: " + data);
        WriteToFile(data, USER_DATA_PATH);
    }

    public void SaveKeyAndValue(string key, string value) {
        data[key] = value;
        SaveData();
    }

    public static void WriteToFile(Dictionary<string, string> data, string path) {
        File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }

    public static Dictionary<string, string> ReadFromFile(string path) {
        if (File.Exists(path)) {
            string dataString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(dataString);
        } else {
            return new Dictionary<string, string>();
        }
    }

    public static void SerializeData(Dictionary<string, string> data, string path)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, data);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            fs.Close();
        }
    }
 
    public static Dictionary<string, string> DeserializeData(string path)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
 
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = formatter.Deserialize(fs) as Dictionary<string, string>;
            }
            catch (SerializationException e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        return data;
    }

    void ResetData() {
        data.Clear();
        SaveData();
        data["soilTiles"] = "[]";
        SaveData();
    }
}