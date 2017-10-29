using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudInteraction: MonoBehaviour {
	public GameObject objHud;
	public GameObject objDescription;
	public GameObject objReticle;

	public GameObject objButton;
	public Text textTitle;
	public Text textDetails;

	public void ActivateHUD(string newDetails, string newTitle="Mission Control") {
		textTitle.text = newTitle;
		textDetails.text = newDetails;
		HideMessage (true);
	}

	public void HideMessage(bool bShow=false) {
		objHud.SetActive (!bShow);
		objDescription.SetActive (bShow);
		objReticle.SetActive (!bShow);
	}

}
