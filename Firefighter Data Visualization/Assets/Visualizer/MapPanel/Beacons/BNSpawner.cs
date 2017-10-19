using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class BNSpawner : MonoBehaviour
{


    public GameObject BNPrefab;
    public GameObject Spawner;
    public GameObject Panel;
    public GameObject terminal;
    public GameObject NetworkManager;
    public GameObject FocusWindow;

    public List<GameObject> BNList = new List<GameObject>();

    private float TriScale;


    class BN
    {
        public int id = -1;
        public float lat = 0;
        public float lon = 0;
        public float alt = 0;
        public float temp = 0;
    }

    // Use this for initialization
    void Start()
    {
        TriScale = PlayerPrefs.GetFloat("TriScale");

    }

    // Update is called once per frame
    void Update()
    {
        int index = -1;
        while (WSNetworkManager.BNQueue.Count > 0)
        {
            string instruction = WSNetworkManager.BNQueue.Dequeue();
            if (instruction.Contains("Destroy"))
            {

            }
            else
            {
                BN n = JsonUtility.FromJson<BN>(instruction);
                index = BNList.FindIndex(BN => BN.GetComponent<TriangleData>().id == n.id);

                if (index >= 0)
                {
                    // Object already in list
                    var bnpos = BNList[index].GetComponent<TriangleData>();
                    var m = Panel.GetComponent<LocationController>();
                    bnpos.lng = n.lon;
                    bnpos.lat = n.lat;
                    bnpos.alt = n.alt;
                    bnpos.tmp = n.temp;
                    bnpos.UpdateFocus();
                    double xpos = (bnpos.lng - m.lngMinMap) / (m.lngMaxMap - m.lngMinMap) * 40.96 - 20.48;
                    double ypos = (bnpos.lat - m.latMinMap) / (m.latMaxMap - m.latMinMap) * 40.96 - 20.48;
                    BNList[index].transform.localPosition = new Vector3((float)xpos, (float)ypos, bnpos.alt);
                }
                else
                {
                    GameObject nobj = (GameObject)Instantiate(BNPrefab);
                    var data = nobj.GetComponent<TriangleData>();
                    data.lat = n.lat;
                    data.lng = n.lon;
                    data.alt = n.alt;
                    data.id = n.id;
                    data.tmp = n.temp;
                    data.RestManager = NetworkManager;
                    data.Spawner = Spawner;
                    data.Self = nobj;
                    data.FocusWindow = FocusWindow;

                    nobj.transform.parent = Spawner.transform;
                    nobj.GetComponent<Transform>().localScale = new Vector3(TriScale * ZoomController.ZoomScale, TriScale * ZoomController.ZoomScale, 1);
                    BNList.Add(nobj);
                    Panel.GetComponent<LocationController>().UpdateMap();
                    terminal.GetComponent<TerminalController>().TerminalColorPrint("New BN object with ID: " + data.id, nobj.GetComponent<SpriteRenderer>().color);

                }
            }
        }

        if (UIFunctions.closeScene)
        {
            foreach (GameObject bn in BNList)
            {
                Destroy(bn);
            }
            BNList.Clear();
        }
    }

    public void UpdateScale()
    {
        foreach (GameObject bn in BNList)
        {
            bn.GetComponent<Transform>().localScale = new Vector3(TriScale * ZoomController.ZoomScale, TriScale * ZoomController.ZoomScale, 1);
        }
    }

    public float maxTemp()
    {
        float max = -1;
        foreach(GameObject bn in BNList)
        {
            max = Math.Max(max, bn.GetComponent<TriangleData>().tmp);
        }
        return max;
    }
}