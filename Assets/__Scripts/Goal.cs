using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //a static field accessable by code anywhere
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        //when the trigger is hit by something
        //check to see if its a projectile
        if(other.gameObject.tag == "Projectile")
        {
            //if so, set goalMet to true
            Goal.goalMet = true;
            //also set the alpha of the color to the highest opacity
            Color c =  GetComponent<Renderer>().material.color;
            c.a = 1;
            GetComponent<Renderer>().material.color = c;
            print("Goal Entered");
        }
    }
}
