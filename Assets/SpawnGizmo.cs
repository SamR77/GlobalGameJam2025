using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGizmo : MonoBehaviour
{
    // Create a Gizmo that will show the spawn points in the editor.

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // draw a wide wire cube to represent the row
        Gizmos.DrawWireCube(transform.position + new Vector3(-10,0,0), new Vector3(20, 1, 0));


    }

}
