using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// loosely dervied on code from this starter helper
//	https://stackoverflow.com/questions/29406845/create-pulsing-color-in-unity
public class ColorPulse : MonoBehaviour
{
	public float FadeDuration = 1f;
	public Color Color1 = Color.gray;
	public Color Color2 = Color.white;

	private bool fadeToFirst = false;
	private float timelastColorChange;

	private Image targetImage = null;
	private Material targetMaterial = null;

	void Start()
	{
		fadeToFirst = false;
		targetImage = GetComponent<Image>();
		if (targetImage == null) {
			targetMaterial = GetComponent<Renderer>().material;
		}
	}

	void Update()
	{
		var ratio = (Time.time - timelastColorChange) / FadeDuration;
		ratio = Mathf.Clamp01(ratio);
		Color colorNew;
		if (fadeToFirst)
			colorNew = Color.Lerp(Color2, Color1, ratio);
		else 
			colorNew = Color.Lerp(Color1, Color2, ratio);

		if (targetImage) {
			targetImage.color = colorNew;
		} else if (targetMaterial) {
			targetMaterial.color = colorNew;
		}
		
		//material.color = Color.Lerp(Color1, Color2, Mathf.Sqrt(ratio)); // A cool effect
		//material.color = Color.Lerp(Color1, Color2, ratio * ratio); // Another cool effect

		if (ratio == 1f)
		{
			timelastColorChange = Time.time;
			fadeToFirst = !fadeToFirst;  // Switch colors
		}
	}
}
