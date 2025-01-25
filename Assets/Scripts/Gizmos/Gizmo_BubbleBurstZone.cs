using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_BubbleBurstZone : MonoBehaviour
{
    // draw a Cube gizmo that matches the dimension of this objects BoxCollider
    // Color should be green and 50% transparent
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }







}
