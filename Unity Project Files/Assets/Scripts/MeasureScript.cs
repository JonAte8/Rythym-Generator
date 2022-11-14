using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureScript : MonoBehaviour
{
    public MeasureHolderScript measureHolderScript;
    public List<Symbol> symbols;
    public List<Symbol> condensedSymbols;
    public RythymManager manager;
    public GameObject instantiationParent;
    public List<Transform> childrenNotes;
    public List<playedNote> measureBeats;
    public int whichMeasure;
    public GameObject[] timeSigs;

    // Start is called before the first frame update
    void Start()
    {
        symbols = new List<Symbol>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CondenseMeasure()
    {
        StartCoroutine(CondenseCourotine());
    }

    IEnumerator CondenseCourotine()
    {
        if (manager.condenseMeasures)
        {
            condensedSymbols = new List<Symbol>();
            float addedRestLength = 0;
            for (int i = 0; i < symbols.Count; i++)
            {
                if (i + 1 < symbols.Count)
                {
                    if (symbols[i].symbolType == SymbolType.Rest && symbols[i + 1].symbolType == SymbolType.Rest)
                    {
                        addedRestLength += symbols[i].symbolLength;
                    }
                    else if (symbols[i].symbolType == SymbolType.Rest && symbols[i + 1].symbolType != SymbolType.Rest)
                    {
                        addedRestLength += symbols[i].symbolLength;
                        switch (addedRestLength)
                        {
                            case 4:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[1]));
                                break;
                            case (float)3.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 3:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                break;
                            case (float)2.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 2:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                break;
                            case (float)1.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 1:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                break;
                            case (float)0.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                        }
                        addedRestLength = 0;
                    }
                    else
                    {
                        condensedSymbols.Add(new Symbol(symbols[i]));
                    }
                }
                else
                {
                    if (symbols[i].symbolType == SymbolType.Rest)
                    {
                        addedRestLength += symbols[i].symbolLength;
                        switch (addedRestLength)
                        {
                            case 4:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[1]));
                                break;
                            case (float)3.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 3:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                break;
                            case (float)2.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 2:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[3]));
                                break;
                            case (float)1.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                            case 1:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[5]));
                                break;
                            case (float)0.5:
                                condensedSymbols.Add(new Symbol(manager.allSymbols[7]));
                                break;
                        }
                        addedRestLength = 0;
                    }
                    else
                    {
                        condensedSymbols.Add(new Symbol(symbols[i]));
                    }
                }
            }
        }
        else
        {
            condensedSymbols = new List<Symbol>();
            for (int i = 0; i < symbols.Count; i++)
            {
                condensedSymbols.Add(new Symbol(symbols[i]));
            }
        }
        StartCoroutine(designateBeats());
        yield return null;
    }

    IEnumerator designateBeats()
    {
        float currentBeat = 1;
        measureBeats = new List<playedNote>();
        for (int i = 0; i < condensedSymbols.Count; i++)
        {
            condensedSymbols[i].beatSymbolIsOn = currentBeat;
            measureBeats.Add(new playedNote(currentBeat, false, condensedSymbols[i].symbolType, condensedSymbols[i].symbolLength, condensedSymbols[i].isPattern, condensedSymbols[i].patternName));
            foreach(Symbol symbol in manager.allSymbols)
            {
                symbol.beatSymbolIsOn = 0;
            }
            currentBeat += condensedSymbols[i].symbolLength;
        }
        measureHolderScript.measureBeatsLists[whichMeasure - 1] = measureBeats;
        StartCoroutine(VisualizeMusic());
        yield return null;
    }

    IEnumerator VisualizeMusic()
    {
        for (int i = 0; i < condensedSymbols.Count; i++)
        {
            GameObject SYMBOL = Instantiate(condensedSymbols[i].symbolPrefab, instantiationParent.transform, false);
            SYMBOL.transform.localPosition = new Vector3(condensedSymbols[i].beatSymbolIsOn, SYMBOL.transform.localPosition.y, SYMBOL.transform.localPosition.z);
            childrenNotes.Add(SYMBOL.transform);
        }
        yield return null;
    }
}
