using UnityEngine;
using System.Collections;

public class Persistent : MonoBehaviour {

    public string songPath;
    public int songBPM;
    public float songMulti;


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
