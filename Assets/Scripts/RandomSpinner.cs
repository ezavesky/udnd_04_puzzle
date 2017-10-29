using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpinner : MonoBehaviour {
	public float rotateMagnitude = 10;

	private Vector3 rotateVelocity;

	// Use this for initialization
	void Start () {
		rotateVelocity = Random.onUnitSphere;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Force: "+rotateMagnitude*rotateVelocity+", rotation:"+transform.rotation);
		transform.Rotate (rotateVelocity * rotateMagnitude);
	}
}
