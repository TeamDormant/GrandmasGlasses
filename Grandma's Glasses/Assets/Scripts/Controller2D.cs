using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;
    const float skinWidth = 0.015f;
    public int xRayCount = 4; // number of rays fired Horizontally
    public int yRayCount = 4; // number of rays fired Vertically
    float xRaySpacing; // space between rays fired Horizontally
    float yRaySpacing; // space between rays fired Vertically
    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;

    public CollisionInfo collisions;
	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
	}

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // Origin is BOTTOM Left, not TOP Left. Shit ain't HTML, be careful with this, guys.
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        xRayCount = Mathf.Clamp(xRayCount, 2, int.MaxValue);
        yRayCount = Mathf.Clamp(yRayCount, 2, int.MaxValue);
        xRaySpacing = bounds.size.y / (xRayCount - 1);
        yRaySpacing = bounds.size.x / (yRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    void HorizontalCollisions(ref Vector3 velocity) // pass velocity by reference so it always updates properly
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (int i = 0; i < xRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (yRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity) // pass velocity by reference so it always updates properly
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < yRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (xRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }


    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
	// Update is called once per frame
	void Update (){
        
	}
}
