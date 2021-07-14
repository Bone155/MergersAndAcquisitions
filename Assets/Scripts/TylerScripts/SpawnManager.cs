using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject parentObject;
    public List<GameObject> startBuildPrefabs;
    public Image buildingImage;
    //public TextMeshProUGUI text;
    [Header("Time")]
    public float time = 10.0f;
    public float decreasePerAction = 1.0f;
    [Header("Settings")]
    public float alphaStartIn = 48;
    [HideInInspector]
    public List<GameObject> objects;
    public float timer;
    private bool doOnce = false;
    private static SpawnManager instance;
    private float miniTimer = 0f;
    private float alphaAdd;
    void Start()
    {
        if (objects == null) objects = new List<GameObject>();
        else objects.Clear();
        timer = time;
        instance = this;
        if(buildingImage != null)
        {
            buildingImage.color = new Color(buildingImage.color.r, buildingImage.color.g, buildingImage.color.b, (alphaStartIn / 255));
            alphaAdd = (1 - (alphaStartIn/255))/time;
        }
        miniTimer = 0.0f;
    }
    void Update()
    {
        if (!doOnce)
        {
            Enabled();
        }
        if(timer <= 0)
        {
            if (buildingImage != null)
            {
                buildingImage.color = new Color(buildingImage.color.r, buildingImage.color.g, buildingImage.color.b,1f);
            }
        }
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if (miniTimer < 1.0f) miniTimer += Time.deltaTime;
            else
            {
                if(buildingImage  != null && buildingImage.color.a < 255)
                {
                    alphaAdd = (1 - (buildingImage.color.a / 255)) / timer;
                    buildingImage.color = new Color(buildingImage.color.r, buildingImage.color.g, buildingImage.color.b, buildingImage.color.a + alphaAdd);
                }
                miniTimer = 0.0f;
            }
        }
    }
    public static SpawnManager GetInstance()
    {
        return instance != null ? instance : null;
    }
    // just when it needs to get enabled once
    public void Enabled()
    {
        if (parentObject.transform.childCount > 0.0f && !doOnce)
        {
            foreach (Transform obj in parentObject.transform)
            {
                objects.Add(obj.gameObject);
            }
            doOnce = true;
        }
    }
    // returns if the parent objects are full.
    public void OnButtonClick()
    {
        if (timer > 0.0f)
        {
            if (timer - decreasePerAction <= 0.0f) timer = 0.0f;
            else
            {
                timer -= decreasePerAction;
            }
        }
        if (timer <= 0.0f)
        {
            Spawn();
            timer = time;
            if (buildingImage != null)
            {
                buildingImage.color = new Color(buildingImage.color.r, buildingImage.color.g, buildingImage.color.b, (alphaStartIn / 255));
            }
        }
    }
    public Transform GetParent()
    {
        return parentObject != null ? parentObject.transform:null;
    }
    // Spawns the building in
    public bool Spawn()
    {
        int counter = 0;
        for(int i = 0; i < objects.Count; i++)
        {
            if (objects[i].transform.childCount <= 0)
            {
                if(startBuildPrefabs != null && startBuildPrefabs.Count > 0)
                {
                    GameObject o = Instantiate(startBuildPrefabs[Random.Range(0, startBuildPrefabs.Count - 1)], objects[i].transform);
                    o.transform.position = objects[i].transform.position + Control.GetOffSet(o);
                    return true;
                }
            }
        }
        return false;
    }
}
