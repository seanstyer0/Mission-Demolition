using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //fields set in the unity inspection pane
    public GameObject prefabProjectile;
    public float velocityMult = 4.0f;
    public bool _______________________;

    //field set dynamically
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    // Awake is called before the first frame update
    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void Update()
    {
        //if slingshot is not in aiming mode, don't run this code
        if (!aimingMode) return;
        //get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        //convert the mouse position to 3D world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //limit mouseDelta to the radius of the Slingshot sphere collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //the mouse has been released
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta*velocityMult;
            projectile = null;
        }
    }

    void OnMouseEnter()
    {
        print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //the player has pressed the mouse button while over the slingshot
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(prefabProjectile);
        //start it at the launchPoint
        projectile.transform.position = launchPos;
        //set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
