using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSetup 
{
	public string charName;
	public bool isTeacher;
	public Texture2D sceneImg;
	public Texture2D enlargedImg;
	public int xPos;
	public int yPos;	
	public List<SequenceSetup> sequence;
}
