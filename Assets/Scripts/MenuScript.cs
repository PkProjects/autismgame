using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	
	public void StartButton1Click()
	{
		SceneManager.LoadScene (1);
		PlayerPrefs.SetInt ("Scenario", 1);
	}

    public void StartButton2Click()
    {
		SceneManager.LoadScene (2);
		PlayerPrefs.SetInt ("Scenario", 2);
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
