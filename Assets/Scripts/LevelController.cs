using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	public List<LevelSetup> levels;
	public RawImage background;
	int currentLevel = 0;

	// Use this for initialization
	void Start () {
		var tempLvl = levels [0];
		background.texture = tempLvl.levelBackground;
		Debug.Log (tempLvl.characters.Count);
		foreach (CharacterSetup tempChar in tempLvl.characters) {
			GameObject tempGO = new GameObject ("Lmao");
			tempGO.AddComponent<Image>();
			tempGO.GetComponent<Image> ().sprite = (Sprite) tempChar.charImg;
			tempGO.transform.position = new Vector2 (0.5f, 0.5f);
			//GUITexture tempImg = new GUITexture ();
			//tempImg.texture = tempChar.charImg;
			//Vector2 tempPos = new Vector2 (tempChar.xPos, tempChar.yPos);
			//Instantiate (tempImg);
			//could be added to an array if a reference to the images needs to be stored somewhere
		}
		Debug.Log ("After Loops");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 10)) {
			if (hit.transform.gameObject.tag == "Character") {
				Debug.Log ("Clicked a char");
				hit.transform.gameObject.GetComponent<DialogueScript> ().StartConvo ();
			}
		}
	}
}
