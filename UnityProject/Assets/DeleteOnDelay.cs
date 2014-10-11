using UnityEngine;
using System.Collections;

public class DeleteOnDelay : MonoBehaviour {

	public float delay = 3f;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
