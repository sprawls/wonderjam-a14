using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>, IBeatReceiver {

    public GameObject black;
    public GameObject white;

	protected GameManager () {} // guarantee this will be always a singleton only - can't use the constructor!

    private BeatManager BeatManagerRef;
	private ZombieFactory ZombieFactoryRef;

    private List<IUpdate> component = new List<IUpdate>();
    private List<IBeatReceiver> beats = new List<IBeatReceiver>();
    private TileAnimation[] tiles;

    private AudioClip song;

    private Persistent PersistentScript;

    private Animator camAnim;
    private PlayerBehaviour p1;
    private PlayerBehaviour p2;

    private bool finished = false;

	// Use this for initialization
	void Start () {

        GameObject persistentObj = GameObject.Find("persistent");
        PersistentScript = persistentObj.GetComponent("Persistent") as Persistent;
        song = Resources.Load(PersistentScript.songPath) as AudioClip;
        audio.clip = song;
        audio.pitch = PersistentScript.songMulti;
        audio.Play();

		BeatManagerRef = BeatManager.Instance;
		ZombieFactoryRef = ZombieFactory.Instance;

        BeatManager.Instance.SetBeat(this);
        ZombieFactory.Instance.SetBeat(this);

        BeatManagerRef.changeTempo(PersistentScript.songBPM);

        tiles = PlancherCreator.CreatePlancher(black, white);

        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        p1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerBehaviour>();
        p2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
    void Update()
    {
		// @TODO
        if (Input.GetKeyDown("w"))
        {
            BeatManagerRef.setInputP1(0);
        }
        if (Input.GetKeyDown("a"))
        {
            BeatManagerRef.setInputP1(1);
        }
        if (Input.GetKeyDown("s"))
        {
            BeatManagerRef.setInputP1(2);
        }
        if (Input.GetKeyDown("d"))
        {
            BeatManagerRef.setInputP1(3);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            BeatManagerRef.setInputP2(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            BeatManagerRef.setInputP2(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            BeatManagerRef.setInputP2(2);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            BeatManagerRef.setInputP2(3);
        }
    }

	void LateUpdate () {

	    foreach(var n in component)
        {
            n.OnUpdate();
        }
        int l = checkVictory();
        if(l!=0)
        {
            doVictory(l);
        }
	}

    public void requestBeat(IBeatReceiver b)
    {
        beats.Add(b);
    }

    public void OnBeat(BeatEnum mainPlayer, BeatEnum offPlayer, bool turnP1)
    {
        foreach (var n in beats)
        {
            n.OnBeat(mainPlayer, offPlayer, turnP1);
        }
    }

    public void OnQuarterBeat()
    {
        foreach (var n in beats)
        {
            n.OnQuarterBeat();
        }
    }

    public TileAnimation getTile(int  i)
    {
//		Debug.Log(i);
        return tiles[i];
    }

    private int checkVictory()
    {
        if(p1.score > p2.score)
        {
            if(p1.score > 30000)
            {
                return 1;
            }
        }
        else
        {
            if (p2.score > 30000)
            {
                return 2;
            }
        }
        return 0;
    }

    private void doVictory(int player)
    {
        if (!finished)
        {
            string name = "Player" + player.ToString() + "_win";
            
            camAnim.SetTrigger(name);
            finished = true;


            p1.Part2 = "a gagné!";
            p2.Part2 = "a gagné!";
 
            
        }
    }


    
}
