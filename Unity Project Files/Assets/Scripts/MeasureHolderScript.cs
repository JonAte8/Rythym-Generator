using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureHolderScript : MonoBehaviour
{

    public List<MeasureScript> measureScripts;
    public List<List<playedNote>> measureBeatsLists;
    int amountOfMeasures;

    // Start is called before the first frame update
    void Start()
    {
        amountOfMeasures = measureScripts.Count;
        measureBeatsLists = new List<List<playedNote>>( new List<playedNote>[amountOfMeasures] );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
