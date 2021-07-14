using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public Vector3 startPos;
    public SpriteRenderer rend;
    public Sprite[] sprite;
    public bool bufferPos,tierInc;
    public float bufferTime, startTime, lerpSpeed;
    public int tierTracker;
    public long buildingBounty;
    public enum TIER { T0 = 0, T1 = 1, T2 = 2, T3  = 3};
    public TIER tier;
    public enum TYPE { Cafe = 0, Office = 1, BookStore = 2, ClothingStore = 3};
    public TYPE buildingType;
    public enum COLOR { WHITE = 0, RED = 1, BLUE = 2, GOLD = 3 };
    public COLOR lerpColor;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
        bufferPos = true;
        tierInc = false;
        switch (tier)
        {
            case TIER.T0:
                tierTracker = 0;
                rend.sprite = sprite[0];
                break;
            case TIER.T1:
                tierTracker = 1;
                buildingBounty *= 2;
                rend.sprite = sprite[1];
                break;
            case TIER.T2:
                tierTracker = 2;
                buildingBounty = (buildingBounty * 2) * 2;
                rend.sprite = sprite[2];
                break;
            case TIER.T3:
                tierTracker = 3;
                buildingBounty = ((buildingBounty * 2) * 2)*2;
                rend.sprite = sprite[3];
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time - startTime) * lerpSpeed;
        switch (bufferPos && bufferTime < Time.time)
        {
            case true:
                startPos = transform.position;
                bufferPos = false;
                break;
        }
        switch (tierInc)
        {
            case true:
                if (tier < TIER.T3)
                {
                    tier++;
                    buildingBounty *= 2;
                    transform.position = transform.parent.position + Control.GetOffSet(transform.gameObject);
                }
                else if(tier >= TIER.T3)
                {
                    tier = TIER.T3;
                }
                switch (tier)
                {
                    case TIER.T0:
                        tierTracker = 0;
                        rend.sprite = sprite[0];
                        break;
                    case TIER.T1:
                        tierTracker = 1;
                        rend.sprite = sprite[1];
                        break;
                    case TIER.T2:
                        tierTracker = 2;
                        rend.sprite = sprite[2];
                        break;
                    case TIER.T3:
                        tierTracker = 3;
                        rend.sprite = sprite[3];
                        break;
                }
                        tierInc = false;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PositionTracker other = collision.GetComponent<PositionTracker>();
        MergeManager manager = MergeManager.GetInstance();
        if (manager != null && other != null) manager.OnChildCollision2D(this,other);       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PositionTracker other = collision.GetComponent<PositionTracker>();
        MergeManager manager = MergeManager.GetInstance();
        if (manager != null && other != null) manager.OnChildExit2D(this, other);
    }
}
