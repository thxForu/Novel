using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMasterTesting : MonoBehaviour 
{
	public Texture2D tex1;
	public Texture2D tex2;
	public Texture2D tex3;
	public Texture2D trans1;
	public Texture2D trans2;
	public Texture2D trans3;
	// Use this for initialization
	void Start () 
	{
		
	}

	int progress = 0;
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				progress = Mathf.Clamp(progress - 1, 0, 10);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				progress = Mathf.Clamp(progress + 1, 0, 10);
			}

			switch(progress)
			{
			case 0:
				TransitionMaster.ShowScene(false);
				break;
			case 1:
				TransitionMaster.ShowScene(true);
				break;
			case 2:
				TransitionMaster.TransitionLayer(BCFC.instance.background, tex1, trans1);
				break;
			case 3:
				TransitionMaster.TransitionLayer(BCFC.instance.background, tex2, trans2);
				break;
			case 4:
				TransitionMaster.TransitionLayer(BCFC.instance.background, tex3, trans3);
				break;
			case 5:
				BCFC.instance.background.TransitionToTexture(tex1);
				break;
			case 6:
				TransitionMaster.TransitionLayer(BCFC.instance.background, tex3, trans3);
				break;
			case 7:
				BCFC.instance.background.TransitionToTexture(tex2);
				break;
			case 8:
				TransitionMaster.TransitionLayer(BCFC.instance.background, null, trans1);
				break;
			case 9:
				BCFC.instance.background.TransitionToTexture(tex3);
				TransitionMaster.ShowScene(true);
				break;
			case 10:
				TransitionMaster.ShowScene(false);
				break;
			}
		}
	}
}
