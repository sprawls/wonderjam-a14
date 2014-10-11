using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieFactory : MonoBehaviour, IBeatReceiver , IUpdate {

	public List<ZombieBehaviour>  Zombies;

	private GameObject ZombiePrefab;
	private GameObject emptyObject;
	private GameObject zombieAnchorObject;
	private IBeatReceiver IbeatReceiverRef;
	private int NumZombies = 50;
	

	public void OnUpdate(){

	}

	public ZombieFactory(IBeatReceiver Beat){
		IbeatReceiverRef = Beat;

		ZombiePrefab = (GameObject) Resources.Load ("prefab/ZombiePrefab");
		zombieAnchorObject = (GameObject) Resources.Load ("prefab/ZombieAnchor");
		Zombies = new List<ZombieBehaviour>();
		//instantiate empty game object for grid
		Instantiate (zombieAnchorObject);
		emptyObject = GameObject.Find("ZombieAnchor(Clone)");



		for(int i = 0; i < NumZombies; i++) {
			Debug.Log (ZombiePrefab + "  " + zombieAnchorObject);
			Zombies.Add (((GameObject)Instantiate(ZombiePrefab,emptyObject.transform.position, emptyObject.transform.rotation)).GetComponent<ZombieBehaviour>());
			Zombies[i].transform.parent = emptyObject.transform;
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool p1turn){
		for(int i = 0; i < Zombies.Count; i++) {
			Zombies[i].OnBeat (p1,p2);
		}
	}


	

	void Update () {
	
	}


}
