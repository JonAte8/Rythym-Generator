using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletechildrenScript : MonoBehaviour
{

    public List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startDeletion()
    {
        StartCoroutine(deleteChildren());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator deleteChildren()
    {
        foreach(GameObject child in children)
        {
            Destroy(child);
        }
        yield return null;
    }
}
