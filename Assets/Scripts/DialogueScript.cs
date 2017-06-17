using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public List<QuestionSetup> questions;// = new List<questionSetup> ();
	private List<int> answerLog = new List<int>();
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

	public void StartConvo(GameObject panel)
	{
		activeChar = this.gameObject;
		convoPanel = panel;
		convoPanel.SetActive (true);
		Button1 = GameObject.FindWithTag("Button1");
		Button1.GetComponent<Button> ().onClick.AddListener (ButtonOneClicked);
		Button2 = GameObject.FindWithTag ("Button2");
		Button2.GetComponent<Button> ().onClick.AddListener (ButtonTwoClicked);
		Button3 = GameObject.FindWithTag("Button3");	
		Button3.GetComponent<Button> ().onClick.AddListener (ButtonThreeClicked);
		mainQuestionText = GameObject.FindWithTag ("MainQuestion").GetComponent<Text>();
		NextQuestion ();
	}

	void NextQuestion()
	{
		Debug.Log ("starting question " + currentQuestion);
		if (currentQuestion >= questions.Count) {
			Debug.Log ("Ending convo");
			currentQuestion = 0;
			activeChar.transform.position = originPos;
			activeChar.GetComponent<Image> ().sprite = Sprite.Create( sceneImg, new Rect(0f, 0f, sceneImg.width , sceneImg.height), new Vector2(0f, 0f), 100f);

			Button1.GetComponent<Button> ().onClick.RemoveListener (ButtonOneClicked);
			Button2.SetActive (true);
			Button2.GetComponent<Button> ().onClick.RemoveListener (ButtonTwoClicked);
			Button3.SetActive (true);
			Button3.GetComponent<Button> ().onClick.RemoveListener (ButtonThreeClicked);
			convoPanel.SetActive (false);
			return;
		}
		var nextQuestion = questions [currentQuestion];
		mainQuestionText.text = nextQuestion.mainQuestion;
		if (nextQuestion.isQuestion) {
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
		} else {
			Button1.GetComponentInChildren<Text> ().text = "Next";
			Button2.SetActive (false);
			Button3.SetActive (false);
		}
	}

	public void ButtonOneClicked()
	{
		//Debug.Log ("jwztest");
		currentQuestion = questions [currentQuestion].followUp1;
		answerLog.Add (1);
		NextQuestion ();
	}

	public void ButtonTwoClicked()
	{
		currentQuestion = questions [currentQuestion].followUp2;
		answerLog.Add (2);
		NextQuestion ();
	}

	public void ButtonThreeClicked()
	{
		currentQuestion = questions [currentQuestion].followUp3;
		answerLog.Add (3);
		NextQuestion ();
	}

	public List<int> getAnswers()
	{
		return answerLog;
	}
}
