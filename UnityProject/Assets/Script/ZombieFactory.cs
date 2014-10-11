using UnityEngine;
using System.Collections;

public class ZombieFactory : MonoBehaviour, IBeatReceiver , IUpdate {

	public ZombieBehaviour[]  Zombies;
	public GameObject ZombiePrefab;

	private IBeatReceiver IbeatReceiverRef;
	private int NumZombies = 10;

	public void OnUpdate(){

	}

	public ZombieFactory(IBeatReceiver Beat){
		IbeatReceiverRef = Beat;
		Zombies = new ZombieBehaviour[10];
		for(int i = 0; i < NumZombies; i++) {
			Zombies[i] = ((GameObject)Instantiate(ZombiePrefab)).GetComponent<ZombieBehaviour>();
		}
	}

	public void OnBeat(BeatEnum mainInput, BeatEnum otherInput){
		for(int i = 0; i < Zombies.Length; i++) {
			Zombies[i].OnBeat (mainInput,otherInput);
		}
	}


	

	void Update () {
	
	}


}
