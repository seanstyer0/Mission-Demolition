using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static public MissionDemolition S;  //singleton

    //fields set in the Unity inspector
    public GameObject[] castles;    //an array of the castle prefabs
    public Text gtLevel;            //the UI element for level progress
    public Text gtScore;            //the UI element for score
    public Text gameOver;           //the UI element to alert the player if they have lost the level
    public Vector3 castlePos;       //the place to put castles

    public bool ___________________;

    //fields set dynamically
    public int level;           //the current level
    public int levelMax;        //the number of levels
    public int shotsLeft;
    public GameObject castle;   //the current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot"; //followCam mode

    // Start is called before the first frame update
    void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    public void StartLevel()
    {
        //clear any leftover game over text
        S.gameOver.text = "";

        //get rid of any old castle
        if (castle != null)
        {
            Destroy(castle);
        }

        //destroy any old projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //isntantite the new castle
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsLeft = 3;

        //reset the camera
        SwitchView("Both");
        ProjectileLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }

    void ShowGT()
    {
        //show the data in the UI texts
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Left: " + shotsLeft;
    }

    // Update is called once per frame
    void Update()
    {
        ShowGT();

        //check for level end
        if(mode == GameMode.playing && Goal.goalMet)
        {
            //change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Both");
            //start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    void OnGUI()
    {
        //draw the GUI button for view switching at the top of the screen
        Rect buttonRect = new Rect(Screen.width / 2 - 50, 10, 100, 24);

        switch (showing)
        {
            case "Slingshot":
                if(GUI.Button(buttonRect, "Show Castle"))
                {
                    SwitchView("Castle");
                    //make sure the player hasn't already lost
                    if(S.shotsLeft == 0)
                    {
                        GameOver();
                        Invoke("StartLevel", 5f);
                    }
                }
                break;
            case "Castle":
                if (GUI.Button(buttonRect, "Show Both"))
                {
                    SwitchView("Both");
                    if (S.shotsLeft == 0)
                    {
                        GameOver();
                        Invoke("StartLevel", 5f);
                    }
                }
                break;
            case "Both":
                if (GUI.Button(buttonRect, "Show Slingshot"))
                {
                    SwitchView("Slingshot");
                    if (S.shotsLeft == 0)
                    {
                        GameOver();
                        Invoke("StartLevel", 5f);
                    }
                }
                break;
        }
    }

    //Static method that allows code anywhere to request a change
    static public void SwitchView(string eView)
    {
        S.showing = eView;
        switch (S.showing)
        {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    //static method that allows anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsLeft--;
    }

    //print game over text on the screen
    public static void GameOver()
    {
        S.gameOver.text = "LEVEL FAILED";
    }
}
