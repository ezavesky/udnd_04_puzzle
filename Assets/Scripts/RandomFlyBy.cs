using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlyBy : MonoBehaviour {
	public GameObject objStartRef1; //starting point for random fly
	public GameObject objEndRef1; //ending point for random fly
	public GameObject objStartRef2; //starting point for random fly
	public GameObject objEndRef2; //ending point for random fly
	public GameObject[] prefabRandSet;  //set of prefabs for random fly
	public float timeMaxTTL = 40;	//max seconds for TTL
	public float timeMinTTL = 10;	//min seconds for TTL
	public int randomCount = 4; //how many random objects exist at a time?
	public float scaleMin = 100;  //min scale for random objects
	public float scaleMax = 300;  //max scale for random objects

	public class RandObj {	//all properties of our random object
		public GameObject obj;
		public float ttl;
		public float clock;
		public Vector3 start;
		public Vector3 end;
	}
	private List<RandObj> activeRandList = new List<RandObj>();

	void Start() {
		//randomly generate a few objects to exist in the scene
		int randInit = Random.Range(3, randomCount);
		while (activeRandList.Count < randInit) {
			SpawnObject ();
			RandObj objMove = activeRandList [activeRandList.Count - 1];
			objMove.clock += Random.Range (0.0f, 1.0f) * objMove.ttl;   // update progress somewhere in the middle
		}

	}

	// create a new random object
	private void SpawnObject() {
		RandObj objNew = new RandObj ();
		objNew.start = Vector3.Lerp(objStartRef1.transform.position, objStartRef2.transform.position, Random.Range(0.0f, 1.0f));
		objNew.end = Vector3.Lerp(objEndRef1.transform.position, objEndRef2.transform.position, Random.Range(0.0f, 1.0f));
		objNew.obj = Instantiate (prefabRandSet [Random.Range (0, prefabRandSet.Length)], objNew.start, Quaternion.identity);
		objNew.obj.AddComponent<RandomSpinner> ();	//add new random spinner to object
		RandomSpinner spinner = objNew.obj.GetComponent<RandomSpinner>();
		spinner.rotateMagnitude = Random.Range (0.5f, 6.0f);
		objNew.obj.transform.parent = transform;  //add it as a new child of this object
		float scaleRand = Random.Range (scaleMin, scaleMax);
		objNew.obj.transform.localScale += new Vector3 (scaleRand, scaleRand, scaleRand);
		objNew.ttl = Random.Range (timeMinTTL, timeMaxTTL);  //random time to live
		objNew.clock = 0;
		activeRandList.Add (objNew);

		//Debug.Log ("TTL:"+objNew.ttl+", start:"+objNew.start+", end:"+objNew.end+", scale:"+scaleRand+", spin:"+spinner.rotateMagnitude);
	}

	// Update is called once per frame
	void Update () {
		for (int i = activeRandList.Count - 1; i >= 0; i--) //loop thorugh each object, move it along
		{
			RandObj objMove = activeRandList [i];
			objMove.clock += Time.deltaTime;
			float ratio = objMove.clock / objMove.ttl;
			if (ratio > 1.0) {  //moved beyond life time, delete
				Destroy(objMove.obj);  //remove object from game
				activeRandList.RemoveAt(i);  // remove tracked object
			} else {
				objMove.obj.transform.position = Vector3.Lerp (objMove.start, objMove.end, ratio);
			}
		}  //end iteration of active objects

		//do we need to spawn a new obejct?
		if (activeRandList.Count < randomCount) {
			SpawnObject ();
		}
			
	}
}
