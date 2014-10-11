using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IBeatReceiver {

    private BeatManager BeatManagerRef;
	private ZombieFactory ZombieFactoryRef;

    List<IUpdate> component = new List<IUpdate>();
    List<IBeatReceiver> beats = new List<IBeatReceiver>();

	// Use this for initialization
	void Start () {
        BeatManagerRef = new BeatManager(this);
		ZombieFactoryRef = new ZombieFactory(this);
        beats.Add(ZombieFactoryRef);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            BeatManagerRef.setInputP2(0);
        }
        if (Input.GetKeyDown("a"))
        {
            BeatManagerRef.setInputP2(1);
        }
        if (Input.GetKeyDown("s"))
        {
            BeatManagerRef.setInputP2(2);
        }
        if (Input.GetKeyDown("d"))
        {
            BeatManagerRef.setInputP2(3);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            BeatManagerRef.setInputP1(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            BeatManagerRef.setInputP1(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            BeatManagerRef.setInputP1(2);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            BeatManagerRef.setInputP1(3);
        }
    }

	void LateUpdate () {

	    foreach(var n in component)
        {
            n.OnUpdate();
        }
	}

    public void requestBeat(IBeatReceiver b)
    {
        beats.Add(b);
    }

    public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1)
    {
        foreach (var n in beats)
        {
            n.OnBeat(p1, p2, turnP1);
        }
    }
    
}
