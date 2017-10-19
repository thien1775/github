using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewController : MonoBehaviour {

    public Text OverviewField;
    public GameObject ffspawner;
    public GameObject bnspawner;

    public string address = "";
    public Time runtime = null;
    public float latCenter = 0;
    public float lngCenter = 0;


	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateText", 1, 1);
	}


    public void UpdateText()
    {
        var ff = ffspawner.GetComponent<FFSpawner>();
        var bn = bnspawner.GetComponent<BNSpawner>();
        if (latCenter != ff.latCenter || lngCenter != ff.lngCenter)
        {
            latCenter = ff.latCenter;
            lngCenter = ff.lngCenter;
            UpdateLocation(new Vector2(lngCenter, latCenter));
        }

        string output = "";
        output += "> Location: " + address + "\n";
        output += "> Lat/Lon: " + latCenter.ToString("N6") + ", " + lngCenter.ToString("N6") + "\n";
        output += "> Runtime: " + TimeSpan.FromSeconds((int)Time.timeSinceLevelLoad).ToString() + "\n";
        output += "> Firefighters: " + ff.FFList.Count + "\n";
        output += "> Beacons: " + bn.BNList.Count + "\n";
        output += "> Max Temp: " + Math.Max(ff.maxTemp(), bn.maxTemp()) + "°F\n";
        output += "> Max CO: " + ff.maxCO() + "ppm";
        OverviewField.text = output;
    }

    public void UpdateLocation(Vector2 point)
    {
        OnlineMapsGoogleGeocoding query = OnlineMapsGoogleGeocoding.Find(point);
        query.OnComplete += QueryComplete;
    }

    private void QueryComplete(string s)
    {
        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(s);
        address =  doc.Element("GeocodeResponse").Element("result").Element("formatted_address").Value;
        UpdateText();
    }
}
