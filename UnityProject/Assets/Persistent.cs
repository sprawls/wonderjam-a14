using UnityEngine;
using System.Collections;

public class Persistent : MonoBehaviour {

    public string songPath;
    public int songBPM;
    public float songMulti;

	static bool instanceIsLoaded = false;

	void OnLevelWasLoaded(int level){
		if(level == 0) RenderSettings.ambientLight = new Color(0.2f,0.2f,0.2f);
		else if(level == 1) RenderSettings.ambientLight = new Color(0.2f,0.2f,0.2f);
		else if(level == 2) RenderSettings.ambientLight = new Color(0,0,0);
	}


	// Use this for initialization
	void Start () {
		if(instanceIsLoaded == false) {
			DontDestroyOnLoad(this);
			instanceIsLoaded = true;
			//Cap framerate
			Application.targetFrameRate = 60;
			OnLevelWasLoaded(Application.loadedLevel);
		} else {
			Destroy (this);
		}
     

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
