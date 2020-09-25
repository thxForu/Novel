using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputScreen : MonoBehaviour {

	public static InputScreen instance;

	public TMP_InputField inputField;
	/// <summary>
	/// The current input delared by the player on the input screen.
	/// </summary>
	/// <value>The current input.</value>
	public static string currentInput{get{return instance.inputField.text;}}

	public TitleHeader header;

	public GameObject root;

	void Awake()
	{
		instance = this;
		Hide();
	}

	public static void Show(string title, bool clearCurrentInput = true)
	{
		instance.root.SetActive(true);

		if (clearCurrentInput)
			instance.inputField.text = "";

		//only show a title if one is given.
		if (title != "")
			instance.header.Show(title);
		else
			instance.header.Hide();

		if (isRevealing)
			instance.StopCoroutine(revealing);

		revealing = instance.StartCoroutine(Revealing());
	}

	public static void Hide()
	{
		instance.root.SetActive(false);
		instance.header.Hide();
	}

	public static bool isShowingInputField {get{return instance.inputField.gameObject.activeInHierarchy;}}
	public static bool isWaitingForUserInput {get{return instance.root.activeInHierarchy;}}
	public static bool isRevealing {get{return revealing != null;}}
	static Coroutine revealing = null;
	static IEnumerator Revealing()
	{
		instance.inputField.gameObject.SetActive(false);

		while(instance.header.isRevealing)
			yield return new WaitForEndOfFrame();

		instance.inputField.gameObject.SetActive(true);

		revealing = null;
	}

	/// <summary>
	/// Accept the current input and close the screen.
	/// </summary>
	public void Accept()
	{
		Hide();
	}
}
