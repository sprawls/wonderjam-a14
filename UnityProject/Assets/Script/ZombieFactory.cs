using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieFactory : Singleton<ZombieFactory>, IBeatReceiver , IUpdate {
	protected ZombieFactory () {}

	public List<ZombieBehaviour>  Zombies;

	private GameObject ZombiePrefab;
	private GameObject emptyObject;
	private GameObject zombieAnchorObject;
	private IBeatReceiver IbeatReceiverRef;
	private int NumZombies = 50;
	

	public void OnUpdate(){

	}

	public void SetBeat(IBeatReceiver Beat){
		IbeatReceiverRef = Beat;

		ZombiePrefab = (GameObject) Resources.Load ("prefab/ZombiePrefab");
		zombieAnchorObject = (GameObject) Resources.Load ("prefab/ZombieAnchor");
		Zombies = new List<ZombieBehaviour>();
		//instantiate empty game object for grid
		Instantiate (zombieAnchorObject);
		emptyObject = GameObject.Find("ZombieAnchor(Clone)");

		for(int i = 0; i < NumZombies; i++) {
			Zombies.Add (((GameObject)Instantiate(ZombiePrefab)).GetComponent<ZombieBehaviour>());
			Zombies[i].transform.parent = emptyObject.transform;
			Zombies[i].transform.localRotation = Quaternion.Euler(new Vector3(90,180,0)); //Orient sprites with camera
		}

		// Register to beat
		GameManager.Instance.requestBeat (this);
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool p1turn){
		for(int i = 0; i < Zombies.Count; i++) {
			Zombies[i].OnBeat (p1,p2,p1turn);
		}
	}


	

	void Update () {
	
	}


}
