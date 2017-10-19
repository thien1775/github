using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationController : MonoBehaviour {

    public GameObject map;
    public GameObject ffspawner;
    public GameObject bnspawner;
    public GameObject slider;
    

    public double latMinMap;
    public double lngMinMap;
    public double latMaxMap;
    public double lngMaxMap;

    // Use this for initialization
    void Start () {

        //yield return new WaitForSeconds(5);
        //UpdateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateMap()
    {
        var ffs = ffspawner.GetComponent<FFSpawner>();
        var bns = bnspawner.GetComponent<BNSpawner>();
        var m = map.GetComponent<OnlineMaps>();
        bool update = ffs.UpdateCenter();
        if (true)
        {
            slider.GetComponent<Slider>().value = 15;
            m.zoom = 15;
            m.SetPosition(ffs.lngCenter, ffs.latCenter);
            m.GetBottomRightPosition(out lngMaxMap, out latMinMap);
            m.GetTopLeftPosition(out lngMinMap, out latMaxMap);
            
            foreach (GameObject ff in ffs.FFList)
            {
                var ffpos = ff.GetComponent<CircleData>();
                double xpos = (ffpos.lng - lngMinMap) / (lngMaxMap - lngMinMap) * 40.96 - 20.48;
                double ypos = (ffpos.lat - latMinMap) / (latMaxMap - latMinMap) * 40.96 - 20.48;
                ff.transform.localPosition = new Vector3((float)xpos, (float)ypos, ffpos.alt);
            }
            
            foreach(GameObject bn in bns.BNList)
            {
                var bnpos = bn.GetComponent<TriangleData>();
                double xpos = (bnpos.lng - lngMinMap) / (lngMaxMap - lngMinMap) * 40.96 - 20.48;
                double ypos = (bnpos.lat - latMinMap) / (latMaxMap - latMinMap) * 40.96 - 20.48;
                bn.transform.localPosition = new Vector3((float)xpos, (float)ypos, bnpos.alt);
            }
        }
    }
}
