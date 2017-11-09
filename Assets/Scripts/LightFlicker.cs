using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
	public float speed = 1.5f;
	public float offMin = 0.05f;
	public float offMax = 0.2f;
	public int flickersMax = 5;

	private Light lightObj = null;
	private bool flickerEnabled = false;

	//Experience is a basic property
	public bool flicker
	{
		get
		{
			//Some other code
			return flickerEnabled;
		}
		set
		{
			//Some other code
			flickerEnabled = value;
			if (flickerEnabled) {
				StartCoroutine (FlickerRoutine (this.gameObject));
			}
		}
	}

	// Use this for initialization
	void Start () {
		lightObj = GetComponent<Light> ();
		flicker = true;
	}

	IEnumerator FlickerRoutine (GameObject obj){
		//lightObj.enabled = true;
		float randNoise = Random.Range (offMin, offMax);
		int randFlicks = Random.Range (1, flickersMax);
		Debug.Log ("LightFlicker: " + randFlicks + " flickers at " + randNoise + " frequency.");
		for (int i = 0; i < randFlicks; i++) {
			//lightObj.enabled = false;
			obj.SetActive (false);
			yield return new WaitForSeconds (randNoise);
			//lightObj.enabled = true;
			obj.SetActive (true);
			yield return new WaitForSeconds (randNoise);
		}
		yield return new WaitForSeconds(speed);
		if (flickerEnabled) {
			StartCoroutine (FlickerRoutine (obj));
		}
	}
}
