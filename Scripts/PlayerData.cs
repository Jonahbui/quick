/*  Author: Jonah Bui
	Purpose: To encapsulate data about the player so that it may be serialized and saved.
	Date: January 14, 2020
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerData
{
    public int score;
    public PlayerData(Player player)
    {
        score = player.PlayerHighscore;
    }
}
