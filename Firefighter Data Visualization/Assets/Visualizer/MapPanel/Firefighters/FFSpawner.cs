using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class FFSpawner : MonoBehaviour {


    public GameObject FFPrefab;
    public GameObject Spawner;
    public GameObject Panel;
    public GameObject terminal;
    public GameObject NetworkManager;
    public GameObject FocusWindow;
    public GameObject TargetPrefab;

    public List<GameObject> FFList = new List<GameObject>();

    public float latCenter;
    public float lngCenter;
    public float latRange;
    public float lngRange;

    private float DotScale;


    class FF
    {
        public float lat;
        public float lon;
        public float alt;
        public float temp;
        public int id;
        public string time;
        public float light;
        public float co;
        public float humidity;
    }

	// Use this for initialization
	void Start () {
        DotScale = PlayerPrefs.GetFloat("DotScale");

	}
	
	// Update is called once per frame
	void Update () {
        int index = -1;
        while (WSNetworkManager.FFQueue.Count > 0)
        {
            string instruction = WSNetworkManager.FFQueue.Dequeue();
            if (instruction.Contains("Destroy"))
            {
                Destroy(FFList[index]);
                FFList.RemoveAt(index);
            }
            else
            {
                FF n = JsonUtility.FromJson<FF>(instruction);
                index = FFList.FindIndex(FF => FF.GetComponent<CircleData>().id == n.id);

                if (index >= 0)
                {
                    // Object already in list
                    var ffpos = FFList[index].GetComponent<CircleData>();
                    var m = Panel.GetComponent<LocationController>();
                    ffpos.lng = n.lon;
                    ffpos.lat = n.lat;
                    ffpos.alt = n.alt;
                    ffpos.tmp = n.temp;
                    ffpos.time = n.time;
                    ffpos.humidity = n.humidity;
                    ffpos.light = n.light;
                    ffpos.co = n.co;
                    if (ffpos.currentTarget) { ffpos.UpdateFocus(); };
                    double xpos = (ffpos.lng - m.lngMinMap) / (m.lngMaxMap - m.lngMinMap) * 40.96 - 20.48;
                    double ypos = (ffpos.lat - m.latMinMap) / (m.latMaxMap - m.latMinMap) * 40.96 - 20.48;
                    FFList[index].transform.localPosition = new Vector3((float)xpos, (float)ypos, ffpos.alt);
                }
                else
                {
                    GameObject nobj = (GameObject)Instantiate(FFPrefab);
                    Color nColor = UnityEngine.Random.ColorHSV((float)0.2027, (float)1.2027, 1, 1, 1, 1, 1, 1);
                    nobj.GetComponent<SpriteRenderer>().color = nColor;
                    nobj.GetComponent<TrailRenderer>().startColor = nColor;
                    nColor.a = 0;
                    nobj.GetComponent<TrailRenderer>().endColor = nColor;


                    var data = nobj.GetComponent<CircleData>();
                    data.lat = n.lat;
                    data.lng = n.lon;
                    data.alt = n.alt;
                    data.id = n.id;
                    data.tmp = n.temp;
                    data.time = n.time;
                    data.humidity = n.humidity;
                    data.light = n.light;
                    data.co = n.co;
                    data.RestManager = NetworkManager;
                    data.Spawner = Spawner;
                    data.Self = nobj;
                    data.FocusWindow = FocusWindow;
                    data.TargetPrefab = TargetPrefab;

                    nobj.transform.parent = Spawner.transform;
                    nobj.GetComponent<Transform>().localScale = new Vector3(DotScale * ZoomController.ZoomScale, DotScale * ZoomController.ZoomScale, 1);
                    FFList.Add(nobj);
                    Panel.GetComponent<LocationController>().UpdateMap();
                    terminal.GetComponent<TerminalController>().TerminalColorPrint("New FF object with ID: " + data.id, Color.cyan);

                }
            }
        }

        if (UIFunctions.closeScene)
        {
            foreach(GameObject ff in FFList)
            {
                Destroy(ff);
            }
            FFList.Clear();
        }
	}

    public Boolean UpdateCenter()
    {
        float latMin;
        float lngMin;
        float latMax;
        float lngMax;

        if (FFList.Count > 0)
        {
            latMin = latMax = FFList[0].GetComponent<CircleData>().lat;
            lngMin = lngMax = FFList[0].GetComponent<CircleData>().lng;

            foreach (GameObject ff in FFList)
            {
                var data = ff.GetComponent<CircleData>();
                if (data.lat < latMin) latMin = data.lat;
                if (data.lng < lngMin) lngMin = data.lng;
                if (data.lat > latMax) latMax = data.lat;
                if (data.lng > lngMax) lngMax = data.lng;
            }

            
            float latRangen = latMax - latMin;
            float lngRangen = lngMax - lngMin;
            float latCentern = latMax - latRangen / 2;
            float lngCentern = lngMax - lngRangen / 2;
            
            if (Math.Abs(latCentern - latCenter) > latRange / 4 || Math.Abs(lngCentern - lngCenter) > lngRange / 4)
            {
                latRange = latRangen;
                lngRange = lngRangen;
                latCenter = latCentern;
                lngCenter = lngCentern;
                return true;
            }
            
            /*
            Debug.Log("###########################################");
            Debug.Log(latRange);
            Debug.Log(lngRange);
            Debug.Log(latCenter);
            Debug.Log(lngCenter);
            */
        }
        return false;
    }
    public void UpdateScale()
    { 
        foreach(GameObject ff in FFList)
        {
            ff.GetComponent<Transform>().localScale = new Vector3(DotScale * ZoomController.ZoomScale, DotScale * ZoomController.ZoomScale, 1);
        }
    }

    public void RequestTarget()
    {
        foreach(GameObject ff in FFList)
        {
            
            if (ff.GetComponent<CircleData>().currentTarget)
            {
                Debug.Log("Removing");
                ff.GetComponent<CircleData>().RemoveTarget();
                break;
            }
        }
    }

    public float maxTemp()
    {
        float max = -1;
        foreach (GameObject ff in FFList)
        {
            max = Math.Max(max, ff.GetComponent<CircleData>().tmp);
        }
        return max;
    }

    public float maxCO()
    {
        float max = -1;
        foreach(GameObject ff in FFList)
        {
            max = Math.Max(max, ff.GetComponent<CircleData>().co);
        }
        return max;
    }

}
