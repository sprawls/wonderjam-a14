using UnityEngine;
using System.Collections;

public class ZombieAnchor : MonoBehaviour {

	/// <summary>
	/// This Script just positions the basic Anchor for the Zombies to base on.
	/// 
	/// </summary>

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(34f,0.5f,-4f); //Ugly stuff here, look away.
		transform.rotation = Quaternion.Euler(90,270,0);
		transform.localScale = new Vector3(1,1,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
