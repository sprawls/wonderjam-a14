using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IBeatReceiver {

    private BeatManager BeatManagerRef;
	private ZombieFactory ZombieFactoryRef;

    List<IUpdate> component = new List<IUpdate>();

	// Use this for initialization
	void Start () {
        BeatManagerRef = new BeatManager(this);
		ZombieFactoryRef = new ZombieFactory(this);
	}
	
	// Update is called once per frame
	void LateUpdate () {

	    foreach(var n in component)
        {
            n.OnUpdate();
        }

	}

    public void OnBeat(BeatEnum p1, BeatEnum p2)
    {
		ZombieFactoryRef.OnBeat (p1,p2);
    }
    
}
