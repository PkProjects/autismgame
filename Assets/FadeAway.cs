using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAway : MonoBehaviour {

	Image picture;
	float oldTime = 0;

	// Use this for initialization
	void Start () {
		oldTime = Time.time;
		picture = this.GetComponent<Image> ();
		picture.CrossFadeAlpha (0f, 3f, false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - oldTime > 3f) {
			this.gameObject.SetActive (false);
		}
	}
}
