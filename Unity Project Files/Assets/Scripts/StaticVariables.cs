using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StaticVariables : MonoBehaviour
{
    public AudioSource source;
    public Animator anim;
    public string animationName;
    public UIScript ui_Script;
    [SerializeField]
    private ScriptableObjectTest notes;
    [SerializeField]
    private ScriptableObjectTest patterns;
    [SerializeField]
    private OtherSettingsSO otherSettings;
    public GameObject loadingBarGameObject;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public bool runningCoroutine;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(Load(index));
    }

    IEnumerator Load(int ind)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(ind);
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            loadingBarGameObject.SetActive(true);
            loadingText.enabled = true;
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                loadingBar.value = progress;
                loadingText.text = (progress * 100).ToString() + "%";
                yield return null;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator RefreshNotes(List<bool> values, List<Toggle> noteToggles, RythymManager rythymManager, List<bool> patternValues, List<Toggle> patternToggles, bool start)
    {
        runningCoroutine = true;
        /*if(lastUsedNotes.Length == 0)
        {
            for(int i = 0; i < 19; i++)
            {
                //lastUsedNotes.Add(false);
            }
        }
        if(lastUsedPatterns.Length == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                //lastUsedPatterns.Add(false);
            }
        }*/
        if (start)
        {
            int j = 0;
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = notes.Values[i];
                noteToggles[i].isOn = notes.Values[i];
                j += notes.Values[i] ? 1 : 0;
                rythymManager.allSymbols[i].canBeUsed = notes.Values[i];
            }
            for (int i = 0; i < patternValues.Count; i++)
            {
                patternValues[i] = patterns.Values[i];
                patternToggles[i].isOn = patterns.Values[i];
                j += patterns.Values[i] ? 1 : 0;
                rythymManager.allPatterns[i].canBeUsed = patterns.Values[i];
            }
            if (j == 0)
            {
                noteToggles[0].isOn = true;
                values[0] = true;
                notes.Values[0] = true;
                rythymManager.allSymbols[0].canBeUsed = notes.Values[0];
            }
            rythymManager.condenseMeasures = otherSettings.CondenseMeasures;
            rythymManager.playMetWithRythym = otherSettings.PlayMetWithRythym;
            yield return null;
        }
        else
        {
            int j = 0;
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = noteToggles[i].isOn;
                notes.Values[i] = noteToggles[i].isOn;
                j += noteToggles[i].isOn ? 1 : 0;
                rythymManager.allSymbols[i].canBeUsed = notes.Values[i];
            }
            for (int i = 0; i < patternValues.Count; i++)
            {
                patternValues[i] = patternToggles[i].isOn;
                patterns.Values[i] = patternToggles[i].isOn;
                j += patternToggles[i].isOn ? 1 : 0;
                rythymManager.allPatterns[i].canBeUsed = patterns.Values[i];
            }
            if (j == 0)
            {
                noteToggles[0].isOn = true;
                values[0] = true;
                notes.Values[0] = true;
                rythymManager.allSymbols[0].canBeUsed = notes.Values[0];
            }
            yield return null;
        }
        runningCoroutine = false;
    }
}
