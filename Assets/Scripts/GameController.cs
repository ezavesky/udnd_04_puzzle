using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject[] lightsDanger;
	public GameObject[] lightsNormal;

	public GameObject objDoor;
	public GameObject[] positionDoor = new GameObject[2];
	public GameObject objShip;
	public GameObject[] positionShip;

	//public HudInteraction objHud;

	public float moveSpeed = 1.0f;
	public GameObject objPlayer;
	public GameObject waypointSpawn;
	public GameObject waypointPuzzle1;
	public GameObject waypointFinal;

	public enum GameState { STATE_INVALID, STATE_STARTUP, STATE_PUZZLE1, STATE_COMPLETE }
	private GameState gameState = GameState.STATE_INVALID;
	//private GameState gameAutoProgress = GameState.STATE_INVALID;

	private float timeAutoAction = -1f;
	private GameObject autoDisable = null;
	private GameObject autoEnable = null;

	// Use this for initialization
	protected void Start () {
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
	protected void Update () {
		if (timeAutoAction > 0.0f && Time.time>timeAutoAction) {
			if (autoDisable) {
				autoDisable.SetActive (false);
				autoDisable = null;
			}
			if (autoEnable) {
				autoEnable.SetActive (true);
				autoEnable = null;
			}
			timeAutoAction = -1f;
		}
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
			iTween.CameraFadeAdd (iTween.CameraTexture (Color.black));
			iTween.CameraFadeFrom (iTween.Hash ("amount", 1.0f, "time", 2.0f));
			objActive = waypointSpawn;
			waypointFinal.SetActive (false);
			waypointPuzzle1.SetActive (false);
			waypointSpawn.SetActive (true);
			ManipulateShip (false);
			ManipulateDoor (false);
			//gameAutoProgress = GameState.STATE_PUZZLE1;
			//objHud.ActivateHUD ("Welcome to the Puzzle, version 1.");
			break;
		case GameState.STATE_PUZZLE1:
			objActive = waypointPuzzle1;
			waypointFinal.SetActive (false);
			waypointSpawn.SetActive (false);
			waypointPuzzle1.SetActive (true);
			ManipulateDoor (true);
			//objHud.DeactivateHUD ();
			break;
		case GameState.STATE_COMPLETE: 
			objActive = waypointFinal;
			waypointPuzzle1.SetActive (false);
			waypointSpawn.SetActive (false);
			ManipulateShip (true);
			autoEnable = waypointFinal;
			timeAutoAction = Time.time + 5f;
			break;
		}
		Debug.Log ("GameController: Entering new state: " + gameState);

		//allow position and rotation to move...
		if (objActive) {
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

	void ManipulateShip(bool activate) {
		RandomSpinner rs = objShip.GetComponent<RandomSpinner> ();
		positionShip [1].SetActive (activate);
		positionShip [0].SetActive (!activate);
		if (activate) { 
			rs.ToggleRotate (false);
			positionShip [1].SetActive (true);
			iTween.MoveTo (objShip, 
				iTween.Hash (
					"position", positionShip [1].transform.position, 
					"time", 3f, "delay", 2f,
					"easetype", "easeInExpo"
				)
			);
			for (int i = 2; i < positionShip.Length; i++) {
				positionShip [i].SetActive (true);
				iTween.MoveTo (objShip, 
					iTween.Hash (
						"position", positionShip [i].transform.position, 
						"time", 1f, "delay", 3f+i*1f,
						"easetype", "linear"
					)
				);
			}
		}
		else {
			objShip.transform.position = positionShip [0].transform.position;
			for (int i = 0; i < positionShip.Length; i++) {
				positionShip [i].SetActive (false);
			}
			rs.ToggleRotate (true);
		}
	}

	void ManipulateDoor(bool activate) {
		positionDoor [1].SetActive (activate);
		positionDoor [0].SetActive (!activate);
		if (activate) { 
			iTween.MoveTo(objDoor, 
				iTween.Hash(
					"position", positionDoor[1].transform.position, 
					"time", 1, "delay", 1.5f,
					"easetype", "easeInExpo"
				)
			);
		}
		else {
			iTween.MoveTo(objDoor, 
				iTween.Hash(
					"position", positionDoor[0].transform.position, 
					"time", 1, 
					"easetype", "easeOutExpo"
				)
			);
		}
	}

}
