using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Vector3Int startPos, endPos;
    


    //startPos.GetComponent<Spawner>().SetPathfindingStart();



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Algorithm();
        }
    }

    private void Algorithm()
    {
        PathfindingDebugger.MyInstance.CreateTiles(startPos, endPos);
    }
    
}
