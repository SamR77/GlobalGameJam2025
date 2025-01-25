using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo_BubbleClearer : MonoBehaviour
{
    // Draw a Cube gizmo that matches the dimension and position of this object's BoxCollider
    // Color should be red and 50% transparent
    private void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // Gizmo color: red and 50% transparent
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        // Adjust the position to match the collider's center
        Vector3 colliderCenter = transform.position + boxCollider.center;

        // Draw the gizmo cube with the position and size of the BoxCollider
        Gizmos.DrawCube(colliderCenter, boxCollider.size);
    }
}
