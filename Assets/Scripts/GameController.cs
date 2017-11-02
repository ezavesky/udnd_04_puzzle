using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] lightsDanger;
	public GameObject[] lightsNormal;

	//public HudInteraction objHud;

	public float moveSpeed = 1.0f;
	public GameObject objPlayer;
	public GameObject waypointSpawn;
	public GameObject waypointPuzzle1;
	public GameObject waypointFinal;

	public enum GameState { STATE_INVALID, STATE_STARTUP, STATE_PUZZLE1, STATE_COMPLETE }
	private GameState gameState = GameState.STATE_INVALID;
	//private GameState gameAutoProgress = GameState.STATE_INVALID;

	private GameObject[] activatePrevious = null;

	// Use this for initialization
	void Start () {
		//disable all the waypoints because they contain other objects
		waypointSpawn.SetActive(false);
		waypointPuzzle1.SetActive (false);
		waypointFinal.SetActive (false);

		//force start at the beginning
		objPlayer.transform.position = waypointSpawn.transform.position;
		SetGameState (GameState.STATE_STARTUP);
		/*iTween.CameraFadeFrom(iTween.Hash
			"amount", 0, "time", 2.0f
		);*/
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (gameAutoProgress!=GameState.STATE_INVALID) {	//had an 'auto' progress state?
			if (Input.GetMouseButtonDown (0)) {		//wait for click anywhere
				GameState gameStateTemp = gameAutoProgress;
				gameAutoProgress = GameState.STATE_INVALID;
				SetGameState (gameStateTemp);
				return;
			}
		}
		*/


	}

	public void SetGameState(string value)
	{
		switch (value) {
		case "reset":
			SetGameState (GameState.STATE_STARTUP);
			break;
		case "puzzle1":
			SetGameState (GameState.STATE_PUZZLE1);
			break;
		case "finish":
			SetGameState (GameState.STATE_COMPLETE);
			break;
		default:
			break;
		}
	}

	public void SetGameState(GameState value)
	{
		if (gameState == value)
			return;
		gameState = value;

		GameObject objActive = null;
		switch (gameState) {
		default:
		case GameState.STATE_INVALID:
			return;
		case GameState.STATE_STARTUP:
			objActive = waypointSpawn;
			waypointFinal.SetActive (false);
			waypointPuzzle1.SetActive (false);
			//gameAutoProgress = GameState.STATE_PUZZLE1;
			//objHud.ActivateHUD ("Welcome to the Puzzle, version 1.");
			break;
		case GameState.STATE_PUZZLE1:
			objActive = waypointPuzzle1;
			waypointFinal.SetActive (false);
			waypointSpawn.SetActive (false);
			//objHud.DeactivateHUD ();
			break;
		case GameState.STATE_COMPLETE:
			objActive = waypointFinal;
			waypointPuzzle1.SetActive (false);
			waypointSpawn.SetActive (false);
			break;
		}
		Debug.Log ("GameController: Entering new state: " + gameState);

		//allow position and rotation to move...
		if (objActive) {
			objActive.SetActive (true);
			// Move the player to the play position.
			iTween.MoveTo(objPlayer, 
				iTween.Hash(
					"position", objActive.transform.position, 
					"time", moveSpeed, 
					"easetype", "linear"
				)
			);
		}
	}

	public GameState GetGameState()
	{
		return gameState;
	}



}
