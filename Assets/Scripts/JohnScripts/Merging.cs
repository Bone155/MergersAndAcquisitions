using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merging : MonoBehaviour
{
    //Enum "MERGE" will track the levels of each property, only properties of the same level can be combined.
    public enum TIER { T0, T1, T2, T3, T4 };
    public TIER tier;
    //Enum "TYPE" will track what each building is.
    public enum TYPE { Cafe, Office, BookStore, ClothingStore };
    public TYPE buildingType;
    public enum COLOR { WHITE, RED, BLUE, GOLD };
    public COLOR lerpColor;
    //public TestScriptObj tSO;
    public float revenuePerSec,startTime,lerpSpeed;
    public bool hasMoved, levelUp, timeAndPosUpdate;
    public Vector3 startPos;

    public SpriteRenderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        timeAndPosUpdate = true;
        hasMoved = false;
        startPos = gameObject.transform.position;
        lerpColor = COLOR.WHITE;
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeAndPosUpdate)
        {
            startPos = gameObject.transform.position;
            timeAndPosUpdate = false;
        }
        float t = (Time.time - startTime) * lerpSpeed;
        if(levelUp)
        {
            tier++;
            levelUp = false;
        }
        //Checks to see if the cube has moved.
        switch (transform.position != startPos)
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag == "Building")
        {
            case true:
                switch (other.GetComponent<Merging>().buildingType == buildingType
                    && other.GetComponent<Merging>().tier == tier)
                {
                    case true:
                        //Checks to see what building is being moved. 
                        //The one that has been moved will be deactivated and sent back to its original placement.
                        //The stationary building will increase in level.
                        switch (hasMoved)
                        {
                            case true:
                                this.gameObject.SetActive(false);
                                this.gameObject.transform.position = startPos;
                                break;
                            case false:
                                lerpColor = COLOR.GOLD;
                                levelUp = true;
                                break;
                        }
                        break;
                    case false:
                        Debug.Log("Incompatable building type!");
                        lerpColor = COLOR.RED;
                        break;
                }
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        lerpColor = COLOR.WHITE;
        //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }
    #region Old 3D Code
    //private void OnTriggerEnter(Collider other)
    //{
    //    //Checks to see if the two colliding buildings match tier and type.
    //    switch (other.GetComponent<Merging>().buildingType == buildingType
    //        && other.GetComponent<Merging>().tier == tier)
    //    {
    //        case true:
    //            //Checks to see what building is being moved. 
    //            //The one that has been moved will be deactivated and sent back to its original placement.
    //            //The stationary building will increase in level.
    //            switch (hasMoved)
    //            {
    //                case true:
    //                    this.gameObject.SetActive(false);
    //                    this.gameObject.transform.position = startPos;
    //                    break;
    //                case false:
    //                    //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(Color.white, Color.yellow, .65f));
    //                    lerpColor = COLOR.GOLD;
    //                    levelUp = true;
    //                    break;
    //            }
    //            break;
    //        case false:
    //            Debug.Log("Incompatable building type!");
    //            //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    //            lerpColor = COLOR.RED;
    //            break;
    //    }
    //
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    lerpColor = COLOR.WHITE;
    //    //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    //}
    #endregion
}


