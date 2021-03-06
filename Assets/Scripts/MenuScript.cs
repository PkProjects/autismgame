﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public GameObject panels;
	AudioSource audioS = new AudioSource();
	public AudioClip bgMusic;
	private bool isPanelsActive = false;

	void Start()
	{
		audioS = this.gameObject.GetComponent<AudioSource> ();
		audioS.PlayOneShot (bgMusic);
	}

	public void StartButton1Click()
	{
		SceneManager.LoadScene (3);
		PlayerPrefs.SetInt ("Scenario", 3);
	}

    public void StartButton2Click()
    {
		SceneManager.LoadScene (3);
		PlayerPrefs.SetInt ("Scenario", 3);
    }

	public void LevelsButtonClick()
	{
		if (isPanelsActive) {
			panels.SetActive (false);
			isPanelsActive = false;
		} else {
			panels.SetActive (true);
			isPanelsActive = true;
		}
	}

    public void OptionsButtonClick()
	{
		Debug.Log ("Yeah this is a work in progress or smt");
	}

	public void ExitButtonClick()
	{
		Application.Quit ();
	}


}
