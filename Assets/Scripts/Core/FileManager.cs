using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManager : MonoBehaviour
{
	/// <summary>
	/// the keys used to encrypt JSON files.
	/// </summary>
	public static byte[] keys = new byte[3] { 23, 70, 194 };

	/// <summary>
	/// The root save path for ALL data
	/// </summary>
	/// <value>The sav path.</value>
	public static string savPath
	{
		get
		{
		#if UNITY_EDITOR
			return "Assets/";
		#else
			return Application.persistentDataPath + "/";
		#endif
		}
	}

	/// <summary>
	/// The save path for all data generated at runtime.
	/// </summary>
	/// <value>The data path.</value>
	public static string dataPath
	{
		get
		{
			return savPath + "data/";
		}
	}

	public static string fileExtension = ".txt";

	/// <summary>
	/// Separates the filename from the containing directory and returns the directory path.
	/// </summary>
	/// <returns>The directory from path.</returns>
	/// <param name="filePath">File path.</param>
	public static string GetDirectoryFromPath(string filePath)
	{
		string directoryPath = "";
		string[] parts = filePath.Split('/');
		foreach (string part in parts)
		{
			if (!part.Contains("."))
				directoryPath += part + "/";
		}
		return directoryPath;
	}

	/// <summary>
	/// Will return true if the directory already exists or was created. Returns false if the directory could not be made.
	/// </summary>
	/// <returns><c>true</c>, if create directory from path was tryed, <c>false</c> otherwise.</returns>
	/// <param name="path">Path.</param>
	public static bool TryCreateDirectoryFromPath(string path)
	{
		string directoryPath = path;

		if (Directory.Exists(path) || File.Exists(path)) return true;
		if (path.Contains("."))
		{
			directoryPath = GetDirectoryFromPath(path);
			if (Directory.Exists(directoryPath)) return true;
		}

		if (directoryPath != "" && !directoryPath.Contains("."))
		{
			print(directoryPath);
			try
			{
				Directory.CreateDirectory(directoryPath);
				return true;
			}
			catch (System.Exception e)
			{
				Debug.LogError("Could not create Directory!\nERROR DETAILS: " + e.ToString());
				return false;
			}
		}
		else
		{
			Debug.LogError("Directory was invalid - " + directoryPath + "\npath=" + path + "\ndirectoryPath=" + directoryPath);
			return false;
		}
	}

	/// <summary>
	/// Takes a file path and injects the default path if needed, and appends the default extension if needed.
	/// </summary>
	/// <returns>The correct file path.</returns>
	/// <param name="filePath">File path.</param>
	public static string AttemptCorrectFilePath(string filePath)
	{
		//make sure we add the default save path if desired.
		filePath = filePath.Replace("[]", dataPath);
		//add the default extension if no extension is present.
		if (!filePath.Contains(".")) filePath += fileExtension;

		return filePath;
	}

	public static void SaveFile(string filePath, string line)
	{
		SaveFile(filePath, new List<string>() { line });
	}

	/// <summary>
	/// Save a file with the specified lines	
	/// </summary>
	public static void SaveFile(string filePath, List<string> lines)
	{
		filePath = AttemptCorrectFilePath(filePath);

		//If the directory does not exist, try to create it. Prevent continuation if path was not valid.
		if (!TryCreateDirectoryFromPath(filePath))
		{
			Debug.LogError("FAILED TO SAVE FILE [" + filePath + "] Please see console/log for details.");
			return;
		}

		StreamWriter sw = new StreamWriter(filePath);
		int i = 0;
		for (i = 0; i < lines.Count; i++)
		{
			sw.WriteLine(lines[i]);
		}

		sw.Close();

		print("Saved " + i.ToString() + " lines to file [" + filePath + "]");
	}

	/// <summary>
	/// Converts an array of strings into a list of the same values.
	/// </summary>
	/// <returns>The to list.</returns>
	/// <param name="array">Array.</param>
	/// <param name="removeBlankLines">If set to <c>true</c> remove blank lines.</param>
	public static List<string> ArrayToList(string[] array, bool removeBlankLines = true)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++)
		{
			string s = array[i];
			if (s.Length > 0 || !removeBlankLines)
			{
				list.Add(s);
			}
		}
		return list;
	}

	/// <summary>
	/// Reads the data from the file at this path and returns a list of lines.
	/// </summary>
	/// <returns>The of file.</returns>
	/// <param name="filePath">File path.</param>
	public static List<string> LoadFile(string filePath, bool removeBlankLines = true)
	{
		filePath = AttemptCorrectFilePath(filePath);

		if (File.Exists(filePath))
		{
			List<string> lines = ArrayToList(File.ReadAllLines(filePath), removeBlankLines);
			return lines;
		}
		else
		{
			string errorMessage = "ERR! File " + filePath + " does not exist!";
			Debug.LogError(errorMessage);
			return new List<string>() { errorMessage };
		}
	}

	/// <summary>
	/// Read a text asset and return a list of lines
	/// </summary>
	/// <returns>The text asset.</returns>
	/// <param name="txt">Text.</param>
	public static List<string> ReadTextAsset(TextAsset txt)
	{
		string[] lines = txt.text.Split('\n', '\r');

		return ArrayToList(lines);
	}

	/// <summary>
	/// Takes a class and saves every public variable in that class regardless of whether it is serializable or not. 
	/// Allows for saving of non serializable variables such as colors, vectors, quaternions, sprites, textures, audio clips, etc. 
	/// Saves anything by converting it into a string that JSON can read at a later time.
	/// </summary>
	/// <param name="filePath">File path.</param>
	/// <param name="serializableClassToSave">Serializable class to save.</param>
	public static void SaveJSON(string filePath, object classToSave)
	{
		string jsonString = JsonUtility.ToJson(classToSave);

		SaveFile(filePath, jsonString);
	}

	public static void SaveEncryptedJSON(string filePath, object classToSave, byte[] encryptionKeys)
	{
		string jsonString = JsonUtility.ToJson(classToSave);

		//encrypt the string before saving to file
		byte[] stringBytes = ConvertToBytes(jsonString);
		//encrypt the bytes
		XOR(ref stringBytes, encryptionKeys);

		SaveComposingBytes(filePath, stringBytes);
	}

	/// <summary>
	/// Save a file by writing it with the bytes that compose the actual file instead of just what is stored in it as done with SaveBytes/SaveEncryptedBytes. Can be used to save any file type.
	/// </summary>
	/// <param name="filePath">File path.</param>
	/// <param name="bytes">Bytes.</param>
	public static void SaveComposingBytes(string filePath, byte[] bytes)
	{
		filePath = AttemptCorrectFilePath(filePath);

		//If the directory does not exist, try to create it. Prevent continuation if path was not valid.
		if (!TryCreateDirectoryFromPath(filePath))
		{
			Debug.LogError("FAILED TO SAVE FILE [" + filePath + "] Please see console/log for details.");
			return;
		}

		//otherwise the directory doesnt exist and we can't write a file.
		File.WriteAllBytes(filePath, bytes);

		print("Saved file '" + filePath + "' with " + bytes.Length + " bytes.");
	}


	/// <summary>
	/// Load a class from a JSON file by converting every string to its proper value. 
	/// Loads non serializable objects such as colors, vectors, quaternions, sprites, textures, audio clips, etc.
	/// </summary>
	/// <returns>The JSO.</returns>
	/// <param name="filePath">File path.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T LoadJSON<T>(string filePath)
	{
		string jsonString = LoadFile(filePath)[0];

		return JsonUtility.FromJson<T>(jsonString);
	}

	public static T LoadEncryptedJSON<T>(string filePath, byte[] encryptionKeys)
	{
		byte[] stringBytes = LoadComposingBytes(filePath);

		//decrypt the string before attempting to convert it
		XOR(ref stringBytes, encryptionKeys);
		string jsonString = BuildObjectFromBytes(stringBytes) as string;

		return JsonUtility.FromJson<T>(jsonString);
	}

	/// <summary>
	/// Loads the bytes that compose the actual file, not the bytes stored in the file as done with LoadBytes/LoadEncryptedBytes. Can be used to load any file type.
	/// </summary>
	/// <returns>The bytes only.</returns>
	/// <param name="filePath">File path.</param>
	public static byte[] LoadComposingBytes(string filePath)
	{
		filePath = AttemptCorrectFilePath(filePath);

		byte[] data = File.ReadAllBytes(filePath);

		return data;
	}

	/// <summary>
	/// Convert an array of objects into an array of bytes. The objects must be serializable.
	/// </summary>
	/// <returns>The to bytes.</returns>
	/// <param name="objects">Objects.</param>
	public static byte[] ConvertToBytes(string str)
	{
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();

		bf.Serialize(ms, str);
		return ms.ToArray();
	}

	/// <summary>
	/// Takes an array of bytes and reconstructs the data in its original form. If there was only one object in the array before conversion, that one object is returned. If there were many, then an array of all reconstructed objects is returned.
	/// </summary>
	/// <returns>The object from bytes.</returns>
	/// <param name="bytes">Bytes.</param>
	public static object BuildObjectFromBytes(byte[] bytes)
	{
		MemoryStream ms = new MemoryStream(bytes);
		BinaryFormatter bf = new BinaryFormatter();

		object ob = bf.Deserialize(ms);

		//try to get it as an array. if it works then this is an array of one or more objects.
		object[] arr = ob as object[];
		if (arr != null)
		{
			if (arr.Length == 1)
				return arr[0];//return the only item in the array if this was just one object saved instead of many.
		}

		return ob;
	}

	/// <summary>
	/// XORs an array of bytes to encrypt or decrypt it through the use of cycling keys. 
	/// </summary>
	/// <param name="bytes">Bytes.</param>
	/// <param name="keys">Keys.</param>
	public static void XOR(ref byte[] bytes, params byte[] keys)
	{
		byte[] bytesarr = new byte[bytes.Length];
		int curKeyIndex = 0;

		for (int i = 0; i < bytes.Length; i++)
		{
			byte key = keys[curKeyIndex];
			bytesarr[i] = (byte)(bytes[i] ^ key);
			curKeyIndex = curKeyIndex == keys.Length - 1 ? 0 : curKeyIndex + 1;
		}

		bytes = bytesarr;
	}
}