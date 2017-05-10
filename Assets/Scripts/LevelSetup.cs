using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSetup 
{
	public string levelName;
	public Texture levelBackground;
	public List<CharacterSetup> characters;
}
