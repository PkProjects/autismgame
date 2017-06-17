using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
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
	private GameObject levelGO;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		BuildLevel (0);
	}

	void BuildLevel(int level)
	{
		currentLevel = level;
		var tempLvl = levels [level];
		background.texture = tempLvl.levelBackground;
		//Debug.Log (tempLvl.characters.Count);
		var canvas = GameObject.FindWithTag ("mainCanvas");
		levelGO = new GameObject ("LevelContent");
		levelGO.transform.SetParent (canvas.gameObject.transform);
		levelGO.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		foreach (CharacterSetup tempChar in tempLvl.characters) {
			GameObject tempGO = new GameObject (tempChar.charName);
			tempGO.AddComponent<DialogueScript>();
			tempGO.GetComponent<DialogueScript>().questions = tempChar.questions;
			tempGO.GetComponent<DialogueScript>().charName = tempChar.charName;
			tempGO.GetComponent<DialogueScript>().sceneImg = tempChar.sceneImg;
			tempGO.GetComponent<DialogueScript>().enlargedImg = tempChar.enlargedImg;
			tempGO.AddComponent<Image>();
			var xRatio = (Screen.width/100) * tempChar.xPos;
			var wRatio = (Screen.width/100) * tempChar.sceneImg.width;
			var yRatio = (Screen.height/100) * tempChar.yPos;
			tempGO.gameObject.transform.localScale = new Vector3 (1f, 2f, 1f);
			tempGO.gameObject.transform.SetParent (levelGO.transform, false);
			tempGO.GetComponent<Image> ().sprite = Sprite.Create( tempChar.sceneImg, new Rect(0f, 0f, tempChar.sceneImg.width , tempChar.sceneImg.height), new Vector2(0f, 0f), 100f);

			tempGO.gameObject.tag = "Character";
			tempGO.transform.position = new Vector2 (xRatio, yRatio);
			tempGO.GetComponent<DialogueScript>().originPos = new Vector2 (xRatio, yRatio);
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
					//convoPanel.SetActive (true);
					target.gameObject.transform.position = new Vector2 (100f, 100f);
					DialogueScript tempDia = target.gameObject.GetComponent<DialogueScript> ();
					target.gameObject.transform.GetComponent<Image> ().sprite = Sprite.Create (tempDia.enlargedImg, new Rect (0f, 0f, tempDia.enlargedImg.width, tempDia.enlargedImg.height), new Vector2 (0f, 0f), 100f);
					tempDia.StartConvo (convoPanel);
				}
			}
		}
	}

	public void SwitchLevel(bool goRight)
	{
		var charList = GameObject.FindGameObjectsWithTag ("Character");
		Dictionary<string, object> customDic = new Dictionary<string,object>();
		foreach (var character in charList) {
			if (character.GetComponent<DialogueScript> () != null) {
				var tempDia = character.GetComponent<DialogueScript> ();
				var answerList = tempDia.getAnswers ();
				string tempStr = "";
				foreach (int answer in answerList) {
					tempStr += answer + ", ";
				}
				customDic.Add (tempDia.charName, tempStr);
			}
		}

		Analytics.CustomEvent ("switchScene", customDic);
		convoPanel.SetActive (false);
		Destroy (levelGO);
		int nextLvl = 0;
		if (goRight) {
			if (currentLevel + 1 < levels.Count) {
				nextLvl = currentLevel + 1;
			} else {
				nextLvl = 0;
			}
		} else {
			if (currentLevel - 1 <= 0) {
				nextLvl = levels.Count - 1;
			} else {
				nextLvl = currentLevel - 1;
			}
		}
		BuildLevel (nextLvl);
	}
}
