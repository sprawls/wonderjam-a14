using UnityEngine;
using System.Collections;

public class ZombieFactory : MonoBehaviour, IBeatReceiver , IUpdate {

	public ZombieBehaviour[]  Zombies;

	private GameObject ZombiePrefab;
	private GameObject emptyObject;
	private IBeatReceiver IbeatReceiverRef;
	private int NumZombies = 10;
	

	public void OnUpdate(){

	}

	public ZombieFactory(IBeatReceiver Beat){
		IbeatReceiverRef = Beat;

		ZombiePrefab = (GameObject) Resources.Load ("prefab/zombie");
		Zombies = new ZombieBehaviour[10];
		//instantiate empty game object for grid
		emptyObject = (GameObject) Instantiate (new GameObject(), Vector3.zero, Quaternion.identity);
		for(int i = 0; i < NumZombies; i++) {
			Zombies[i] = ((GameObject)Instantiate(ZombiePrefab)).GetComponent<ZombieBehaviour>();
			Zombies[i].transform.parent = emptyObject.transform;
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2){
		for(int i = 0; i < Zombies.Length; i++) {
			Zombies[i].OnBeat (p1,p2);
		}
	}


	

	void Update () {
	
	}


}
