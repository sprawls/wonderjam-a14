using UnityEngine;
using System.Collections;

public class SpotLightFollowScript : MonoBehaviour {

	public Transform objectToFollow;
	public Light myLight;

	// Use this for initialization
	void Start () {
		objectToFollow = GetComponentInChildren<RandomMouvementInArea>().transform;
		myLight = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		myLight.transform.LookAt(objectToFollow.position);
	}
}
