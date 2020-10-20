using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    //fields set in the unity inspector
    public int numClouds = 40;          //the # of clouds to make
    public GameObject[] cloudPrefabs;   //the prefabs for the clouds
    public Vector3 cloudsPosMin;        //Min position of each cloud
    public Vector3 cloudsPosMax;        //Max position of each cloud
    public float cloudScaleMin = 1;     //Min scale of each cloud
    public float cloudScaleMax = 5;     //Max scale of each cloud
    public float cloudSpeedMult = 0.15f;  //adjust the speed of clouds

    public bool ________________________;

    //fields set dynamically
    public GameObject[] cloudInstances;


    void Awake()
    {
        //make an array large enough to store all of the cloud instances
        cloudInstances = new GameObject[numClouds];
        //find the CloudAnchor parent GameObject
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Iterate through and make Cloud_s
        GameObject cloud;
        for(int i = 0; i < numClouds; i++)
        {
            //pick an int between 1 and cloudPrefabs.length-1
            //Random.Range will never pick the top number
            int prefabNum = UnityEngine.Random.Range(0, cloudPrefabs.Length-1);
            //Make an instance
            cloud = Instantiate(cloudPrefabs[prefabNum] as GameObject);
            //Position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = UnityEngine.Random.Range(cloudsPosMin.x, cloudsPosMax.x);
            cPos.y = UnityEngine.Random.Range(cloudsPosMin.y, cloudsPosMax.y);
            //Scale cloud
            float scaleU = UnityEngine.Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller clouds (with smaller scaleU) should be lower to the ground
            cPos.z = 100 - 90 * scaleU;
            //Apply these transforms to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Make cloud a child of the anchor
            cloud.transform.parent = anchor.transform;
            //add the cloud to cloudInstances
            cloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //iterate over each created cloud object
        foreach(GameObject cloud in cloudInstances)
        {
            //get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //move closer clouds faster
            cPos.x -= (1 - (cPos.z * 0.015f)) * Time.deltaTime * cloudSpeedMult;
            //if a cloud has moved too far to the left
            if (cPos.x <= cloudsPosMin.x)
            {
                //Move it to the far right
                cPos.x = cloudsPosMax.x;
            }
            //Apply the new postition to the cloud
            cloud.transform.position = cPos;
        }
    }
}
