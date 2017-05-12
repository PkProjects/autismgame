using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public List<QuestionSetup> questions;// = new List<questionSetup> ();
	public Text mainQuestionText;
	public GameObject Button1;
	public GameObject Button2;
	public GameObject Button3;
	int currentQuestion = 0;
	public Texture2D sceneImg;
	public Texture2D enlargedImg;

	public void StartConvo()
	{
		Button1 = GameObject.FindWithTag("Button1");
		Button1.GetComponent<Button> ().onClick.AddListener (ButtonOneClicked);
		Button2 = GameObject.FindWithTag("Button2");	
		Button2.GetComponent<Button> ().onClick.AddListener (ButtonTwoClicked);
		Button3 = GameObject.FindWithTag("Button3");	
		Button3.GetComponent<Button> ().onClick.AddListener (ButtonThreeClicked);
		mainQuestionText = GameObject.FindWithTag ("MainQuestion").GetComponent<Text>();
		NextQuestion ();
	}

	void NextQuestion()
	{
		Debug.Log ("starting question");
		if (currentQuestion >= questions.Count) {
			Debug.Log ("Fake news");
			currentQuestion = questions.Count - 1;
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
		Debug.Log ("jwztest");
		currentQuestion = questions [currentQuestion].followUp1;
		NextQuestion ();
	}

	public void ButtonTwoClicked()
	{
		currentQuestion = questions [currentQuestion].followUp2;
		NextQuestion ();
	}

	public void ButtonThreeClicked()
	{
		currentQuestion = questions [currentQuestion].followUp3;
		NextQuestion ();
	}
}
