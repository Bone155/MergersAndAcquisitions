using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject gNode;
    public Vector3[] startingLocation;
    public GridNodes[,] grid;
    public int x, y;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridNodes[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                grid[i, j] = Instantiate(gNode, new Vector3(i, j, 0),
                    Quaternion.identity, gameObject.transform).GetComponent<GridNodes>();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
