using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public GameObject mainPanel;
    public GameObject loadGamePanel;
    public GameObject settingsPanel;
    public GameObject aboutUsPanel;

    public GameSavePanel saveLoadPanel;

    void Awake()
    {
        instance = this;
    }
    int currentSaveLoadPage
    {
        get { return saveLoadPanel.currentSaveLoadPage; }
    }

    public string selectGameFile = "";
    // Start is called before the first frame update
    void Start()
    {
        CloseLoadGamePanel();
        loadGamePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        aboutUsPanel.gameObject.SetActive(false);
    }
    public void StartNewGame()
    {
        selectGameFile = "new file";
        //save the name of the file that we will be loading in the visual novel. 
        FileManager.SaveFile(FileManager.savPath + "savData/file", selectGameFile);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Novel");
    }
    public void ClickLoadGame()
    {
        mainPanel.SetActive(false);
        loadGamePanel.gameObject.SetActive(true);

        //populate the saveLoad panel
        saveLoadPanel.LoadFilesOntoScreen(currentSaveLoadPage);

        //if(settingsPanel.gameObject.activeInHierarchy)  
    }
    public void ClickOpenSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void Continue()
    {

    }
    public void Patreon()
    {
        Application.OpenURL("https://www.patreon.com");
    }
    public void CloseAndOpenMenu()
    {
        aboutUsPanel.gameObject.SetActive(false);
        mainPanel.SetActive(true);
        
    }
    public void CloseLoadGamePanel()
    {
        loadGamePanel.gameObject.SetActive(false);
    }
}
