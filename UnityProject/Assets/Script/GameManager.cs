using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    List<IUpdate> component = new List<IUpdate>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

	    foreach(var n in component)
        {
            n.OnUpdate();
        }

	}
}
