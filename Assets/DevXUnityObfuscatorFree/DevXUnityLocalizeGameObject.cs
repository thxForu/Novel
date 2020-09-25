using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene objects localization class
/// </summary>
//[System.Reflection.ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
public class DevXUnityLocalizeGameObject : MonoBehaviour
{
    private int textKey;

    private TextMesh _textMesh;
    private UnityEngine.UI.Text _uiTest;
  
    void Start()
    {
        // Init action
        _textMesh = GetComponent<TextMesh>();
        if (_textMesh != null)
        {
            textKey=DevXUnity.GetLocalizationKey(_textMesh.text);
            if (string.IsNullOrEmpty(_textMesh.text) == false)
            {
                DevXUnity.AddToChangeLang(OnChangeLanguage);
                OnChangeLanguage();
                return;
            }
        }

        _uiTest = GetComponent<UnityEngine.UI.Text>();
        if (_uiTest != null)
        {
            textKey=DevXUnity.GetLocalizationKey(_uiTest.text);
            if (string.IsNullOrEmpty(_uiTest.text) == false)
            {
                DevXUnity.AddToChangeLang(OnChangeLanguage);
                OnChangeLanguage();

                return;
            }
        }
    }

    void OnDestroy()
    {
        // Remove action
        DevXUnity.RemoveFromChangeLang(OnChangeLanguage);
    }

    /// <summary>
    /// Action On Change Language
    /// </summary>
    void OnChangeLanguage()
    {
        // Localize to current lang
        string s=DevXUnity.GetLocalizedText(textKey);
        if (s != null)
        {
            if(_textMesh!=null) _textMesh.text = s;
            if(_uiTest!=null) _uiTest.text = s;
        }
       
    }
}
