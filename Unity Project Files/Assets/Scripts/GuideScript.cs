using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideScript : MonoBehaviour
{
    public Transform slide;
    public float lastSlidePosition;
    public float firstSlidePosition;
    public void NextButton()
    {
        if(slide.transform.localPosition.x == lastSlidePosition)
        {
            slide.transform.localPosition = new Vector3(firstSlidePosition, 0, 0);
        }
        else
        {
            slide.transform.localPosition += new Vector3(-600, 0, 0);
        }
    }

    public void PreviousButton()
    {
        if(slide.transform.localPosition.x == firstSlidePosition)
        {
            slide.transform.localPosition = new Vector3(lastSlidePosition, 0, 0);
        }
        else
        {
            slide.transform.localPosition += new Vector3(600, 0, 0);
        }
    }
}
