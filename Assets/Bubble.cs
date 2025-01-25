using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        // Destroy the sphere
        Destroy(gameObject);
    }

}
