using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public List<QuestionSetup> questions;// = new List<questionSetup> ();
	public QuestionSetup[] otherquestions;
	public Text mainQuestionText;
	public GameObject Button1;
	public GameObject Button2;
	public GameObject Button3;
	int currentQuestion = 0;
	public RawImage mainImage;
	public Texture something;

	void Start()
	{
		NextQuestion ();
	}

	void NextQuestion()
	{
		var nextQuestion = questions [currentQuestion];
		if (PlayerPrefs.GetInt("Scenario") == 1){
			mainImage.texture = (Texture) Resources.Load ("imgQuestion" + currentQuestion); //something;
		}else if (PlayerPrefs.GetInt("Scenario") == 2){
			mainImage.texture = (Texture) Resources.Load ("imgQuery" + currentQuestion); //something;
		}
		mainQuestionText.text = nextQuestion.mainQuestion;
		Button1.GetComponentInChildren<Text> ().text = nextQuestion.answerOne;
		Button2.GetComponentInChildren<Text> ().text = nextQuestion.answerTwo;
		Button3.GetComponentInChildren<Text> ().text = nextQuestion.answerThree;
	}

	public void ButtonOneClicked()
	{
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
