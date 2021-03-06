﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems ;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public List<LevelSetup> levels;
	public List<GameObject> levelContent = new List<GameObject>();
	private List<GameObject> characterList = new List<GameObject> ();
	private bool[] levelExists;
	public RawImage background;
	int currentLevel = 0;
	Camera cam;
	public GraphicRaycaster GRcaster;
	public GameObject convoPanel;
	public GameObject optionsPanel;
	public GameObject lastNamePanel;
	public GameObject genericPanel;
	private bool optionsActive;
	private GameObject levelGO;
	private List<string> clickOrder = new List<string>();
	public bool convoStarted = false;
	private float oldTime = 0;
	private string lastName = "";
	AudioSource audioSrc;
	public AudioClip bgMusic;

	// Use this for initialization
	void Start () {
		audioSrc = this.gameObject.GetComponent<AudioSource> ();
		//audioSrc.clip = bgMusic;
		//audioSrc.Play ();
		levelExists = new bool[levels.Count];
		cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		BuildLevel (0);
		oldTime = Time.time;
	}

	void BuildLevel(int level)
	{
		currentLevel = level;
		var tempLvl = levels [level];
		background.texture = tempLvl.levelBackground;
		//Debug.Log (tempLvl.characters.Count);
		var canvas = GameObject.FindWithTag ("mainCanvas");
		levelGO = new GameObject ("Level" + currentLevel + "Content");
		levelGO.transform.SetParent (canvas.gameObject.transform);
		levelGO.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		levelContent.Add (levelGO);
		foreach (CharacterSetup tempChar in tempLvl.characters) {
			GameObject tempGO = new GameObject (tempChar.charName);
			tempGO.AddComponent<DialogueScript> ();
			tempGO.GetComponent<DialogueScript> ().sequences = tempChar.sequence;
			tempGO.GetComponent<DialogueScript> ().charName = tempChar.charName;
			tempGO.GetComponent<DialogueScript> ().sceneImg = tempChar.sceneImg;
			tempGO.GetComponent<DialogueScript> ().enlargedImg = tempChar.enlargedImg;
			tempGO.AddComponent<Image> ();
			var xRatio = (Screen.width / 100) * tempChar.xPos;
			var wRatio = (Screen.width / 100) * tempChar.sceneImg.width;
			var yRatio = (Screen.height / 100) * tempChar.yPos;
			if (tempChar.isTeacher) {
				tempGO.gameObject.transform.localScale = new Vector3 (1.5f, 3f, 1f);
			} else {
				tempGO.gameObject.transform.localScale = new Vector3 (1f, 2f, 1f);
			}
			tempGO.gameObject.transform.SetParent (levelGO.transform, false);
			tempGO.GetComponent<Image> ().sprite = Sprite.Create (tempChar.sceneImg, new Rect (0f, 0f, tempChar.sceneImg.width, tempChar.sceneImg.height), new Vector2 (0f, 0f), 100f);
			tempGO.gameObject.tag = "Character";
			tempGO.transform.position = new Vector2 (xRatio, yRatio);
			tempGO.GetComponent<DialogueScript> ().originPos = new Vector2 (xRatio, yRatio);
			characterList.Add (tempGO);
			if (tempChar.isTeacher) {
				tempGO.GetComponent<DialogueScript> ().isTeacher = true;
				tempGO.GetComponent<DialogueScript> ().isTalking = true;
				convoStarted = true;
				//convoPanel.SetActive (true);
				tempGO.transform.position = new Vector2 (100f, 100f);
				tempGO.gameObject.transform.localScale = new Vector3 (2f, 4f, 2f);
				tempGO.transform.GetComponent<Image> ().sprite = Sprite.Create (tempChar.enlargedImg, new Rect (0f, 0f, tempChar.enlargedImg.width, tempChar.enlargedImg.height), new Vector2 (0f, 0f), 100f);
				tempGO.GetComponent<DialogueScript> ().StartConvo (convoPanel, this, audioSrc);
			}
		}
		levelExists[currentLevel] = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			PointerEventData pointer = new PointerEventData (null);
			pointer.position = Input.mousePosition;
			List<RaycastResult> results = new List <RaycastResult> ();
			GRcaster.Raycast (pointer, results);
			foreach (RaycastResult target in results) {
				DialogueScript tempDia = target.gameObject.GetComponent<DialogueScript> ();
				if(	tempDia != null){
					if (!tempDia.isTalking && !convoStarted) {
						if(tempDia.convoCompleted()){
							genericPanel.GetComponentInChildren<Text> ().text = tempDia.charName + " heeft niks meer te zeggen!";
							genericPanel.SetActive (true);
							return;
						}
						var charList = GameObject.FindGameObjectsWithTag ("Character");
						if (tempDia.isTeacher) {
							int completedCount = 0;
							foreach (var character in characterList) {
								if (characterList.Count == 1) {
									Debug.Log ("Only 1 char");
									genericPanel.GetComponentInChildren<Text> ().text = "Je moet eerst met iedereen praten!";
									genericPanel.SetActive (true);
									return;
								}
								if (character.GetComponent<DialogueScript> () != null) {
									/*if (character.GetComponent<DialogueScript> ().isTeacher) {
										return;
									}*/
									if (character.GetComponent<DialogueScript> ().convoCompleted ()) {
										completedCount++;
										Debug.Log ("Completed count =" + completedCount);
									}
								}
							}
							if (completedCount < characterList.Count - 1) {
								if (!genericPanel.activeInHierarchy) {
									genericPanel.GetComponentInChildren<Text> ().text = "Je moet eerst met iedereen praten!";
									genericPanel.SetActive (true); 
								}
								return;
							}

						}
						Debug.Log ("Clicked a char!");
						tempDia.isTalking = true;
						convoStarted = true;
						target.gameObject.transform.position = new Vector2 (100f, 100f);
						target.gameObject.transform.localScale = new Vector3 (2f, 4f, 2f);
						clickOrder.Add (tempDia.charName);
						target.gameObject.transform.GetComponent<Image> ().sprite = Sprite.Create (tempDia.enlargedImg, new Rect (0f, 0f, tempDia.enlargedImg.width, tempDia.enlargedImg.height), new Vector2 (0f, 0f), 100f);
						tempDia.StartConvo (convoPanel, this, audioSrc);
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			if (optionsActive) {
				optionsPanel.SetActive (false);
				optionsActive = false;
			} else {
				optionsPanel.SetActive (true);
				optionsActive = true;
			}
		}
	}

	public void SwitchLevel(bool goRight)
	{
		var levelC = GameObject.Find ("Level" + currentLevel + "Content");
		//AnalyticsData ();
		ResetPanel();
		convoStarted = false;
		convoPanel.SetActive (false);
		levelGO.SetActive (false);
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
		if (levelExists [nextLvl]) { 
			Debug.Log ("Loading lvl" + nextLvl);
			LoadLevel (nextLvl);
		} else {
			Debug.Log ("Building lvl" + nextLvl);
			BuildLevel (nextLvl);
		}
	}

	void ResetPanel()
	{
		var charList = GameObject.FindGameObjectsWithTag ("Character");
		foreach (var character in charList) {
			if (character.GetComponent<DialogueScript> () != null) {
				var tempDia = character.GetComponent<DialogueScript> ();
				if (tempDia.isTalking) {
					tempDia.ResetPanel ();
				}
			}
		}
	}

	void LoadLevel(int level){
		
		levelContent[currentLevel].SetActive(false);
		foreach (var GO in levelContent) {
			Debug.Log (levelContent);
		}

		levelContent [level].SetActive (true);
		var tempLvl = levels [level];
		background.texture = tempLvl.levelBackground;
		currentLevel = level;
	}

	public void AnalyticsData()
	{
		foreach (GameObject container in levelContent) {
			container.SetActive (true);
		}
		Dictionary<string, object> answerDic = new Dictionary<string,object>();
		string tempStr = "";
		var charList = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject character in charList) {
			Debug.Log (character);
			if (character.GetComponent<DialogueScript> () != null) {
				var tempDia = character.GetComponent<DialogueScript> ();
				answerDic = new Dictionary<string,object>();
				if (tempDia.isTalking) {
					tempDia.ResetPanel ();
				}
				var answerList = tempDia.getAnswers ();
				tempStr = "" + lastName + " : ";
				foreach (int answer in answerList) {
					tempStr += answer + ", ";
				}
				answerDic.Add (tempDia.charName + "'s closed", tempStr);
				var openAnswerList = tempDia.getOpenAnswers ();
				tempStr = "" + lastName + " : ";
				foreach (string answer in openAnswerList) {
					tempStr += answer + ", ";
				}
				answerDic.Add (tempDia.charName + "'s open", tempStr);
				openAnswerList = tempDia.getTimes();
				tempStr = "" + lastName + " : ";
				foreach (string time in openAnswerList) {
					tempStr += time + ", ";
				}
				answerDic.Add (tempDia.charName + "'s times", tempStr);
				Analytics.CustomEvent (tempDia.charName + "'s stats", answerDic);
			}
		}
		answerDic = new Dictionary<string,object>();
		tempStr = "" + lastName + " :";
		foreach (string click in clickOrder) {
			tempStr += click + ", ";
		}
		answerDic.Add ("ClickOrder", tempStr);
		tempStr = "" + lastName + " : " + (Time.time - oldTime);
		answerDic.Add ("Level time", tempStr);
		Analytics.CustomEvent ("General Stats", answerDic);
	}

	public void ReturnMain()
	{
		AnalyticsData ();
		SceneManager.LoadScene (0);
	}

	public void closeNotCompletedPanel()
	{
		genericPanel.SetActive (false);
	}

	public void SetLastName()
	{
		lastName = lastNamePanel.GetComponentInChildren<InputField> ().text;
		lastNamePanel.SetActive (false);
		Debug.Log (lastName);
	}
}
