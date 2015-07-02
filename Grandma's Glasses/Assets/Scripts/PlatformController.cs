using UnityEngine;
using System.Collections;

public class PlatformController : RaycastController
{

    public Vector3 move;
	// Use this for initialization
	public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = move * Time.deltaTime;
        transform.Translate(velocity);
	}

    void MovePassengers(Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Vertically moving platform
        if(velocity.y != 0)
        {

        }
    }
}
