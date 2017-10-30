using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {
	public float intensity = 0.5f;
	public float period = 2f;

	private Vector3 positionInitial;
	private Vector3 positionTop;
	private Vector3 positionBottom;
	private bool fadeToTop = false;
	private float timeExecuted;

	// Use this for initialization
	void Start () {
		positionInitial = transform.position;
		positionTop = positionInitial + (Vector3.up * intensity / 2);
		positionBottom = positionInitial - (Vector3.up * intensity / 2);
		fadeToTop = Random.Range (0.0f, 1.0f) > 0.5 ? true : false;
		timeExecuted = Random.Range (0.0f, period);
	}
	
	// Update is called once per frame
	void Update () {
		timeExecuted += Time.deltaTime;
		var ratio = timeExecuted / period;
		ratio = Mathf.Clamp01(ratio);
		if (fadeToTop) {
			transform.position = Vector3.Slerp (positionInitial, positionTop, ratio);
		} else {
			transform.position = Vector3.Slerp (positionInitial, positionBottom, ratio);
		}

		if (ratio >= 1f)
		{
			positionInitial = transform.position;
			timeExecuted = 0;
			fadeToTop = !fadeToTop;
		}
	}
}
