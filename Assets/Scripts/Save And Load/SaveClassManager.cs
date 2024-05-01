using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveClassManager : MonoBehaviour
{
    List<ISaveable> saveables = new List<ISaveable>();

    public Dictionary<string, object> SaveDataDictonary = new();

    private Dictionary<string ,ISaveable> savealbleDictionary = new();

    private Dictionary<string ,int> orderDictionary = new();


    private string saveFilePath;



    private SaveClassManagerRefrence refrence;

    private void SetRefrence()
    {
        refrence = (SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    private void Awake()
    {
        SetRefrence();
        saveFilePath = Application.persistentDataPath + "/savedData.dat";
    }

    /// <summary>
    /// Will get called when an ISaveable will become enabled to add its name, its self and it's execution 
    /// order to dictionaries that are related.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="order"></param>
    public void AddISaveableToDictionary(string key, ISaveable item, int order)
    {
        savealbleDictionary.Add(key, item);
        orderDictionary.Add(key, order);
    }

    /// <summary>
    /// Will start the saving procces and iterates through every ISaveable
    /// </summary>
    public void SaveCurrentState()
    {
        SaveDataDictonary.Clear();
        foreach (var item in savealbleDictionary)
        {
            string name = item.Key;
            object saveObject = item.Value.Save();
            SaveDataDictonary.Add(name, saveObject);
        }
        SaveData(SaveDataDictonary);

    }

    /// <summary>
    /// Will write the Save Data file
    /// </summary>
    /// <param name="data"></param>
    private void SaveData(Dictionary<string, object> data)
    {
        BinaryFormatter formatter = new();
        FileStream fileStream = File.Create(saveFilePath);

        //SaveDataClass saveClass = new(data);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    /// <summary>
    /// Will start the proccess of loading every data that are in the list.
    /// </summary>
    public void LoadSavedGame()
    {
        Dictionary<string,object> savedData = LoadData();
        Debug.Log(savedData.Count + " Saved data dictionary count");
        actionArray = new Action[orderDictionary.Count];
        if (savedData != null)
        {
            foreach(var item in savedData)
            {
                if (savealbleDictionary.ContainsKey(item.Key))
                {
                    //actionArray[0] = () => savealbleDictionary[item.Key].Load(savedData[item.Key]);
                    Action target = () => savealbleDictionary[item.Key].Load(savedData[item.Key]);
                    SetOrder(target, item.Key);
                }
            }
            ExecuteOrder();
        }
        else
        {
            Debug.Log("dictionary is null");
        }
    }

    private Dictionary<string, object> LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Open(saveFilePath, FileMode.Open);

            Dictionary<string,object> data = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        Debug.Log("Couldnt find the file");
        return null;
    }


    private delegate void MyDelegate();
    Action[] actionArray;

    /// <summary>
    /// adds the related order of execution method to the dictionary
    /// </summary>
    /// <param name="method"></param>
    /// <param name="name"></param>
    private void SetOrder(Action method, string name)
    {
        foreach (var item in orderDictionary)
        {
            Debug.Log(item.Key);
        }
        if (orderDictionary.ContainsKey(name))
        {
            Debug.Log(orderDictionary[name]);
            Debug.Log(actionArray.Length);
            actionArray[orderDictionary[name]] = method;
        }
        else
        {
            Debug.Log("Dictionary doesnt contain the key");
        }
    }

    /// <summary>
    /// Will execute the load actions that are in the array in the proper order.
    /// </summary>
    private void ExecuteOrder()
    {
        for(int i = 0; i < actionArray.Length; i++)
        {
            actionArray[i]();
        }
    }

}