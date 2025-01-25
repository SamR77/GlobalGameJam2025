using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform triggerCenter; // Reference to trigger's center point


    [SerializeField] private float PerfectMinPercent = 10f; 
    [SerializeField] private float GoodMinPercent = 50f; 
    [SerializeField] private float EarlyLateMinPercent = 100f;

    public float triggerSize = 2.0f; // get this size from the collider object

    public bool isInsideTrigger = false;
    private float distanceFromCenter;
    private bool isOnRightSide;    

    // Update is called once per frame
    void Update()
    {
        // Move the bubble to the left
        this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

                

        // Calculate distance and side if inside trigger
        if (isInsideTrigger && triggerCenter != null)
        {
            distanceFromCenter = Mathf.Abs(transform.position.x - triggerCenter.position.x);
            isOnRightSide = transform.position.x > triggerCenter.position.x;
        }


    }

    void OnMouseDown()
    {
        // Check If the bubble is not inside the trigger when clicked
        if (isInsideTrigger == false)
        {
            Debug.Log("Baby did not like that >:( !!!!");

            // bubble was burst outside of trigger... baby is upset.        
        }

        else if (isInsideTrigger == true)
        {
            Debug.Log("Baby Liked that :D !!!");
            
            /*
            float percentageFromCenter = (distanceFromCenter / (triggerSize / 2f)) * 100f;

            if (percentageFromCenter <= PerfectMinPercent)
            {
                Debug.Log("Perfect!");
            }

            else if (percentageFromCenter <= GoodMinPercent && percentageFromCenter < PerfectMinPercent)
            {
                Debug.Log("Good!");
            }

            else if (percentageFromCenter <= EarlyLateMinPercent && percentageFromCenter < GoodMinPercent)
            {
                if (isOnRightSide == true)
                { 
                    Debug.Log("Early!"); 
                }
                
                else 
                {
                    Debug.Log("Late!");
                }
            }
            */
        }       

        Destroy(gameObject); // might want to break this out into its own Method or consider calling it differntly in the results above, we might want different visual effects
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BurstZone")
        {
            isInsideTrigger = true;
            triggerCenter = other.transform;            
        }        
    }

    void OnTriggerExit(Collider other)
    {
        isInsideTrigger = false;
        triggerCenter = null;
    }

}
