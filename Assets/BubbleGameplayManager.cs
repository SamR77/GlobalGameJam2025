using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGameplayManager : MonoBehaviour
{
    public GameObject bubblePrefab;

    public Transform SpawnRow_00;
    public Transform SpawnRow_01;
    public Transform SpawnRow_02;
    public Transform SpawnRow_03;

    private List<GameObject>[] rows; // List of objects for each row



    void Start()
    {
        rows = new List<GameObject>[4]; // Initialize list for 4 rows
        for (int i = 0; i < 4; i++)
        {
            rows[i] = new List<GameObject>(); // Set up an empty list for each row
        }

        InvokeRepeating("SpawnBubble", 2, 2);
    }

    private void SpawnBubble()
    {
        // Randomly select 1 of 4 rows.
        // instantiate a new bubble a one of the fours SpawnRow positions.


        int spawnRow = Random.Range(0, 0);

        switch (spawnRow)
        {
            case 0:
                Instantiate(bubblePrefab, SpawnRow_00.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(bubblePrefab, SpawnRow_01.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(bubblePrefab, SpawnRow_02.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(bubblePrefab, SpawnRow_03.position, Quaternion.identity);
                break;
        }


    }



    void OnEnable()
    {
        // Subscribe to the Action events
        Actions.W_KeyEvent += () => DeleteLeftmostObject(0); // Row 1 (W key)
        Actions.A_KeyEvent += () => DeleteLeftmostObject(1); // Row 2 (A key)
        Actions.S_KeyEvent += () => DeleteLeftmostObject(2); // Row 3 (S key)
        Actions.D_KeyEvent += () => DeleteLeftmostObject(3); // Row 4 (D key)
    }




    // Deleting the leftmost object in a given row
    private void DeleteLeftmostObject(int rowIndex)
    {
        if (rows[rowIndex].Count > 0)
        {
            GameObject leftmostObject = rows[rowIndex][0];
            rows[rowIndex].RemoveAt(0); // Remove the object from the list
            Destroy(leftmostObject); // Destroy the object
        }
    }




    void OnDisable()
    {
        // Unsubscribe from the Action events to prevent memory leaks
        Actions.W_KeyEvent -= () => DeleteLeftmostObject(0);
        Actions.A_KeyEvent -= () => DeleteLeftmostObject(1);
        Actions.S_KeyEvent -= () => DeleteLeftmostObject(2);
        Actions.D_KeyEvent -= () => DeleteLeftmostObject(3);
    }
}
