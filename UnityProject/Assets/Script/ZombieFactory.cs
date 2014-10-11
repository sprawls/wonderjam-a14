using UnityEngine;
using System.Collections;

public class ZombieFactory : IBeatReceiver , IUpdate {

	public ZombieBehaviour[]  Zombies;

	private int NumZombies = 10;


	public ZombieFactory(){
		Zombies = new ZombieBehaviour[10];
		for(int i = 0; i < NumZombies; i++) {
			Zombies[i] = new ZombieBehaviour();
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
