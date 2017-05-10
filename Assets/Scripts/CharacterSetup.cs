using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSetup 
{
	public string charName;
	public Texture charImg;
	public int xPos;
	public int yPos;	
	public List<QuestionSetup> questions;
}
