using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
	public static NovelController instance;

	public GameSavePanel saveLoadPanel;
	public Animator animator;

	/// <summary> The lines of data loaded directly from a chapter file.	/// </summary>
	List<string> data = new List<string>();

	void Awake()
	{
		instance = this;
	}
	[HideInInspector]
	public string activeGameFileName = "";
	GAMEFILE activeGameFile
	{
		get { return GAMEFILE.activeFile; }
		set { GAMEFILE.activeFile = value; }
	}
	string activeChapterFile = "";

	// Use this for initialization
	void Start()
	{
		saveLoadPanel.gameObject.SetActive(false);
		LoadGameFile(FileManager.LoadFile(FileManager.savPath+"savData/file.txt")[0]);
	}

	bool encryptGameFile = true;

	public void LoadGameFile(string gameFileName)
	{
		activeGameFileName = gameFileName;

		string filePath = FileManager.savPath + "savData/gameFiles/" + activeGameFileName + ".txt";

		if (!System.IO.File.Exists(filePath))
		{
			activeGameFile = new GAMEFILE();//don't save because we want a new one to start whenever we hit new game. any save is manual.
		}
		else
		{
			if (encryptGameFile)
				activeGameFile = FileManager.LoadEncryptedJSON<GAMEFILE>(filePath, keys);
			else
				activeGameFile = FileManager.LoadJSON<GAMEFILE>(filePath);
		}

		//Load the file
		data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + activeGameFile.chapterName);
		activeChapterFile = activeGameFile.chapterName;
		cachedLastSpeaker = activeGameFile.cachedLastSpeaker;

		DialogueSystem.instance.Open(activeGameFile.currentTextSystemSpeakerNameText, activeGameFile.currentTextSystemDisplayText);

		//Load the layer images back into the scene
		if (activeGameFile.background != null)
			BCFC.instance.background.SetTexture(activeGameFile.background);
		if (activeGameFile.cinematic != null)
			BCFC.instance.cinematic.SetTexture(activeGameFile.cinematic);
		if (activeGameFile.foreground != null)
			BCFC.instance.foreground.SetTexture(activeGameFile.foreground);

		//start the music back up
		if (activeGameFile.music != null)
			AudioManager.instance.PlaySong(activeGameFile.music);

		if (handlingChapterFile != null)
			StopCoroutine(handlingChapterFile);
		handlingChapterFile = StartCoroutine(HandlingChapterFile());

		chapterProgress = activeGameFile.chapterProgress;
	}

	public void SaveGameFile()
	{
		string filePath = FileManager.savPath + "savData/gameFiles/" + activeGameFileName + ".txt";

		activeGameFile.chapterName = activeChapterFile;
		activeGameFile.chapterProgress = chapterProgress;
		activeGameFile.cachedLastSpeaker = cachedLastSpeaker;

		activeGameFile.currentTextSystemSpeakerNameText = DialogueSystem.instance.speakerNameText.text;
		activeGameFile.currentTextSystemDisplayText = DialogueSystem.instance.speechText.text;

		//get all the characters and save their stats.
		activeGameFile.charactersInScene.Clear();
		for (int i = 0; i < CharacterManager.instance.characters.Count; i++)
		{
			Character character = CharacterManager.instance.characters[i];
			GAMEFILE.CHARACTERDATA data = new GAMEFILE.CHARACTERDATA(character);
			activeGameFile.charactersInScene.Add(data);
		}

		//save the layers to disk
		BCFC b = BCFC.instance;
		activeGameFile.background = b.background.activeImage != null ? b.background.activeImage.texture : null;
		activeGameFile.cinematic = b.cinematic.activeImage != null ? b.cinematic.activeImage.texture : null;
		activeGameFile.foreground = b.foreground.activeImage != null ? b.foreground.activeImage.texture : null;

		//save the music to disk
		activeGameFile.music = AudioManager.activeSong != null ? AudioManager.activeSong.clip : null;

		//save the ambiance to disk if there is any playing.
		activeGameFile.ambiance = AudioManager.activeAmbianceClips;


		//save a preview image (screenshot) to be viewed from the save load screen
		string screenShotPath = FileManager.savPath + "savData/gameFiles/" + activeGameFileName + ".png";

		if (FileManager.TryCreateDirectoryFromPath(screenShotPath + ".png"))
		{
			GAMEFILE.activeFile.previewImage = ScreenCapture.CaptureScreenshotAsTexture();
			byte[] textureData = activeGameFile.previewImage.EncodeToPNG();
			FileManager.SaveComposingBytes(screenShotPath, textureData);
		}
		//save the data and time this file was created or modified
		activeGameFile.modificationDate = System.DateTime.Now.ToString();

		if (encryptGameFile)
			FileManager.SaveEncryptedJSON(filePath, activeGameFile, keys);
		else
			FileManager.SaveJSON(filePath, activeGameFile);
	}
	byte[] keys
	{
		get { return FileManager.keys; }
	}

	// Update is called once per frame
	void Update()
	{
		//testing
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			Next();
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			Back();
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			//only onpe the save load panel if we are able.
			if (InputScreen.isShowingInputField || ChoiceScreen.isWaitingForChoiceToBeMade)
				return;

			if (!saveLoadPanel.gameObject.activeInHierarchy)
			{
				saveLoadPanel.gameObject.SetActive(true);
				//update the display
				saveLoadPanel.LoadFilesOntoScreen(saveLoadPanel.currentSaveLoadPage);
			}
			else
			{
				saveLoadPanel.gameObject.SetActive(false);
			}
		}

		//testing as well
		if (Input.GetKeyDown(KeyCode.S))
		{
			SaveGameFile();
		}
	}

	public void LoadChapterFile(string fileName)
	{
		activeChapterFile = fileName;

		data = FileManager.ReadTextAsset(Resources.Load<TextAsset>($"Story/{fileName}"));
		cachedLastSpeaker = "";

		if (handlingChapterFile != null)
			StopCoroutine(handlingChapterFile);
		handlingChapterFile = StartCoroutine(HandlingChapterFile());

		//auto start the chapter.
		Next();
	}

	/// <summary>
	/// Trigger that advances the progress through a chapter file.
	/// </summary>
	bool _next = false;
	bool _back = false;
	/// <summary>
	/// Procede to the next line of a chapter or finish the line right now.
	/// </summary>
	public void Next()
	{
		_next = true;
		_back = false;
	}
	public void Back()
	{
		_back = true;
		_next = false;
	}

	public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
	Coroutine handlingChapterFile = null;
	[HideInInspector] public int chapterProgress = 0;
	IEnumerator HandlingChapterFile()
	{
		//the progress through the lines in this chapter.
		chapterProgress = 0;

		while (chapterProgress < data.Count)
		{
			//we need a way of knowing when the player wants to advance. We need a "next" trigger. Not just a keypress. But something that can be triggerd
			//by a click or a keypress
			if (_next)
			{
				string line = data[chapterProgress];//this is the line loaded in its pure format. No injections have taken place yet.
                //make sure the line has the proper data injected in it where it needs it.
                TagManager.Inject(ref line);//inject data into the line where it may be needed.
                //now our line will be properly formatted and ready to use.

				//this is a choice
				if (line.StartsWith("choice"))
				{
					yield return HandlingChoiceLine(line);
					chapterProgress++;
				}
				//this is user input
				else if (line.StartsWith("input"))
				{
					yield return HandlingInputLine(line);
					chapterProgress++;
				}
				//this is a normal line of dialogue and actions.
				else
				{
					HandleLine(line);
					chapterProgress++;
					while (isHandlingLine)
					{
						yield return new WaitForEndOfFrame();
					}
				}
			}

			else if (_back)
			{
				string line = data[chapterProgress];
				HandleLine(line);
				chapterProgress--;
				while (isHandlingLine)
				{
					yield return new WaitForEndOfFrame();
				}
				Debug.Log(chapterProgress);
			}
			yield return new WaitForEndOfFrame();
		}

		handlingChapterFile = null;
	}

	IEnumerator HandlingInputLine(string line)
	{
		string title = line.Split('"')[1];

		//get the one or more commands to execute when this input is done and accepted.
		string[] parts = line.Split(' ');
		List<string> endingCommands = new List<string>();
		if (parts.Length >= 3)
		{
			for (int i = 2; i < parts.Length; i++)
			{
				endingCommands.Add(parts[i]);
			}
		}

		//we have the title and the ending commands to execute. Now we need to bring up the input screen.
		InputScreen.Show(title);
		while (InputScreen.isShowingInputField || InputScreen.isRevealing)
		{
			//wait for the input screen to finish revealing before being able to accept input.
			if (Input.GetKey(KeyCode.Return) && !InputScreen.isRevealing)
			{
				//if the input is not empty, accept it.
				if (InputScreen.currentInput != "")
					InputScreen.instance.Accept();
			}

			yield return new WaitForEndOfFrame();
		}

		//the input has been accepted, now it is time to execute the commands that follow.
		for (int i = 0; i < endingCommands.Count; i++)
		{
			string command = endingCommands[i];
			HandleAction(command);
		}
	}
	IEnumerator HandlingChoiceLine(string line)
	{
		string title = line.Split('"')[1];
		List<string> choices = new List<string>();
		List<string> actions = new List<string>();

		bool gatheringChoices = true;
		while (gatheringChoices)
		{
			chapterProgress++;
			line = data[chapterProgress];

			if (line == "{")
				continue;

			line = line.Replace("    ", "");//remove the tabs that have become quad spaces.

			if (line != "}")
			{
				choices.Add(line.Split('"')[1]);
				actions.Add(data[chapterProgress + 1].Replace("    ", ""));
				chapterProgress++;
			}
			else
			{
				gatheringChoices = false;
			}
		}

		//display choices
		if (choices.Count > 0)
		{
			ChoiceScreen.Show(title, choices.ToArray()); yield return new WaitForEndOfFrame();
			while (ChoiceScreen.isWaitingForChoiceToBeMade)
				yield return new WaitForEndOfFrame();

			//choice is made. execute the paired action.
			string action = actions[ChoiceScreen.lastChoiceMade.index];
			HandleLine(action);

			while (isHandlingLine)
				yield return new WaitForEndOfFrame();
		}
		else
		{
			Debug.LogError("Invalid choice operation. No choices were found.");
		}
	}

	void HandleLine(string rawLine)
	{
		CLM.LINE line = CLM.Interpret(rawLine);

		//now we need to handle the line. This requires a loop full of waiting for input since the line consists of multiple segments that have to be
		//handled individually.
		StopHandlingLine();
		handlingLine = StartCoroutine(HandlingLine(line));
	}

	void StopHandlingLine()
	{
		if (isHandlingLine)
			StopCoroutine(handlingLine);
		handlingLine = null;
	}

	[HideInInspector]
	/// <summary> Used as a fallback when no speaker is given.</summary>
	public string cachedLastSpeaker = "";

	public bool isHandlingLine { get { return handlingLine != null; } }
	Coroutine handlingLine = null;
	IEnumerator HandlingLine(CLM.LINE line)
	{
		//since the "next" trigger controls the flow of a chapter by moving through lines and yet also controls the progression through a line by
		//its segments, it must be reset.
		_next = false;
		_back = false;
		int lineProgress = 0;//progress through the segments of a line.

		while (lineProgress < line.segments.Count)
		{
			_next = false;//reset at the start of each loop.
			_back = false;
			CLM.LINE.SEGMENT segment = line.segments[lineProgress];

			//always run the first segment automatically. But wait for the trigger on all proceding segments.
			if (lineProgress > 0)
			{
				if (segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelay)
				{
					for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
					{
						yield return new WaitForEndOfFrame();
						if (_next)
							break;//allow the termination of a delay when "next" is triggered. Prevents unskippable wait timers.
					}
				}
				else
				{
					while (!_next && !_back)
						yield return new WaitForEndOfFrame();//wait until the player says move to the next segment.
				}
			}
			_next = false;//next could have been triggered during an event above.
			_back = false;
			//the segment now needs to build and run.
			segment.Run();

			while (segment.isRunning)
			{
				yield return new WaitForEndOfFrame();
				//allow for auto completion of the current segment for skipping purposes.
				if (_next)
				{
					//rapidly complete the text on first advance, force it to finish on the second.
					if (!segment.architect.skip)
						segment.architect.skip = true;
					else
						segment.ForceFinish();
					_next = false;
					_back = false;
				}
			}

			lineProgress++;

			yield return new WaitForEndOfFrame();
		}

		//Line is finished. Handle all the actions set at the end of the line.
		for (int i = 0; i < line.actions.Count; i++)
		{
			HandleAction(line.actions[i]);
		}

		handlingLine = null;
	}

	//ACTIONS
	//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void HandleAction(string action)
	{
		//print("execute command - " + action);
		string[] data = action.Split('(', ')');
		switch (data[0])
		{
			case "setBackground":
				Command_SetLayerImage(data[1], BCFC.instance.background);
				break;

			case "setCinematic":
				Command_SetLayerImage(data[1], BCFC.instance.cinematic);
				break;

			case "setForeground":
				Command_SetLayerImage(data[1], BCFC.instance.foreground);
				break;

			case "effect":
				Command_Effect(data[1]);
				break;

			case "getEffect":
				Command_GetEffect(data[1]);
				break;

			case "savePlayerName":
				Command_SavePlayerName(InputScreen.currentInput);//saves the player name as what was last input by the player
				break;

			case "playSound":
				Command_PlaySound(data[1]);
				break;

			case "playMusic":
				Command_PlayMusic(data[1]);
				break;

			case "transBackground":
				Command_TransLayer(BCFC.instance.background, data[1]);
				break;

			case "transCinematic":
				Command_TransLayer(BCFC.instance.cinematic, data[1]);
				break;

			case "transForeground":
				Command_TransLayer(BCFC.instance.foreground, data[1]);
				break;

			case "showScene":
				Command_ShowScene(data[1]);
				break;

			case "Load":
				Command_Load(data[1]);
				break;

			case "next":
				Next();
				break;

			case "saveTempVal":
				Command_SaveTemporaryValue(data[1]);
				break;

			case "saveTempInput"://this takes only the index in the cache to save the value to.
				Command_SaveTemporaryValue(data[1] + "," + InputScreen.currentInput);
				break;

			case "anim":
				Command_Anim(data[1]);
				break;
			case "shake":
				Command_CamShake();
				break;
		}

	}

	void Command_SaveTemporaryValue(string data)
	{
		string[] parts = data.Split(',');
		int val = 1;
		if (!int.TryParse(parts[0], out val))
			val = 1;
		val = Mathf.Clamp(val, 1, 9);

		//чекнути файли орігінала і доробити
		//CACHE.tempVals[val - 1] = parts[1].Replace("~", " ");//since spaces are not allowed in values, a squiggly line represents a space in these values.
	}
	void Command_SavePlayerName(string newName)
	{
		activeGameFile.playerName = newName;
	}
	void Command_StopAmbiance(string ambianceTrackName)
	{
		AudioManager.instance.StopAmbiance(ambianceTrackName);
	}

	void Command_StopAllAmbiance()
	{
		AudioManager.instance.StopAmbiance();
	}
	void Command_PlayAmbiance(string ambianceTrackName)
	{
		//load the track
		AudioClip clip = Resources.Load<AudioClip>("Audio/Ambiance/" + ambianceTrackName);
		if (clip != null)
		{
			AudioManager.instance.PlayAmbiance(clip);
		}
	}
	void Command_Load(string chapterName)
	{
		NovelController.instance.LoadChapterFile(chapterName);
	}

	//TODO:change logic do like player name
	void Command_Effect(string data) 
	{
		string[] parameters = data.Split(',');
		string characterName = parameters[0];				
		if (PlayerPrefs.HasKey(characterName))
		{
			int effect = int.Parse(parameters[1]);
			Debug.Log(effect);
			if (effect < 0)
				PlayerPrefs.SetInt(characterName, PlayerPrefs.GetInt(characterName) - effect *-1);
			else if (effect > 0)
				PlayerPrefs.SetInt(characterName, PlayerPrefs.GetInt(characterName) + effect);
			else
				PlayerPrefs.SetInt(characterName, 0);

			PlayerPrefs.Save();
		}
		else
			Debug.LogError("Can not Save Effect. Character dose not exist");
	}

	void Command_GetEffect(string data)
	{
		if (PlayerPrefs.HasKey(data))
		{
			PlayerPrefs.GetInt(data);

			Debug.Log(data +" getEffect : " + PlayerPrefs.GetInt(data));
		}
		else
			Debug.LogError("Can not Save Effect. Character dose not exist");
	}

	void Command_SetLayerImage(string data, BCFC.LAYER layer)
	{
		Debug.Log("YES.");
		string texName = data.Contains(",") ? data.Split(',')[0] : data;
		Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/backdrops/" + texName) as Texture2D;
		float spd = 2f;
		bool smooth = false;

		if (data.Contains(","))
		{
			string[] parameters = data.Split(',');
			foreach (string p in parameters)
			{
				float fVal = 0;
				bool bVal = false;
				if (float.TryParse(p, out fVal))
				{
					spd = fVal; continue;
				}
				if (bool.TryParse(p, out bVal))
				{
					smooth = bVal; continue;
				}
			}
		}

		layer.TransitionToTexture(tex, spd, smooth);
	}

	void Command_PlaySound(string data)
	{
		AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;

		if (clip != null)
			AudioManager.instance.PlaySFX(clip);
		else
			Debug.LogError("Clip does not exist - " + data);
	}

	void Command_PlayMusic(string data)
	{
		if (data.ToLower() == "null")
		{
			AudioManager.instance.PlaySong(null);
		}
		else
		{
			AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;

			AudioManager.instance.PlaySong(clip);
		}
	}

	void Command_TransLayer(BCFC.LAYER layer, string data)
	{
		string[] parameters = data.Split(',');

		string texName = parameters[0];
		string transTexName = parameters[1];
		Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
		Texture2D transTex = Resources.Load("Images/TransitionEffects/" + transTexName) as Texture2D;

		float spd = 2f;
		bool smooth = false;

		for (int i = 2; i < parameters.Length; i++)
		{
			string p = parameters[i];
			float fVal = 0;
			bool bVal = false;
			if (float.TryParse(p, out fVal))
			{ spd = fVal; continue; }
			if (bool.TryParse(p, out bVal))
			{ smooth = bVal; continue; }
		}

		TransitionMaster.TransitionLayer(layer, tex, transTex, spd, smooth);
	}

	void Command_ShowScene(string data)
	{
		string[] parameters = data.Split(',');
		bool show = bool.Parse(parameters[0]);
		string texName = parameters[1];
		Texture2D transTex = Resources.Load("Images/TransitionEffects/" + texName) as Texture2D;
		float spd = 2f;
		bool smooth = false;
		Debug.Log("SCEEN BLACK");
		for (int i = 2; i < parameters.Length; i++)
		{
			string p = parameters[i];
			float fVal = 0;
			bool bVal = false;
			if (float.TryParse(p, out fVal))
			{ spd = fVal; continue; }
			if (bool.TryParse(p, out bVal))
			{ smooth = bVal; continue; }
		}

		TransitionMaster.ShowScene(show, spd, smooth, transTex);
	}
	static void Command_Anim(string data)
	{
		string[] parameters = data.Split(',');
		Character c = CharacterManager.instance.GetCharacter(parameters[0]);
		string trigger = parameters[1];


	}
	void Command_CamShake()
	{
		animator.SetTrigger("shake");
	}
}
