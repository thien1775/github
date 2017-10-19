using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InfinityCode;

public class UIFunctions : MonoBehaviour {

    public GameObject slider;
    public GameObject LoadScreenPrefab;
    public GameObject canvas;
    public GameObject map;
    public static bool closeScene = false;

    
    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        slider.GetComponent<UnityEngine.UI.Slider>().value += Input.GetAxis("Mouse ScrollWheel");
	}

    public void BackToMenu()
    {
        closeScene = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void toSattelite()
    {
        OnlineMaps.instance.mapType = "google.satellite";
    }

    public void toDark()
    {
        OnlineMaps.instance.mapType = "mapbox classic";
    }

    public void toLight()
    {
        OnlineMaps.instance.mapType = "google.relief";
    }

}
