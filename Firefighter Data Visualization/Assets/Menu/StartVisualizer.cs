using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartVisualizer : MonoBehaviour {

    public GameObject LoadingPrefab;
    public GameObject UICanvas;
    
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Startup()
    {
        GameObject LoadingScreen = Instantiate(LoadingPrefab);
        LoadingScreen.transform.parent = UICanvas.transform;
        LoadingScreen.transform.localScale = new Vector3(1, 1, 1);

        SceneManager.LoadScene("Visualizer", LoadSceneMode.Single);
    }
}
