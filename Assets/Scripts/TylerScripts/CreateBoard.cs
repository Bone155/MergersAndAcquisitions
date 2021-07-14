using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateBoard : MonoBehaviour
{
    [Header("Objects Settings")]
    public GameObject objectParent;
    public GameObject platformPrefab;
    [Header("Camera")]
    public Camera camera;
    public float planeSize = 15;
    [Header("Board")]
    public int rows = 3;
    public int columns = 3;
    [Header("Offset Screen Numbers")]
    public int offsetScreenX;
    public int offsetScreenY;
    private int lastScreenHeight = 0;
    private int lastScreenWidth = 0;
    Vector2 res;
    private void Start()
    {
        if(objectParent != null && platformPrefab != null)
        {
            for(int i = 0; i < columns * rows; i++)
            {
                Instantiate(platformPrefab,objectParent.transform);
            }
        }
    }
    private void Update()
    {
        if (res.x != Screen.width || res.y != Screen.height)
        {
            res = new Vector2(Screen.width, Screen.height);
            CreateLayout();
        }
    }
    // Centers the object to the screen
    private void CreateLayout()
    {
        if (camera == null || objectParent == null || columns* rows > objectParent.transform.childCount) return;
        float ySize = (Screen.height/2f)/columns;
        float xSize = (Screen.width)/rows;
        Vector3 minValue = camera.ScreenToWorldPoint(new Vector3(0, 0, planeSize));
        Vector3 maxValue = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, planeSize));
        Transform obj;
        SpriteRenderer sprite;
        Vector3 orgPos = new Vector3(((Screen.width/2f)/2f)/2f, (Screen.height/2f)/2f, 0);
        Vector3 currentScreenPos = orgPos;
        int counter = 0;
        // Looping to set the position for the objects
        for(int i = 0; i < columns; i++)
        {
            for (int k = 0; k < rows; k++)
            {
                obj = objectParent.transform.GetChild(counter);
                if (obj.TryGetComponent<SpriteRenderer>(out sprite))
                {
                    Vector2 objectSize = new Vector2(sprite.sprite.bounds.size.x/2f*obj.transform.localScale.x, sprite.sprite.bounds.size.y/2f*obj.transform.localScale.y);
                    Vector3 cameraPos = camera.ScreenToWorldPoint(currentScreenPos);
                    Vector3 newCPos = new Vector3(Mathf.Clamp(cameraPos.x, minValue.x + objectSize.x, maxValue.x - objectSize.x),
                        Mathf.Clamp(cameraPos.y, minValue.y + objectSize.y, maxValue.y - objectSize.y), 0);
                    cameraPos = newCPos;
                    obj.position = cameraPos;
                    currentScreenPos.x += xSize;
                    counter++;
                } else continue;
            }
            currentScreenPos.y += ySize;
            currentScreenPos.x = orgPos.x;
        }
    }
}
