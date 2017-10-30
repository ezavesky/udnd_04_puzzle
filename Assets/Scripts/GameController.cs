using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] lightsDanger;
	public GameObject[] lightsNormal;
	public GameObject doorEntry;
	public GameObject doorLock;

	public HudInteraction objHud;

	public float waypointDuration = 2.0f;
	public GameObject waypointSpawn;
	public GameObject waypointPuzzle;
	public GameObject waypointFinal;

	public enum GameState { STATE_INVALID, STATE_STARTUP, STATE_PUZZLE1, STATE_COMPLETE }
	private GameState gameState = GameState.STATE_INVALID;
	private GameState gameAutoProgress = GameState.STATE_INVALID;

	private Transform transformBegin = null;	//transform to move FROM
	private Transform transformEnd = null;	//transform to move TO
	private float transformProgress = 0;	//transform transition rate

	// Use this for initialization
	void Start () {
		SetGameState (GameState.STATE_STARTUP);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameAutoProgress!=GameState.STATE_INVALID) {	//had an 'auto' progress state?
			if (Input.GetMouseButtonDown (0)) {		//wait for click anywhere
				GameState gameStateTemp = gameAutoProgress;
				gameAutoProgress = GameState.STATE_INVALID;
				SetGameState (gameStateTemp);
				return;
			}
		}

		//reposition camera
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
		}

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

		transformEnd = null;
		switch (gameState) {
		default:
		case GameState.STATE_INVALID:
			return;
		case GameState.STATE_STARTUP:
			transformEnd = waypointSpawn.transform;
			gameAutoProgress = GameState.STATE_PUZZLE1;
			objHud.ActivateHUD ("Welcome to the Puzzle, version 1.");
			break;
		case GameState.STATE_PUZZLE1:
			transformEnd = waypointPuzzle.transform;
			objHud.DeactivateHUD ();
			break;
		case GameState.STATE_COMPLETE:
			transformEnd = waypointFinal.transform;
			break;
		}
		Debug.Log ("GameController: Entering new state: " + gameState);

		//other logic to change game state

		//allow position and rotation to move...
		if (transformEnd) {
			transformBegin = Camera.main.transform.parent.transform;
			transformProgress = 0.0f;
		}
	}

	public GameState GetGameState()
	{
		return gameState;
	}



}
