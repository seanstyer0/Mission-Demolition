using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    //fields set in the inspector pane
    static public FollowCam S;  //a FollowCam singleton
    public float easing = 0.05f;
    public Vector2 minXY;

    public bool _____________________;
    //fields set dynamically
    public GameObject poi;  //the point of interest
    public float camZ;  //the desired z position of the camera


    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 destination;
        if (poi == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //get the position of the poi
            destination = poi.transform.position;
            //if poi is a projectile, check to see if its at rest
            if(poi.tag == "Projectile")
            {
                //if it is sleeping (that is, not moving)
                if (poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    //return to default view
                    poi = null;
                    //in the next update
                    return;
                }
            }
        }
        //limit the x and y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpolate from the current camera position towards destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //retain a destination.z of camZ
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
        //set the orthographic view of the camera to keep ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
    }
}
