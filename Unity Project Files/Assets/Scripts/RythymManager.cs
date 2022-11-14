using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Symbol
{
    public string name;
    public SymbolType symbolType;
    public float symbolLength;
    public GameObject symbolPrefab;
    public float beatSymbolIsOn;
    public bool canBeUsed;
    public bool isPattern;
    public string patternName;

    public Symbol(Symbol symbol)
    {
        name = symbol.name;
        symbolType = symbol.symbolType;
        symbolLength = symbol.symbolLength;
        symbolPrefab = symbol.symbolPrefab;
        beatSymbolIsOn = symbol.beatSymbolIsOn;
        canBeUsed = symbol.canBeUsed;
        isPattern = symbol.isPattern;
        patternName = symbol.patternName;
    }
}

[System.Serializable]
public class SymbolPattern
{
    public string name;
    public Symbol[] symbols;
    public float patternLength;
    public bool canBeUsed;
}

public enum SymbolType
{
    Note,
    Rest
}

[System.Serializable]
public class TimeSignature
{
    public int numerator;
    public int denominator;
}

[System.Serializable]
public class RythymSounds
{
    public AudioClip clip;
    public float length;
    public bool isPattern;
    public string patternName;
}

[System.Serializable]
public class ClipList
{
    public AudioClip[] clips;
}

public class RythymManager : MonoBehaviour
{
    public int totalMeasures;
    public TimeSignature currentTimeSignature;
    public int currentTimeSignatureID;
    public float tempo;
    public bool swing;
    public bool startMetronomeWhenPlaying;
    public float beatsInAMesure;
    public bool metronomeOn;
    bool startedMetronome;
    public AudioSource audioSource;
    public AudioSource rythymAudioSource;
    public AudioClip metronomeSound;
    public List<RythymSounds> noteSounds;
    public List<Symbol> allSymbols;
    public List<SymbolPattern> allPatterns;
    public List<string> notSwingPatternIDs;
    public List<TimeSignature> allTimeSignatures;
    public MeasureHolderScript[] measureHolders;
    public bool createNewRythym;
    public bool playBackRythym;
    public bool startedPlayback;
    public bool countOff;
    public bool staticCountOff;
    public int clicksForCountoff;
    public int clicksIntoCountoff;
    bool doneWithClearing;
    public float noteTimer;
    public UIScript UI;
    [SerializeField]
    public List<ClipList> tempoClipLists;
    public float patternPercentRatio;
    public bool condenseMeasures;
    public bool playMetWithRythym;

    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator GenerateNewRythym()
    {
        doneWithClearing = false;
        while (!doneWithClearing)
        {
            foreach(MeasureHolderScript measureHolderScript in measureHolders)
            {
                foreach(MeasureScript measureScript in measureHolderScript.measureScripts)
                {
                    measureScript.symbols.Clear();
                    measureScript.symbols = new List<Symbol>();
                    foreach(Transform trans in measureScript.childrenNotes)
                    {
                        if(trans != null)
                        {
                            Destroy(trans.gameObject);
                        }
                    }
                }
            }
            doneWithClearing = true;
            yield return null;
        }
        float denominatorAmount = 4 / currentTimeSignature.denominator;
        beatsInAMesure = currentTimeSignature.numerator * denominatorAmount;
        StartCoroutine(CreateMeasure(beatsInAMesure));
    }

