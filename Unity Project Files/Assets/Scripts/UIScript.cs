using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIScript : MonoBehaviour
{

    public EventSystem system;
    public RythymManager rythymManager;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public Slider tempoSlider;
    public TextMeshProUGUI tempoText;
    public Toggle swingToggle;
    public Toggle condenseToggle;
    public Toggle playMetToggle;
    public TMP_Dropdown timeSignatureDropDown;
    public TMP_Dropdown totalMeasuresDropDown;
    public Button optionsButton;
    public Button metronomeButton;
    public Button playRythymButton;
    public Button createRythymButton;
    public List<Toggle> accents;
    public int accentID;
    public List<GameObject> accentObjects;
    public GameObject accentParent;
    public bool accentPlaceMode;
    public deletechildrenScript deletechildrenScript;
    public List<Toggle> noteToggles;
    public List<Toggle> patternToggles;
    public Scrollbar scrollbar;
    public List<bool> values;
    public List<bool> patternValues;
    public float scrollOffset;
    public StaticVariables variables;
    public OtherSettingsSO otherSettings;
    public List<Quaternion> notMouseClickPositions;

    void Start()
    {
        tempoChange(otherSettings.Tempo / 10);
        setSwing(otherSettings.Swing);
        setTimeSignature(otherSettings.TimeSignatureDropDown);
        setTotalMeasures(otherSettings.TotalMeasuresDropDown);
        UpdateNotesMethod(true);
    }

    Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    public void turnAccentToggleOnOrOff(int item)
    {
        bool value = accents[item].isOn;
        if (value)
        {
            for (int i = 0; i < accents.Count; i++)
            {
                accents[i].interactable = item == i ? true : false;
            }
            optionsButton.interactable = false;
            metronomeButton.interactable = false;
            playRythymButton.interactable = false;
            createRythymButton.interactable = false;
            accentID = item;
            accentPlaceMode = true;
            ColorBlock colorBlock = accents[item].colors;
            colorBlock.normalColor = new Color(0, 255, 0);
            colorBlock.highlightedColor = new Color(0, 255, 0);
            accents[item].colors = colorBlock;
        }
        else
        {
            for (int i = 0; i < accents.Count; i++)
            {
                accents[i].interactable = true;
            }
            optionsButton.interactable = true;
            metronomeButton.interactable = true;
            playRythymButton.interactable = true;
            createRythymButton.interactable = true;
            accentPlaceMode = false;
            ColorBlock colorBlock = accents[item].colors;
            colorBlock.normalColor = new Color(255, 255, 255);
            colorBlock.highlightedColor = new Color(255, 255, 255);
            accents[item].colors = colorBlock;
        }
    }



    public void openOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        scrollbar.gameObject.SetActive(true);
    }

    public void closeOptionsMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        scrollbar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        system.SetSelectedGameObject(null);
        if (Input.GetMouseButtonDown(0))
        {
            bool canClick = true;
            for (int i = 0; i < notMouseClickPositions.Count; i++)
            {
                if (GetMousePos().x > notMouseClickPositions[i].x && GetMousePos().x < notMouseClickPositions[i].y && GetMousePos().y > notMouseClickPositions[i].z && GetMousePos().y < notMouseClickPositions[i].w)
                {
                    canClick = false;
                }
            }
            if (accentPlaceMode && canClick)
            {
                GameObject accent = Instantiate(accentObjects[accentID], accentParent.transform, false);
                accent.transform.localPosition = GetMousePos();
                accent.transform.localScale /= rythymManager.totalMeasures > 1 ? 2 : 1;
                deletechildrenScript.children.Add(accent);
            }
        }
        if (optionsMenu.activeInHierarchy)
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                scrollbar.value = Mathf.Clamp01(scrollbar.value - 0.1f);
            }
            else if(Input.mouseScrollDelta.y < 0)
            {
                scrollbar.value = Mathf.Clamp01(scrollbar.value + 0.1f);
            }
        }
        
    }

    public void scroll(float value)
    {
        optionsMenu.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, (value * 1000) - scrollOffset, 0);
    }

    public void tempoChange(float tempo)
    {
        otherSettings.Tempo = Mathf.RoundToInt(tempo) * 10;
        tempoSlider.value = otherSettings.Tempo / 10;
        rythymManager.tempo = otherSettings.Tempo;
        tempoText.text = "" + otherSettings.Tempo;
    }

    public void PercentChange(int amount)
    {
        rythymManager.patternPercentRatio = amount / 2;
    }

    public void setSwing(bool swing)
    {
        otherSettings.Swing = swing;
        rythymManager.swing = otherSettings.Swing;
        swingToggle.isOn = otherSettings.Swing;
    }
    public void setCondenseMeasures(bool condense)
    {
        otherSettings.CondenseMeasures = condense;
        rythymManager.condenseMeasures = otherSettings.CondenseMeasures;
        condenseToggle.isOn = otherSettings.CondenseMeasures;
    }
    public void setPlayMetWithRythym(bool play)
    {
        otherSettings.PlayMetWithRythym = play;
        rythymManager.playMetWithRythym = otherSettings.PlayMetWithRythym;
        playMetToggle.isOn = otherSettings.PlayMetWithRythym;
    }

    public void setTimeSignature(int value)
    {
        switch (value)
        {
            case 0:
                otherSettings.TimeSignatureDropDown = 0;
                rythymManager.currentTimeSignatureID = otherSettings.TimeSignatureDropDown;
                timeSignatureDropDown.value = otherSettings.TimeSignatureDropDown;
                break;
            case 1:
                otherSettings.TimeSignatureDropDown = 1;
                rythymManager.currentTimeSignatureID = otherSettings.TimeSignatureDropDown;
                timeSignatureDropDown.value = otherSettings.TimeSignatureDropDown;
                break;
            case 2:
                otherSettings.TimeSignatureDropDown = 2;
                rythymManager.currentTimeSignatureID = otherSettings.TimeSignatureDropDown;
                timeSignatureDropDown.value = otherSettings.TimeSignatureDropDown;
                break;
        }
        rythymManager.createNewRythym = true;
    }

    public void setTotalMeasures(int value)
    {
        switch (value)
        {
            case 0:
                otherSettings.TotalMeasuresDropDown = 0;
                rythymManager.totalMeasures = 1;
                totalMeasuresDropDown.value = otherSettings.TotalMeasuresDropDown;
                break;
            case 1:
                otherSettings.TotalMeasuresDropDown = 1;
                rythymManager.totalMeasures = 2;
                totalMeasuresDropDown.value = otherSettings.TotalMeasuresDropDown;
                break;
            case 2:
                otherSettings.TotalMeasuresDropDown = 2;
                rythymManager.totalMeasures = 4;
                totalMeasuresDropDown.value = otherSettings.TotalMeasuresDropDown;
                break;
        }
        rythymManager.createNewRythym = true;
    }

    public void Metronome()
    {
        if (rythymManager.metronomeOn)
        {
            rythymManager.metronomeOn = false;
            metronomeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Metronome On";
            optionsButton.interactable = true;
            playRythymButton.interactable = true;
            createRythymButton.interactable = true;
        }
        else
        {
            rythymManager.metronomeOn = true;
            metronomeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Metronome Off";
            optionsButton.interactable = false;
            playRythymButton.interactable = false;
            createRythymButton.interactable = false;
        }
    }

    public void PlayRythym()
    {
        if (rythymManager.playBackRythym)
        {
            rythymManager.startPlaybackFunction(false);
            playRythymButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Playback";
            optionsButton.interactable = true;
            metronomeButton.interactable = true;
            createRythymButton.interactable = true;
        }
        else
        {
            rythymManager.startPlaybackFunction(true);
            playRythymButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Playback";
            optionsButton.interactable = false;
            metronomeButton.interactable = false;
            createRythymButton.interactable = false;
        }
    }

    public void clickPlayButton()
    {
        rythymManager.clickedPlayButton();
    }

    public void UpdateNotesMethod(bool START)
    {
        if (!variables.runningCoroutine)
        {
            variables.StartCoroutine(variables.RefreshNotes(values, noteToggles, rythymManager, patternValues, patternToggles, START));
        }
    }

    public IEnumerator UpdateNotes()
    {
        int j = 0;
        for (int i = 0; i < values.Count; i++)
        {
            values[i] = noteToggles[i].isOn;
            j += noteToggles[i].isOn ? 1 : 0;
            rythymManager.allSymbols[i].canBeUsed = values[i];
        }
        for (int i = 0; i < patternValues.Count; i++)
        {
            patternValues[i] = patternToggles[i].isOn;
            j += patternToggles[i].isOn ? 1 : 0;
            rythymManager.allPatterns[i].canBeUsed = patternValues[i];
        }
        if(j == 0)
        {
            noteToggles[0].isOn = true;
            values[0] = true;
            rythymManager.allSymbols[0].canBeUsed = values[0];
        }
        yield return null;
    }
}
