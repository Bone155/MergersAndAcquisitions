using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    [Header("Building")]
    public List<GameObject> buildingPrefabs;
    // names for saves
    private string coinSaveName = "coinSave";
    private string buildingSaveName = "buildingSave";
    private string buildingCount = "buildCountSave";
    private string path = "/buildObjects.dat";
    private string rtSaveName = "rtSave";
    private string mbSaveName = "mbSave";
    // does this once
    private bool doOnce = false;
    private void Start()
    {
    }
    private void Update()
    {
        if (!doOnce)
        {
            Load();
            doOnce = true;
        }
    }
    private void OnApplicationPause(bool pause)
    {
        Save();
    }
    public void SaveButton()
    {
        Save();
    }
    // will be saving and loading for the application quitting
    private void OnApplicationQuit()
    {
        Save();
    }
    // will be using this to load all stuff
    private void Load()
    {
        BuildingLoad();
        CoinLoad();
    }
    // will be using to save all stuff
    private void Save()
    {
        BuildingSave();
        CoinSave();
    }
    // used for loading the building
    public void BuildingSave()
    {
        if (SpawnManager.GetInstance() != null)
        {
            BuildData data = new BuildData();
            int counter = 0;
            
            foreach (Transform obj in SpawnManager.GetInstance().parentObject.transform)
            {
                if (obj.childCount > 0)
                {
                    if(obj.GetChild(0).GetComponent<PositionTracker>() != null)
                    {
                        PositionTracker type = obj.GetChild(0).GetComponent<PositionTracker>();
                        data.buildTypeList.Add(type.buildingType);
                        data.buildingTierList.Add(type.tier);
                        data.objectsCounter.Add(counter);
                    }
                }
                counter++;
            }
            if (CurrencyManager.InstanceActive())
            {
                data.currency = CurrencyManager.GetInstance().Get();
            }
            BinaryFormatter bf = new BinaryFormatter();
            string newPath = Application.persistentDataPath + path;
            FileStream stream = new FileStream(newPath, FileMode.Create);
            bf.Serialize(stream, data);
            stream.Close();
            
        }
    }
    public void BuildingLoad()
    {
        string newPath = Application.persistentDataPath + path;
        if (File.Exists(newPath) || buildingPrefabs != null && buildingPrefabs.Count > 0)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(newPath, FileMode.Open);
                BuildData data = bf.Deserialize(stream) as BuildData;
                stream.Close();
                int counter = 0;
                int dataCounter = 0;
                if (CurrencyManager.InstanceActive())
                {
                    CurrencyManager.GetInstance().Set(data.currency);
                }
                foreach (Transform obj in SpawnManager.GetInstance().parentObject.transform)
                {
                    if (dataCounter < data.objectsCounter.Count && data.objectsCounter[dataCounter] == counter)
                    {
                        foreach (GameObject oj in buildingPrefabs)
                        {
                            PositionTracker tracker;
                            if (oj.TryGetComponent<PositionTracker>(out tracker) && dataCounter < data.buildTypeList.Count)
                            {
                                if (data.buildTypeList[dataCounter] == tracker.buildingType)
                                {
                                    GameObject o = Instantiate(oj, obj.transform);
                                    PositionTracker tmp;
                                    if (o.TryGetComponent<PositionTracker>(out tmp))
                                    {
                                        tmp.tier = data.buildingTierList[dataCounter];
                                    }
                                    o.transform.position = obj.transform.position + Control.GetOffSet(o);
                                    break;
                                }
                            }
                        }
                        dataCounter++;
                    }
                    if (dataCounter >= data.buildTypeList.Count) break;
                    counter++;
                }
            }
            catch (IOException)
            {
            }
        }
    }
    public void CoinLoad()
    {
        if (CurrencyManager.GetInstance() != null)
        {
            CurrencyManager.GetInstance().Set(PlayerPrefs.GetInt(coinSaveName));
            CurrencyManager.GetInstance().setRT(PlayerPrefs.GetInt(rtSaveName));
            if(SpawnManager.GetInstance() != null)
            {
                SpawnManager.GetInstance().decreasePerAction = CurrencyManager.GetInstance().getRT();
            }
            CurrencyManager.GetInstance().setMB(PlayerPrefs.GetInt(mbSaveName));
        }
    }
    public void CoinSave()
    {
        if (CurrencyManager.GetInstance() != null)
        {
            PlayerPrefs.SetInt(coinSaveName, (int)CurrencyManager.GetInstance().Get());
            PlayerPrefs.SetInt(rtSaveName, (int)CurrencyManager.GetInstance().getRT());
            PlayerPrefs.SetInt(mbSaveName, (int)CurrencyManager.GetInstance().getMB());
        }
    }
}
[Serializable]
public class BuildData
{
    public List<PositionTracker.TYPE> buildTypeList = new List<PositionTracker.TYPE>();
    public List<PositionTracker.TIER> buildingTierList = new List<PositionTracker.TIER>();
    public List<int> objectsCounter = new List<int>();
    public long currency = 0;

}
