using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems ;

public class LevelController : MonoBehaviour {

	public List<LevelSetup> levels;
	public RawImage background;
	int currentLevel = 0;
	Camera cam;
	public GraphicRaycaster GRcaster;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		var tempLvl = levels [0];
		background.texture = tempLvl.levelBackground;
		Debug.Log (tempLvl.characters.Count);
		var canvas = GameObject.FindWithTag ("mainCanvas");
		foreach (CharacterSetup tempChar in tempLvl.characters) {
			GameObject tempGO = new GameObject ("Lmao");
			tempGO.AddComponent<DialogueScript>();
			tempGO.GetComponent<DialogueScript>().questions = tempChar.questions;
			tempGO.AddComponent<Image>();
			tempGO.GetComponent<Image> ().sprite = Sprite.Create( tempChar.charImg, new Rect(0f, 0f, tempChar.charImg.width, tempChar.charImg.height), new Vector2(0f, 0f), 100f);

			tempGO.gameObject.transform.parent = canvas.gameObject.transform;
			tempGO.gameObject.tag = "Character";
			tempGO.transform.position = new Vector2 (tempChar.xPos, tempChar.yPos);
		}
		Debug.Log ("After Loops");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			PointerEventData pointer = new PointerEventData (null);
			pointer.position = Input.mousePosition;
			List<RaycastResult> results = new List <RaycastResult> ();
			GRcaster.Raycast (pointer, results);
			foreach (RaycastResult target in results) {
				if(	target.gameObject.transform.GetComponent<DialogueScript> ()!= null)
				{
					Debug.Log ("Clicked a char!");
					target.gameObject.transform.GetComponent<DialogueScript> ().StartConvo ();
				}

			}
		}
	}
}
