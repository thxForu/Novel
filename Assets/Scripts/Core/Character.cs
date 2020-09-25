using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character 
{
	public string characterName;
	/// <summary>
	/// The root is the container for all images related tot he character in the scene. The root object.
	/// </summary>
	[HideInInspector]public RectTransform root;

	public bool enabled {get{ return root.gameObject.activeInHierarchy;} set{ root.gameObject.SetActive (value);}}

	DialogueSystem dialogue;

	/// <summary>
	/// Make this character say something.
	/// </summary>
	/// <param name="speech">Speech.</param>
	public void Say(string speech, bool add = false)
	{
		if (!enabled)
			enabled = true;

		dialogue.Say (speech, characterName, add);
	}

	/// <summary>
	/// Create a new character.
	/// </summary>
	/// <param name="_name">Name.</param>
	public Character (string _name, bool enableOnStart = true)
	{
		CharacterManager cm = CharacterManager.instance;
		//locate the character prefab.
		GameObject prefab = Resources.Load ("Characters/Character[" + _name + "]") as GameObject;
		//spawn an instance of the prefab directly on the character panel.
		GameObject ob = Object.Instantiate(prefab, cm.characterPanel);

		root = ob.GetComponent<RectTransform> ();
		characterName = _name;

		dialogue = DialogueSystem.instance;

		enabled = enableOnStart;
	}
	

}
