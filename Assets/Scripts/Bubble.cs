using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bubbleSpeed = 1f;
    [SerializeField] public bool isInBurstZone = false;

    public int LaneIndex { get; private set; } // Store the lane index

    public void InitializeBubble(float speed, int laneIndex)
    {
        bubbleSpeed = speed;
        LaneIndex = laneIndex;
    }

    void Update()
    {
        this.transform.Translate(Vector3.left * bubbleSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BurstZone"))
        {
            isInBurstZone = true;
        }
        if (other.CompareTag("EndOfLaneClearer"))
        {
            // bubble has reached the end of the lane 
            BubbleManager.Instance.HandleBubble(other.transform.position, LaneIndex, false);
            // Consider playing a different sound effect for a missed bubble

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BurstZone"))
        {
            isInBurstZone = false;
        }
    }


}