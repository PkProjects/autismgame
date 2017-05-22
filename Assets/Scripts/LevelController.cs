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
	public GameObject convoPanel;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		var tempLvl = levels [0];
		background.texture = tempLvl.levelBackground;
		//Debug.Log (tempLvl.characters.Count);
		var canvas = GameObject.FindWithTag ("mainCanvas");
		foreach (CharacterSetup tempChar in tempLvl.characters) {
			GameObject tempGO = new GameObject ("Lmao");
			tempGO.AddComponent<DialogueScript>();
			tempGO.GetComponent<DialogueScript>().questions = tempChar.questions;
			tempGO.GetComponent<DialogueScript>().sceneImg = tempChar.sceneImg;
			tempGO.GetComponent<DialogueScript>().enlargedImg = tempChar.enlargedImg;
			tempGO.AddComponent<Image>();
			var xRatio = (Screen.width/100) * tempChar.xPos;
			var wRatio = (Screen.width/100) * tempChar.sceneImg.width;
			var yRatio = (Screen.height/100) * tempChar.yPos;
			tempGO.gameObject.transform.localScale = new Vector3 (1f, 2f, 1f);
			tempGO.gameObject.transform.SetParent (canvas.gameObject.transform, false);
			tempGO.GetComponent<Image> ().sprite = Sprite.Create( tempChar.sceneImg, new Rect(0f, 0f, tempChar.sceneImg.width , tempChar.sceneImg.height), new Vector2(0f, 0f), 100f);

			tempGO.gameObject.tag = "Character";
			tempGO.transform.position = new Vector2 (xRatio, yRatio);
		}
		//Debug.Log ("After Loops");
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
					convoPanel.SetActive (true);
					target.gameObject.transform.position = new Vector2 (100f, 100f);
					DialogueScript tempDia = target.gameObject.GetComponent<DialogueScript> ();
					target.gameObject.transform.GetComponent<Image> ().sprite = Sprite.Create (tempDia.enlargedImg, new Rect (0f, 0f, tempDia.enlargedImg.width, tempDia.enlargedImg.height), new Vector2 (0f, 0f), 100f);
					tempDia.StartConvo ();
				}
			}
		}
	}
}
