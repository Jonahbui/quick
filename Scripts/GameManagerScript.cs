/*  Author: Jonah Bui
	Purpose: To control the flow of the game from start to game over.
	Loads in previous highscores, updates highscores, enable/disable UI, spawn/despawn buttons, start/restart game, and controls many more small details.
	Date: January 14, 2020
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	//Use to update text info in GUI
	public Text timeText, startText, scoreText, highscoreText, readyUpText, bonusText;

	//Button to spawn
	public GameObject buttonPrefab;

	//Use manage restart and start of game
	public Button startButton;
	public GameObject restartButton;

	//Use to play audio
	public AudioSource myAudio;
	public AudioClip myClip;

	//Use to store currently spawned button for the player to tap on
	private GameObject currentButton;

	//Use to keep track of game status
	private bool gameStart = false;
	private bool gameOver = false;

	//Store score info
	private int score = 0;
	private float timeLeft = 10f;

	//Get animations for text
	public Animator readyUpAnim, bonusAnim;

	//Button color values
	private Color buttonColor;
	private int hue = 180, saturation = 24, value = 95;

	void Start () 
	{
		//Load player data
		PlayerData loadData = SaveManager.LoadData();
		if (loadData != null)
		{
			Player.Instance.PlayerHighscore = loadData.score;
		}

		buttonPrefab.GetComponent<SpriteRenderer>().color = HSVtoRGB(hue, saturation, value);
		buttonColor = buttonPrefab.GetComponent<SpriteRenderer>().color;

		//Set initial menu settings
		startText.enabled = true;
		timeText.enabled = false;
		scoreText.enabled = false;
		restartButton.SetActive(false);
		readyUpText.enabled = false;

		//Set highscore
		highscoreText.text = "Highscore:"+Player.Instance.PlayerHighscore;

		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameStart)
		{
			if (!gameOver)
			{
				//Update the time leftk
				timeLeft -= Time.deltaTime;
				timeText.text = "" + string.Format("{0:F1}",timeLeft);
				if (timeLeft <= 0f)
				{
					gameOver = true;
				}

				//Allow user to tap and score
				Touch[] fingers = Input.touches;
				if (Input.touchCount == 1)
				{
					if (fingers[0].phase == TouchPhase.Ended)
					{
						//Check what the user tapped on
						Collider2D cast = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.touches[0].position), .3f);
						if (cast != null)
						{
							//If button hit, update the score and spawn a new button
							if (cast.tag == "Button")
							{
								score++;
								scoreText.text = "Score: " + score;
								myAudio.PlayOneShot(myClip);
								Destroy(currentButton);
								currentButton = Instantiate(buttonPrefab, new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-4.0f, 2.5f)), Quaternion.identity);

								//Give a bonus to the player for every 10 points and change the color of the button
								if (score % 10 == 0 && score != 0)
								{
									timeLeft += 5f;
									bonusAnim.Play("BonusFade");

									buttonPrefab.GetComponent<SpriteRenderer>().color = HSVtoRGB(hue += 60, saturation, value);
									Time.timeScale += .1f;
								}
							}
						}
					}
				}
			}
			else
			{
				timeText.text = "0";
				restartButton.SetActive(true);

				//Set new highscore if there is one
				if (Player.Instance != null)
				{
					if (Player.Instance.PlayerHighscore < score)
					{
						Player.Instance.PlayerHighscore = score;
					}
				}
				//Save info
				SaveManager.SaveData(Player.Instance);
			}
		}
	}
	
	//Function: resets the UI elements, score, time, and time scale
	public void RestartGame()
	{
		if (currentButton != null)
		{
			//If there is a button still, get rid of it
			Destroy(currentButton);
		}

		//Reset scoreline and timer
		score = 0;
		timeLeft = 10f;
		Time.timeScale = 1f;

		//Reset the status to allow user to restart
		gameOver = false;
		gameStart = false;

		//Reset GUI elements
		startButton.enabled = true;
		startText.enabled = true;
		timeText.enabled = false;
		scoreText.text = "Score: 0";
		scoreText.enabled = false;
		restartButton.SetActive(false);
		highscoreText.enabled = true;

		buttonPrefab.GetComponent<SpriteRenderer>().color = buttonColor;

		//Set highscore
		highscoreText.text = "Highscore: " + Player.Instance.PlayerHighscore;
	}
	
	//Function: used for button listener to start game
	public void OnTapStart()
	{
		StartCoroutine(WaitStart());
	}

	//Function: loads main gameplay UI and spawns buttons for the player
	IEnumerator WaitStart()
	{
		//Hide Menu UI elemetns
		startText.enabled = false;
		startButton.enabled = false;
		highscoreText.enabled = false;
		readyUpText.enabled = true;

		//Enable Game UI elements
		timeText.enabled = true;
		scoreText.enabled = true;

		//Give player warning
		readyUpAnim.Play("ReadyUpIdle");
		readyUpText.text = "Ready?";
		yield return new WaitForSeconds(1f);
		readyUpText.text = "Go!";
		readyUpAnim.Play("ReadyUpFade");

		//Start game
		gameStart = true;
		currentButton = Instantiate(buttonPrefab, new Vector2(Random.Range(-2.0f,2.0f), Random.Range(-4.0f,2.5f)), Quaternion.identity);
	}

	//Function: converts from HSV to RGB (*Note: values must be whole integers | Hue: 0-360 | Sat: 0-100 | Val: 0-100)
	private Color HSVtoRGB(int hue, int saturation, int value)
	{
		hue = hue % 360;
		float c = saturation / 100.0f * value / 100.0f;
		float x = c * (1 - Mathf.Abs((hue / 60) % 2 - 1));
		float m = value / 100.0f - c;

		if (hue < 60)
			return new Color((c + m), (x + m), (0 + m));
		else if (hue < 120)
			return new Color((c + m), (c + m), (0 + m));
		else if (hue < 180)
			return new Color((0 + m), (c + m), (x + m));
		else if (hue < 240)
			return new Color((0 + m), (x + m), (c + m));
		else if (hue < 300)
			return new Color((x + m), (0 + m), (c + m));
		else if (hue < 360)
			return new Color((c + m), (0 + m), (x + m));
		else
			return buttonColor;
	}
}
