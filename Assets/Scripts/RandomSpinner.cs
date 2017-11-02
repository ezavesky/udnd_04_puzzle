using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpinner : MonoBehaviour {
	public float rotateMagnitude = 10;

	private bool rotateActive = true;
	private Vector3 rotateVelocity;
	private Vector3 rotateInitial;

	// Use this for initialization
	void Start () {
		rotateVelocity = Random.onUnitSphere;
		rotateInitial = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Force: "+rotateMagnitude*rotateVelocity+", rotation:"+transform.rotation);
		if (rotateActive) {
			transform.Rotate (rotateVelocity * rotateMagnitude);
		}
	}

	public void ToggleRotate(bool newRotate) {
		rotateActive = newRotate;
		iTween.RotateTo (gameObject, rotateInitial, 2f);
	}

}
