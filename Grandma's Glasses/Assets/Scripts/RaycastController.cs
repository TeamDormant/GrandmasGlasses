using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;
    public const float skinWidth = 0.015f;
    public int xRayCount = 4; // number of rays fired Horizontally
    public int yRayCount = 4; // number of rays fired Vertically
    [HideInInspector]
    public float xRaySpacing; // space between rays fired Horizontally
    [HideInInspector]
    public float yRaySpacing; // space between rays fired Vertically
    [HideInInspector]
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }    

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // Origin is BOTTOM Left, not TOP Left. Shit ain't HTML, be careful with this, guys.
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        xRayCount = Mathf.Clamp(xRayCount, 2, int.MaxValue);
        yRayCount = Mathf.Clamp(yRayCount, 2, int.MaxValue);
        xRaySpacing = bounds.size.y / (xRayCount - 1);
        yRaySpacing = bounds.size.x / (yRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