    IEnumerator CreateMeasure(float beatsInMeasure)
    {
        int currentMeasureGenerating = 0;
        while(currentMeasureGenerating < totalMeasures)
        {
            float beatsIntoMeasure = 0;
            while(beatsIntoMeasure < beatsInMeasure)
            {
                int i = Random.Range(1, 50);
                int test = i <= patternPercentRatio && allPatterns.Count > 0 ? 1 : 0;
                if(test > 0)
                {
                    SymbolPattern pattern = allPatterns[Random.Range(0, allPatterns.Count)];
                    if (beatsIntoMeasure + pattern.patternLength <= beatsInMeasure && pattern.canBeUsed)
                    {
                        switch (totalMeasures)
                        {
                            case 1:
                                for (int j = 0; j < pattern.symbols.Length; j++)
                                {
                                    measureHolders[0].measureScripts[currentMeasureGenerating].symbols.Add(pattern.symbols[j]);
                                }
                                break;
                            case 2:
                                for (int j = 0; j < pattern.symbols.Length; j++)
                                {
                                    measureHolders[1].measureScripts[currentMeasureGenerating].symbols.Add(pattern.symbols[j]);
                                }
                                break;
                            case 4:
                                for (int j = 0; j < pattern.symbols.Length; j++)
                                {
                                    measureHolders[2].measureScripts[currentMeasureGenerating].symbols.Add(pattern.symbols[j]);
                                }
                                break;
                        }
                        beatsIntoMeasure += pattern.patternLength;
                    }
                }
                else
                {
                    Symbol symbol = allSymbols[Random.Range(0, allSymbols.Count)];
                    if (beatsIntoMeasure + symbol.symbolLength <= beatsInMeasure && symbol.canBeUsed)
                    {
                        switch (totalMeasures)
                        {
                            case 1:
                                measureHolders[0].measureScripts[currentMeasureGenerating].symbols.Add(symbol);
                                break;
                            case 2:
                                measureHolders[1].measureScripts[currentMeasureGenerating].symbols.Add(symbol);
                                break;
                            case 4:
                                measureHolders[2].measureScripts[currentMeasureGenerating].symbols.Add(symbol);
                                break;
                        }
                        beatsIntoMeasure += symbol.symbolLength;
                    }
                }
                yield return null;
            }
            currentMeasureGenerating++;
        }
        switch (totalMeasures)
        {
            case 1:
                foreach(MeasureScript measureScript in measureHolders[0].measureScripts)
                {
                    measureScript.CondenseMeasure();
                }
                break;
            case 2:
                foreach(MeasureScript measureScript in measureHolders[1].measureScripts)
                {
                    measureScript.CondenseMeasure();
                }
                break;
            case 4:
                foreach (MeasureScript measureScript in measureHolders[2].measureScripts)
                {
                    measureScript.CondenseMeasure();
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tempo >= 120)
        {
            //rythymAudioSource.pitch = tempo / 120 == 0 ? 1 : tempo / 120;
        }
        else
        {
            //rythymAudioSource.pitch = 2 - (120 / tempo);
        }
        if (metronomeOn && !startedMetronome)
        {
            StartCoroutine(Metronome());
            startedMetronome = true;
        }
        currentTimeSignature = allTimeSignatures[currentTimeSignatureID];
        switch (totalMeasures)
        {
            case 1:
                measureHolders[0].gameObject.SetActive(true);
                measureHolders[1].gameObject.SetActive(false);
                measureHolders[2].gameObject.SetActive(false);
                break;
            case 2:
                measureHolders[0].gameObject.SetActive(false);
                measureHolders[1].gameObject.SetActive(true);
                measureHolders[2].gameObject.SetActive(false);
                break;
            case 4:
                measureHolders[0].gameObject.SetActive(false);
                measureHolders[1].gameObject.SetActive(false);
                measureHolders[2].gameObject.SetActive(true);
                break;
        }
        switch (currentTimeSignatureID)
        {
            case 0:
                foreach(MeasureHolderScript measureHolder in measureHolders)
                {
                    foreach(MeasureScript measure in measureHolder.measureScripts)
                    {
                        measure.timeSigs[0].SetActive(true);
                        measure.timeSigs[1].SetActive(false);
                        measure.timeSigs[2].SetActive(false);
                    }
                }
                break;
            case 1:
                foreach (MeasureHolderScript measureHolder in measureHolders)
                {
                    foreach (MeasureScript measure in measureHolder.measureScripts)
                    {
                        measure.timeSigs[0].SetActive(false);
                        measure.timeSigs[1].SetActive(true);
                        measure.timeSigs[2].SetActive(false);
                    }
                }
                break;
            case 2:
                foreach (MeasureHolderScript measureHolder in measureHolders)
                {
                    foreach (MeasureScript measure in measureHolder.measureScripts)
                    {
                        measure.timeSigs[0].SetActive(false);
                        measure.timeSigs[1].SetActive(false);
                        measure.timeSigs[2].SetActive(true);
                    }
                }
                break;
        }
        if (createNewRythym)
        {
            StartCoroutine(GenerateNewRythym());
            createNewRythym = false;
        }
    }

    public void clickedPlayButton()
    {
        createNewRythym = true;
    }

    public void startPlaybackFunction(bool play)
    {
        playBackRythym = play;
        if (playBackRythym && !startedPlayback)
        {
            if (1 > 2)
            {
                metronomeOn = true;
                StartCoroutine(CountOffTimer());
            }
            else
            {
                StartCoroutine(PlayRythym());
                startedPlayback = true;
            }
            
        }
        else
        {
            metronomeOn = false;
            countOff = staticCountOff;
            StopCoroutine(CountOffTimer());
            StopCoroutine(Metronome());
        }
    }

    IEnumerator CountOffTimer()
    {
        float time = (1 / (tempo / 60 == 0 ? 1 : tempo / 60)) * clicksForCountoff;
        yield return new WaitForSeconds(time);
        if (!metronomeOn)
        {

        }
        else if (clicksIntoCountoff < 3 +clicksForCountoff)
        {

        }
        else
        {
            metronomeOn = false;
            countOff = false;
            startPlaybackFunction(true);
        }
    }

    IEnumerator Metronome()
    {
        clicksIntoCountoff = 0;
        float timer = (1 / (tempo / 60 == 0 ? 1 : tempo / 60)) + 1;
        while (metronomeOn)
        {
            if(timer > (1 / (tempo / 60 == 0 ? 1 : tempo / 60)))
            {
                clicksIntoCountoff++;
                audioSource.time = Mathf.Min(timer - (1 / (tempo / 60 == 0 ? 1 : tempo / 60)), metronomeSound.length - 0.001f);
                audioSource.PlayOneShot(metronomeSound);
                timer -= (1 / (tempo / 60 == 0 ? 1 : tempo / 60));
            }
            timer += Time.deltaTime;
            yield return null;
        }
        startedMetronome = false;
    }

    public void getMeasureSymbols()
    {
        switch (totalMeasures)
        {
            case 1:
                measureHolders[0].gameObject.SetActive(true);
                break;
            case 2:
                measureHolders[0].gameObject.SetActive(false);
                measureHolders[1].gameObject.SetActive(true);
                measureHolders[2].gameObject.SetActive(false);
                break;
            case 4:
                measureHolders[0].gameObject.SetActive(false);
                measureHolders[1].gameObject.SetActive(false);
                measureHolders[2].gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator singleMetClick()
    {
        while (true)
        {
            rythymAudioSource.PlayOneShot(metronomeSound);
            yield return new WaitForSeconds(60 / tempo);
        }
    }

    IEnumerator PlayRythym()
    {
        bool playing = true;
        bool startedMet = false;
        float timer = 0;
        int index = 0;
        IEnumerator routine = singleMetClick();
        if (playMetWithRythym)
        {
            StartCoroutine(routine);
        }
        AudioClip clip = metronomeSound;
        List<List<playedNote>> beatSymbols = new List<List<playedNote>>();
        List<playedNote> beatsInOrder = new List<playedNote>();
        while (playBackRythym)
        {
            switch (totalMeasures)
            {
                case 1:
                    beatSymbols = measureHolders[0].measureBeatsLists;
                    break;
                case 2:
                    beatSymbols = measureHolders[1].measureBeatsLists;
                    break;
                case 4:
                    beatSymbols = measureHolders[2].measureBeatsLists;
                    break;
            }
            for (int i = 0; i < beatSymbols.Count; i++)
            {
                List<playedNote> list = beatSymbols[i];
                for (int j = 0; j < list.Count; j++)
                {
                    float number = list[j].beat;
                    SymbolType TYPE = list[j].type;
                    float LENGTH = list[j].length;
                    bool pattern = list[j].isPattern;
                    string patName = list[j].patternName;
                    number = number + (i * beatsInAMesure);
                    beatsInOrder.Add(new playedNote(number, false, TYPE, LENGTH, pattern, patName));
                }
            }
            for (int i = 0; i < beatsInOrder.Count; i++)
            {
                if ((beatsInOrder[i].beat / (float)0.5) % 2 == 1 && swing && !notSwingPatternIDs.Contains(beatsInOrder[i].patternName))
                {
                    beatsInOrder[i].beat += (float)0.1666;
                }
                beatsInOrder[i].beat -= 1;
                beatsInOrder[i].beat *= (1 / (tempo / 60 == 0 ? 1 : tempo / 60));
            }
            if (startMetronomeWhenPlaying && !startedMet)
            {
                metronomeOn = true;
                startedMet = true;
            }
            while (playing)
            {
                if (!playBackRythym)
                {
                    metronomeOn = false;
                    rythymAudioSource.Stop();
                    if(playMetWithRythym)
                    {
                        StopCoroutine(routine);
                    }
                    playing = false;
                    startedPlayback = false;
                    playBackRythym = true;
                    UI.PlayRythym();
                }
                for (int i = 0; i < beatsInOrder.Count; i++)
                {
                    if (timer >= beatsInOrder[i].beat && !beatsInOrder[i].playedAlready && beatsInOrder[i].type == SymbolType.Note)
                    {
                        index = i;
                        foreach (RythymSounds rythym in noteSounds)
                        {
                            if (rythym.isPattern && beatsInOrder[index].isPattern && rythym.patternName == beatsInOrder[index].patternName)
                            {
                                foreach(AudioClip audioClip in tempoClipLists[((int)tempo / 10) - 1].clips)
                                {
                                    if(audioClip.name == rythym.clip.name + " (" + tempo + ")")
                                    {
                                        clip = audioClip;
                                    }
                                }
                            }
                            else if (rythym.length == beatsInOrder[index].length && !rythym.isPattern && !beatsInOrder[index].isPattern)
                            {
                                foreach (AudioClip audioClip in tempoClipLists[((int)tempo / 10) - 1].clips)
                                {
                                    if (audioClip.name == rythym.clip.name + " (" + tempo + ")")
                                    {
                                        clip = audioClip;
                                    }
                                }
                            }
                        }
                        rythymAudioSource.PlayOneShot(clip);
                        beatsInOrder[i].playedAlready = true;
                    }
                }
                timer += Time.deltaTime;
                if (timer > (totalMeasures * beatsInAMesure) * (1 / (tempo / 60 == 0 ? 1 : tempo / 60)))
                {
                    metronomeOn = false;
                    rythymAudioSource.Stop();
                    StopCoroutine(routine);
                    playing = false;
                    startedPlayback = false;
                    UI.PlayRythym();
                }
                yield return null;
            }
            yield return null;
        }
        metronomeOn = false;
        rythymAudioSource.Stop();
        playing = false;
        countOff = staticCountOff;
    }
}

public class playedNote
{
    public float beat;
    public bool playedAlready;
    public SymbolType type;
    public float length;
    public bool isPattern;
    public string patternName;

    public playedNote(float BEAT, bool PLAYEDALREADY, SymbolType TYPE, float LENGTH, bool ISPATTERN, string PATTERNNAME)
    {
        beat = BEAT;
        playedAlready = PLAYEDALREADY;
        type = TYPE;
        length = LENGTH;
        isPattern = ISPATTERN;
        patternName = PATTERNNAME;
    }
}
