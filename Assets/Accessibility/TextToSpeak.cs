using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeak : MonoBehaviour {

public string textToSpeak;
public bool changeState = false;
/// <summary>
/// This function is called when the object becomes enabled and active.
/// </summary>
void OnEnable()
{
    if(textToSpeak== null)
    {
        textToSpeak = gameObject.name;
    }
}

}
