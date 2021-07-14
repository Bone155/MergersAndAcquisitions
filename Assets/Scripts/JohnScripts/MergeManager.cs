using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    //From GridSpawner
    public int x, y;
    public GameObject gNode;
    public Vector3[] startingLocation;
    public GridNodes[,] grid;

    //Enum "MERGE" will track the levels of each property, only properties of the same level can be combined.
    public enum TIER { T0, T1, T2, T3, T4 };
    public TIER tier;

    //Enum "TYPE" will track what each building is.
    public enum TYPE { Cafe, Office, BookStore, ClothingStore };
    public TYPE buildingType;

    //Enum "COLOR" is just used for debugging. Really not that important.
    public enum COLOR { WHITE, RED, BLUE, GOLD };
    public COLOR lerpColor;

    public float revenuePerSec, startTime, lerpSpeed;
    public bool levelUp, timeAndPosUpdate;
    public bool[] hasMoved;
    public GameObject[] buildingGO;
    public Vector3 startPos;
    public SpriteRenderer rend;
    public Sprite[,] sprites = new Sprite[4, 5];
    private static MergeManager instance;
    AudioSource audio;
    public CurrencyManager cMan;

    private void Awake()
    {
        hasMoved = new bool[(x * y)];
        buildingGO = new GameObject[(x * y)];
        timeAndPosUpdate = true;
        //startPos = gameObject.transform.position;
        rend = gameObject.GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridNodes[x, y];
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time - startTime) * lerpSpeed;

        for (int i = 0; i < buildingGO.Length; i++)
        {
            switch (buildingGO[i].gameObject.transform.position
                != buildingGO[i].GetComponent<PositionTracker>().startPos)
            {
                case true:
                    hasMoved[i] = true;
                    break;
                case false:
                    hasMoved[i] = false;
                    break;
            }
        }
    }
    public static MergeManager GetInstance()
    {
        return instance != null ? instance : null;
    }

    public void OnChildCollision2D(PositionTracker pT1, PositionTracker pT2)
    {
        //Checks so see if both things colliding are actually buildings.
        switch (pT1.tag == "Building" && pT2.tag == "Building")
        {

            case true:
                //If they are, are they the same type and level?
                switch (pT1.buildingType == pT2.buildingType
                    && pT1.tier == pT2.tier)
                {
                    case true:
                        //Checks to see if the building is less than max level.
                        switch (pT1.tier < PositionTracker.TIER.T3)
                        {
                            case true:
                                //Checks to see what building is being moved. 
                                //The one that has been moved will be deactivated and sent back to its original placement.
                                //(This has been updated to see which one has moved the farthest)
                                //The stationary building will increase in level.
                                //switch (pT1.transform.position != pT1.startPos)
                                switch(Vector3.Distance(pT1.transform.position, pT1.startPos) 
                                    > Vector3.Distance(pT2.transform.position, pT2.startPos))
                                {
                                    case true:
                                        Destroy(pT1.gameObject);
                                        break;
                                    case false:
                                        //This is where the the merge takes place. 
                                        audio.Play();
                                        cMan.Add(pT1.buildingBounty);
                                        pT1.tierInc = true;
                                        break;
                                }
                                break;
                            case false:
                                cMan.Add(pT1.buildingBounty + pT2.buildingBounty);
                                audio.Play();
                                Destroy(pT2.gameObject);
                                Destroy(pT1.gameObject);
                                Debug.Log("Level Cap Reached!");
                                break;
                        }
                        break;
                    case false:
                        Debug.Log("Incompatable building type!");
                        pT1.lerpColor = PositionTracker.COLOR.RED;
                        break;
                }
                break;
        }
    }

    public void OnChildExit2D(PositionTracker pT1, PositionTracker pT2)
    {
        pT2.lerpColor = PositionTracker.COLOR.WHITE;
        pT1.lerpColor = PositionTracker.COLOR.WHITE;
    }

    void SpawnGrid(GameObject tGO)
    {


        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                grid[i, j] = Instantiate(tGO, new Vector3(i, j, 0),
                    Quaternion.identity, gameObject.transform).GetComponent<GridNodes>();
            }
        }

    }
}
   
