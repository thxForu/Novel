using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceScreenTesting : MonoBehaviour {

	public string title = "I like...";
	public string[] choices;
	
	// Update is called once per frame
	void Start () 
	{
		StartCoroutine(DynamicStoryExample());
	}

	IEnumerator DynamicStoryExample()
	{
		//Load story part 1
		NovelController.instance.LoadChapterFile("story_1"); yield return new WaitForEndOfFrame();
		while(NovelController.instance.isHandlingChapterFile)
			yield return new WaitForEndOfFrame();

		ChoiceScreen.Show("What will you do?", "Go with Terrance", "Stay where you are");
		while(ChoiceScreen.isWaitingForChoiceToBeMade)
			yield return new WaitForEndOfFrame();

		//you chose to go with Terrance
		if (ChoiceScreen.lastChoiceMade.index == 0)
			NovelController.instance.LoadChapterFile("story_a1");
		//you chose to stay where you are
		else
			NovelController.instance.LoadChapterFile("story_b1");
		
		yield return new WaitForEndOfFrame();
		NovelController.instance.Next();

		while(NovelController.instance.isHandlingChapterFile)
			yield return new WaitForEndOfFrame();
	}
}
