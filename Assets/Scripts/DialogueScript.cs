using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public List<QuestionSetup> questions;// = new List<questionSetup> ();
	private List<int> answerLog = new List<int>();
	private List<string> openAnswers = new List<string>();
	private List<string> timeSpent = new List<string>();
	public string charName;
	public Text mainQuestionText;
	private GameObject Button1;
	private GameObject Button2;
	private GameObject Button3;
	int currentQuestion = 0;
	public Texture2D sceneImg;
	public Texture2D enlargedImg;
	public Vector2 originPos;
	private GameObject convoPanel;
	private GameObject activeChar;
	public List<SequenceSetup> sequences;
	private int currentStage = 0;
	private GameObject inputField;
	private bool isOpenQuestion = false;
	public bool isTalking = false;
	public bool isTeacher = false;
	private LevelController levelC;
	//public bool convoCompleted;
	private float oldTime = 0;
	private bool lastQuestion = false;
	private bool isCompleted = false;

	public void StartConvo(GameObject panel, LevelController level)
	{
		levelC = level;
		oldTime = Time.time;
		activeChar = this.gameObject;
		convoPanel = panel;
		convoPanel.SetActive (true);
		inputField = GameObject.FindWithTag("InputField");
		Button1 = GameObject.FindWithTag("Button1");
		Button1.GetComponent<Button> ().onClick.AddListener (ButtonOneClicked);
		Button2 = GameObject.FindWithTag ("Button2");
		Button2.GetComponent<Button> ().onClick.AddListener (ButtonTwoClicked);
		Button3 = GameObject.FindWithTag("Button3");	
		Button3.GetComponent<Button> ().onClick.AddListener (ButtonThreeClicked);
		mainQuestionText = GameObject.FindWithTag ("MainQuestion").GetComponent<Text>();
		NextQuestion ();
	}

	public void ResetPanel()
	{
		ResetCharacter ();
		isTalking = false;
		//levelC.convoStarted = false;
		inputField.SetActive (true);
		Button1.GetComponent<Button> ().onClick.RemoveListener (ButtonOneClicked);
		Button2.SetActive (true);
		Button2.GetComponent<Button> ().onClick.RemoveListener (ButtonTwoClicked);
		Button3.SetActive (true);
		Button3.GetComponent<Button> ().onClick.RemoveListener (ButtonThreeClicked);
		convoPanel.SetActive (false);
	}

	public void ResetCharacter()
	{
		activeChar.transform.position = originPos;
		activeChar.gameObject.transform.localScale = new Vector3 (1f, 2f, 1f);
		activeChar.GetComponent<Image> ().sprite = Sprite.Create (sceneImg, new Rect (0f, 0f, sceneImg.width, sceneImg.height), new Vector2 (0f, 0f), 100f);
	}

	void NextQuestion()
	{
		oldTime = Time.time;
		Debug.Log ("starting question " + currentQuestion);
		if (currentQuestion >= sequences[currentStage].questions.Count) {
			Debug.Log ("Ending convo");
			levelC.convoStarted = false;
			currentQuestion = 0;
			ResetCharacter ();
			if (currentStage < sequences.Count - 1) {
				currentStage++;
			} else {
				isCompleted = true;
				Debug.Log ("Finished all dialogue for this chracter");
			}
			ResetPanel ();
			isTalking = false;
			return;
		}
		questions = sequences [currentStage].questions;
		var nextQuestion = questions [currentQuestion];
		mainQuestionText.text = nextQuestion.mainQuestion;
		if (nextQuestion.isQuestion) {
			if (nextQuestion.isOpen) {
				isOpenQuestion = true;
				Button1.GetComponentInChildren<Text> ().text = "Next";
				inputField.SetActive (true);
			} else {
				inputField.SetActive (false);
				Button1.GetComponentInChildren<Text> ().text = nextQuestion.answerOne;
				if (nextQuestion.answerTwo != "") {
					Button2.SetActive (true);
					Button2.GetComponentInChildren<Text> ().text = nextQuestion.answerTwo;
				} else {
					Button2.SetActive (false);
				}
				if (nextQuestion.answerThree != "") {
					Button3.SetActive (true);
					Button3.GetComponentInChildren<Text> ().text = nextQuestion.answerThree;
				} else {
					Button3.SetActive (false);			
				}
			}
		} else {
			Button1.GetComponentInChildren<Text> ().text = "Next";
			inputField.SetActive (false);
			Button2.SetActive (false);
			Button3.SetActive (false);
		}
		if (nextQuestion.isLastQuestion) {
			lastQuestion = true;
		}
		//nextQuestion.voiceLine.Play ();
	}

	public void ButtonOneClicked()
	{
		//Debug.Log ("jwztest");
		float questionTime = Mathf.Round((Time.time - oldTime) * 100f) / 100f;
		if (isOpenQuestion) {
			openAnswers.Add (inputField.GetComponent<InputField> ().text);
			isOpenQuestion = false;
		} else {
			answerLog.Add (1);
		}
		timeSpent.Add ("" + currentStage + ", " + currentQuestion + ", " + questionTime);
		if (lastQuestion) {
			Debug.Log ("Restarin pls?");
			levelC.ReturnMain ();
			return;
		}
		currentQuestion = questions [currentQuestion].followUp1;
		NextQuestion ();
	}

	public void ButtonTwoClicked()
	{
		var questionTime = Time.time - oldTime;
		timeSpent.Add ("" + currentStage + ", " + currentQuestion + ", " + questionTime);
		currentQuestion = questions [currentQuestion].followUp2;
		answerLog.Add (2);
		NextQuestion ();
	}

	public void ButtonThreeClicked()
	{
		var questionTime = Time.time - oldTime;
		timeSpent.Add ("" + currentStage + ", " + currentQuestion + ", " + questionTime);
		currentQuestion = questions [currentQuestion].followUp3;
		answerLog.Add (3);
		NextQuestion ();
	}

	public bool convoCompleted()
	{
		/*if (currentStage >= sequences.Count - 1) {
			return true;
		} else {
			return false;
		}*/
		return isCompleted;
	}

	public List<int> getAnswers()
	{
		return answerLog;
	}

	public List<string> getOpenAnswers()
	{
		return openAnswers;
	}

	public List<string> getTimes()
	{
		return timeSpent;
	}
}
