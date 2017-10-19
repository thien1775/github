using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public GameObject SettingsPanel;
    public GameObject SettingsIcon;
    public GameObject CloseIcon;
    public GameObject IPField;
    public GameObject PortField;
    public GameObject FFSlider;
    public GameObject FFExample;
    public GameObject BNSlider;
    public GameObject BNExample;

    private bool isActive = false;


	// Use this for initialization
	void Start () {
        IPField.GetComponent<InputField>().text = PlayerPrefs.GetString("ServerIP");
        PortField.GetComponent<InputField>().text = PlayerPrefs.GetString("ServerPort");
        float scale = PlayerPrefs.GetFloat("DotScale");
        FFSlider.GetComponent<Slider>().value = scale;
        FFExample.transform.localScale = new Vector3(scale * 100, scale * 100, 1);
        scale = PlayerPrefs.GetFloat("TriScale");
        BNSlider.GetComponent<Slider>().value = scale;
        BNExample.transform.localScale = new Vector3(scale * 100, scale * 100, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleSettings()
    {
        if (isActive)
        {
            SettingsPanel.transform.GetComponent<RectTransform>().pivot = new Vector2(0, (float)0.5);
            SettingsIcon.GetComponent<Renderer>().enabled = true;
            CloseIcon.GetComponent<Renderer>().enabled = false;
            isActive = false;
        }
        else
        {
            SettingsPanel.transform.GetComponent<RectTransform>().pivot = new Vector2(1, (float)0.5);
            SettingsIcon.GetComponent<Renderer>().enabled = false;
            CloseIcon.GetComponent<Renderer>().enabled = true;
            isActive = true;
        }
    }

    public void UpdateIP(string ip)
    {
        PlayerPrefs.SetString("ServerIP", ip);
    }

    public void UpdatePort(string port)
    {
        PlayerPrefs.SetString("ServerPort", port);
    }

    public void UpdateFF(float scale)
    {
        FFExample.transform.localScale = new Vector3(scale*100, scale*100, 1);
        PlayerPrefs.SetFloat("DotScale", scale);
    }

    public void UpdateBN(float scale)
    {
        BNExample.transform.localScale = new Vector3(scale*100, scale*100, 1);
        PlayerPrefs.SetFloat("TriScale", scale);
    }
}
