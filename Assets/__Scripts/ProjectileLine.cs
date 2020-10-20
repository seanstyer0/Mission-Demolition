using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //singleton

    //fields to be set in the unity inspector
    public float minDist = 0.1f;

    public bool ______________________;

    //fields set dynamically
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;


    void Awake()
    {
        S = this;   //set the singleton
        //get a reference to the line renderer
        line = GetComponent<LineRenderer>();
        //disable the line renderer until its needed
        line.enabled = false;
        //initialize the points list
        points = new List<Vector3>();
    }

    //this is a property
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if(_poi != null)
            {
                //when _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //this can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    //called to add a point to the line
    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //if the point isnt far enough from that last point, it returns
            return;
        }
        if(points.Count == 0)
        {
            //if this is the launch point
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;
            //...it adds an extra bit of line to aid aiming later
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.SetVertexCount(2);
            //sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //enables the LineRenderer
            line.enabled = true;
        }
        else
        {
            //normal behavior of adding a point
            points.Add(pt);
            line.SetVertexCount(points.Count);
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                //if there are no points, return Vector3.zero
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    void FixedUpdate()
    {
        if(poi == null)
        {
            //if there is no poi, search for one
            if(FollowCam.S.poi != null)
            {
                if (FollowCam.S.poi.tag == "Projectile")
                {
                    poi = FollowCam.S.poi;
                }
                else
                {
                    return; //return if we didn't find a poi
                }
            }
            else
            {
                return; //return if we didn't find a poi
            }
        }
        //if there is a poi, its loc is added every FixedUpdate
        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping())
        {
            //once the poi is sleeping, it is cleared
            poi = null;
        }
    }
}
