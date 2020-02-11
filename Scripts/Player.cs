/*  Author: Jonah Bui
	Purpose: To store info about the player so that it may persist.
	Date: January 14, 2020
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerHighscore = 0;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

	//Getters & setters
    public int PlayerHighscore
    {
        get{ return this.playerHighscore; }
        set{ this.playerHighscore = value; }
    }
}
