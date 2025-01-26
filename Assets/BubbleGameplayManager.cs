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

    void Update()
    {
        
        Debug.Log("Row 0 has " + rows[0].Count + " objects.");
        Debug.Log("Row 1 has " + rows[1].Count + " objects.");
        Debug.Log("Row 2 has " + rows[2].Count + " objects.");
        Debug.Log("Row 3 has " + rows[3].Count + " objects.");
        

    }


    private void SpawnBubble()
    {
        // Randomly select 1 of 4 rows.
        // instantiate a new bubble a one of the fours SpawnRow positions.

        int spawnRow = Random.Range(0, 4);

        switch (spawnRow)
        {
            case 0:
                GameObject newObject00 = Instantiate(bubblePrefab, SpawnRow_00.position, Quaternion.identity);
                rows[spawnRow].Add(newObject00);
                break;
            case 1:
                GameObject newObject01 = Instantiate(bubblePrefab, SpawnRow_01.position, Quaternion.identity);
                rows[spawnRow].Add(newObject01);
                break;
            case 2:
                GameObject newObject02 = Instantiate(bubblePrefab, SpawnRow_02.position, Quaternion.identity);
                rows[spawnRow].Add(newObject02);
                break;
            case 3:
                GameObject newObject03 = Instantiate(bubblePrefab, SpawnRow_03.position, Quaternion.identity);
                rows[spawnRow].Add(newObject03);
                break;
        }
    }



    // Deleting the leftmost object in a given row
    private void DeleteLeftmostObject(int rowIndex)
    {
        if (rows[rowIndex].Count > 0)
        {
            Debug.Log("deleting object on row W");

            GameObject leftmostObject = rows[rowIndex][0];
            rows[rowIndex].RemoveAt(0);     // Remove the object from the list
            Destroy(leftmostObject);        // Destroy the object
        }
    }



    void OnEnable()
    {
        // Subscribe to the Action events
        Actions.W_KeyEvent += () => DeleteLeftmostObject(0); // Row 0 (W key)
        Actions.A_KeyEvent += () => DeleteLeftmostObject(1); // Row 1 (A key)
        Actions.S_KeyEvent += () => DeleteLeftmostObject(2); // Row 2 (S key)
        Actions.D_KeyEvent += () => DeleteLeftmostObject(3); // Row 3 (D key)
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
