using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class architectTesting : MonoBehaviour {

	public TextMeshProUGUI tmprotext;

	[TextArea(5,10)]
	public string say;
	public int charactersPerFrame = 1;
	public float speed = 1f;

	// Use this for initialization
	void Start () 
	{
		new TextArchitect(tmprotext, say, "", charactersPerFrame, speed);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			new TextArchitect(tmprotext, say, "", charactersPerFrame, speed);
		}
	}
}
