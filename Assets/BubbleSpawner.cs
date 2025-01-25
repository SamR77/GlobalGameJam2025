using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;

    public Transform SpawnRow_01;
    public Transform SpawnRow_02;
    public Transform SpawnRow_03;
    public Transform SpawnRow_04;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn a bubble every 2 seconds.
        InvokeRepeating("SpawnBubble", 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBubble()
    {
        // Randomly select 1 of 4 rows.
        // instantiate a new bubble a one of the fours SpawnRow positions.


        Debug.Log("Spawning Bubble");
        int spawnRow = Random.Range(1, 5);

        switch (spawnRow)
        {
            case 1:
                Instantiate(bubblePrefab, SpawnRow_01.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(bubblePrefab, SpawnRow_02.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(bubblePrefab, SpawnRow_03.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(bubblePrefab, SpawnRow_04.position, Quaternion.identity);
                break;
        }


    }



}
