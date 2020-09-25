using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTesting : MonoBehaviour {

	DialogueSystem ds;

	// Use this for initialization
	void Start () 
	{
		ds = DialogueSystem.instance;
	}

	public string[] thingsToSay = new string[5]
	{
		"Hi, how are you?:Avira",
		"What's your name?",
		"Oh, that's nice.",
		"So...",
		"What are you doing out here?:true"
	};
	int progress = 0;

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.S)) 
		{
			string item = progress < thingsToSay.Length ? thingsToSay[progress] : "We are done talking.:narrator";
			Say(item);
			progress++;
		}
	}

	void Say(string dialogueInfo)
	{
		string[] data = dialogueInfo.Split(':');

		string dialogue = data[0];
		string speaker = "";
		bool additive = false;

		for(int i = 1; i < data.Length; i++)
		{
			print("'" + data[i] + "'");
			if (data[i] != "true" && data[i] != "false")
			{
				print(data[i] + " is a name.");
				speaker = data[i];
			}
			else
			{
				print(data[i] + " signifies additive.");
				additive = bool.Parse(data[i]);
			}
		}

		ds.Say(dialogue, speaker, additive);
	}
}
