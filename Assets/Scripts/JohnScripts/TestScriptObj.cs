using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings")]
public class TestScriptObj : ScriptableObject
{
    public enum MERGE { T0, T1, T2, T3, T4 };
    public MERGE merge;
    public enum TYPE { Cafe, Office, BookStore, ClothingStore }
    public TYPE buildingType;
    public enum COLOR { WHITE, RED, BLUE, GOLD };
    public COLOR lerpColor;
    public Sprite nullSpr, currentSprite;
    public Sprite[] spriteSet;
    public SpriteRenderer rend;
    public Vector3 startPos;
    public int currentSpriteInt;
    public float revenuePerSec, startTime, lerpSpeed;
    public bool hasMoved, levelUp, timeAndPosUpdate;

    GameObject myBuilding = new GameObject("BuildingSO");
    // Start is called before the first frame update
    void Start()
    {
        timeAndPosUpdate = true;
        myBuilding.AddComponent<BoxCollider2D>();
        //myBuilding.transform.position;
    }
    private void Awake()
    {
        currentSpriteInt = 0;
        this.currentSprite = spriteSet[currentSpriteInt];
        
    }
    // Update is called once per frame
    void Update()
    {
        this.currentSprite = spriteSet[currentSpriteInt];

        float t = (Time.time - startTime) * lerpSpeed;
        if (timeAndPosUpdate)
        {
            startPos = myBuilding.transform.position;
            timeAndPosUpdate = false;
        }
        //Checks to see if the cube has moved.
        switch (myBuilding.transform.position != startPos)
        {
            case true:
                hasMoved = true;
                break;
        }
        switch (lerpColor)
        {
            case COLOR.GOLD:
                rend.material.SetColor
                    ("_Color", Color.Lerp(Color.white, Color.yellow, t));
                break;
            case COLOR.RED:
                rend.material.SetColor
                    ("_Color", Color.Lerp(Color.white, Color.red, t));
                break;
            case COLOR.WHITE:
                rend.material.SetColor
                    ("_Color", Color.Lerp(Color.white, Color.white, t));
                break;
        }
    }
    public void LevelUp()
    {
        currentSpriteInt++;
        //currentSprite[];
    }
    
}
