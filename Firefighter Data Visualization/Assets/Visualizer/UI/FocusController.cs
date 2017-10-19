using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusController : MonoBehaviour {

    public Text Focus;

    public List<string> header;
    public string body = "";

	public void updateFocus()
    {
        Focus.text = "";
        foreach(string message in header)
        {
            Focus.text += "> " + message + "\n";
        }
        Focus.text += body;
    }

    
}
