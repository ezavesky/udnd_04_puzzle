using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] lightsDanger;
	public GameObject[] lightsNormal;

	//objects will be activated when in each state
	public GameObject[] activatePuzzle1;
	public GameObject[] activateFinish;

	//public HudInteraction objHud;

	public float moveSpeed = 1.0f;
	public GameObject objPlayer;
	public GameObject waypointSpawn;
	public GameObject waypointPuzzle;
	public GameObject waypointFinal;

	public enum GameState { STATE_INVALID, STATE_STARTUP, STATE_PUZZLE1, STATE_COMPLETE }
	private GameState gameState = GameState.STATE_INVALID;
	//private GameState gameAutoProgress = GameState.STATE_INVALID;

	private GameObject[] activatePrevious = null;

	// Use this for initialization
	void Start () {
		//objPlayer.transform = waypointSpawn.transform;
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
		//reposition camera
		/*
		if (transformEnd && transformBegin) {
			transformProgress += (Time.deltaTime / waypointDuration);
			if (transformProgress > 1.0f) {
				transformBegin = null;
				Debug.Log ("GameController: Finished position move to new state: " + gameState);
			} else {
				Camera.main.transform.parent.transform.position = 
					Vector3.Slerp (transformBegin.position, transformEnd.position, transformProgress);
				Camera.main.transform.parent.transform.rotation = 
					Quaternion.Slerp (transformBegin.rotation, transformEnd.rotation, transformProgress);
			}
		}*/

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

		Transform transformEnd = null;
		GameObject[] activateNew = null;
		switch (gameState) {
		default:
		case GameState.STATE_INVALID:
			return;
		case GameState.STATE_STARTUP:
			transformEnd = waypointSpawn.transform;
			//gameAutoProgress = GameState.STATE_PUZZLE1;
			//objHud.ActivateHUD ("Welcome to the Puzzle, version 1.");
			break;
		case GameState.STATE_PUZZLE1:
			transformEnd = waypointPuzzle.transform;
			activateNew = activatePuzzle1;
			//objHud.DeactivateHUD ();
			break;
		case GameState.STATE_COMPLETE:
			activateNew = activateFinish;
			transformEnd = waypointFinal.transform;
			break;
		}
		Debug.Log ("GameController: Entering new state: " + gameState);

		//deactivate previous set and activate new one
		if (activatePrevious != null) {
			foreach (GameObject obj in activatePrevious) {
				if (obj) {
					obj.SetActive (false);
				}
			}
			activatePrevious = activateNew;
		}
		if (activateNew != null) {
			foreach (GameObject obj in activateNew) {
				if (obj) {
					obj.SetActive (true);
				}
			}
		}


		//allow position and rotation to move...
		if (transformEnd) {
			// Move the player to the play position.
			iTween.MoveTo(objPlayer, 
				iTween.Hash(
					"position", transformEnd.position, 
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
