using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	
	public void StartButtonClick()
	{
		SceneManager.LoadScene (1);
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
