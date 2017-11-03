using UnityEngine;
using System.Collections;

/* Final
 * This script represents what your LightUp.cs script might look like at the end of the course.
 * 
 * Do not use this script directly as the file and class names are incorrect.
 */
public class LightUp : MonoBehaviour
{
	// The initial color of the orb.
	private Color colorDefault;

	// The color used to light up the orb.
	public Color colorActive;
	public Color colorError;

	// The gameobject that has the GameLogic.cs script attached.
	public GameLogic gameLogic;

	public enum LIGHT_STATE { STATE_ACTIVE, STATE_ERROR, STATE_NORMAL };

	void Start()
	{
		// Assign the initial color of the orb as the default material.
		colorDefault = GetComponent<Renderer>().material.GetColor("_EmissionColor"); 
	}

	private void Light(LightUp.LIGHT_STATE newState) {
		Shader s = this.GetComponent<Shader> ();
		switch(newState) {
		default:
		case LIGHT_STATE.STATE_NORMAL:
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", colorDefault);
			break;
		case LIGHT_STATE.STATE_ACTIVE:
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", colorActive);
			break;
		case LIGHT_STATE.STATE_ERROR:
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", colorError);
			break;
		} 
	}
		
	// Called when the orb is clicked.
	// This function can be hooked up in Unity by adding a Pointer Click event trigger to the orb.
	public void PlayerSelection()
	{
		// Call the GameLogic.PlayerSelection(GameObject sphere) method (see GameLogic.cs script) passing in the orb 
		// this script is attached to.
		gameLogic.PlayerSelection(this.gameObject);

		// Get the GVR audio source component on this orb and play the audio.
		this.GetComponent<GvrAudioSource>().Play();
	}

	// Called when the reticle moves over the orb.
	// This function can be hooked up in Unity by adding a Pointer Enter event trigger to the orb.
	public void GazeLightUp()
	{
		// Assign the lightup material to the orb.
		Light(LIGHT_STATE.STATE_ACTIVE);
	}

	public void AestheticError(float duration)
	{
		PatternLightUp(duration, LIGHT_STATE.STATE_ERROR);
	}

	// Called when the reticle is moved away from orb.
	// This function can be hooked up in Unity by adding a Pointer Exit event trigger to the orb.
	public void AestheticReset()
	{
		// Revert to the orb's default material.
		Light(LIGHT_STATE.STATE_NORMAL);
	}

	// Lightup behavior for displaying the orb lightup pattern.
	// Called when the GameLogic.DisplayPattern() function is invoked (see GameLogic.cs script).
	public void PatternLightUp(float duration, LightUp.LIGHT_STATE newState=LIGHT_STATE.STATE_ACTIVE)
	{ 
		StartCoroutine(LightFor(duration, newState));
	}

	// Called from PatternLightUp(float duration) to light up the orb for a given duration.
	IEnumerator LightFor(float duration, LightUp.LIGHT_STATE newState)
	{ 
		// Assign the lightup material to the orb and play the audio...
		if (LIGHT_STATE.STATE_ACTIVE == newState) {
			// Assign the lightup material to the orb.
			Light(LIGHT_STATE.STATE_ACTIVE);
			// Get the GVR audio source component on this orb and play the audio.
			this.GetComponent<GvrAudioSource>().Play(); 
		}
		else {
			// Assign the lightup material to the orb.
			Light(newState);
		}

		// ...wait...
		yield return new WaitForSeconds(duration - 0.1f);

		// ...revert the material back to the orb's default material.
		AestheticReset();
	}

}
