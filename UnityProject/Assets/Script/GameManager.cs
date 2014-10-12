using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>, IBeatReceiver {

    public GameObject black;
    public GameObject white;
	public GUISkin skin;
	public int maxScore = 4000;

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
	private bool showEnding = false;

    private AudioSource victoryGuylaine;
    private AudioSource victoryJerry;


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

        AudioSource[] victories = this.GetComponentsInChildren<AudioSource>();
        Debug.Log(victories.Length);
        foreach(var v in victories)
        {
            if(v.gameObject.name == "Guylaine")
            {
                victoryGuylaine = v;
            }
            else if(v.gameObject.name == "Jerry")
            {
                victoryJerry = v;
            }
        }
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

	void OnGUI() {
		GUI.skin = skin;
		GUI.Label (new Rect (0, 0, Screen.width, 45), "Objectif: " + maxScore.ToString ());
		if(showEnding == true) {
			Debug.Log ("IM Showing Button !!");
			if(GUI.Button (new Rect((2f/6f)*Screen.width, (4f/6f)*Screen.height, (2f/6f)*Screen.width, (1f/6f)*Screen.width), "Main Menu")){
				Application.LoadLevel(0);
			}
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
        return tiles[i];
    }

    public int checkVictory()
    {
        if(p1.Score > p2.Score)
        {
            if(p1.Score > maxScore)
            {
                return 1;
            }
        }
        else
        {
            if (p2.Score > maxScore)
            {
                return 2;
            }
        }
        return 0;
    }

	public bool Finished {
		get {
			return finished;
		}
	}

	public bool IsAnimating {
		get {
			return !(camAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
		}
	}

    public bool Playing { get { return !IsAnimating && !Finished; } }

    private void doVictory(int player)
    {
        if (!finished)
        {
			StartCoroutine (DisplayEnding());
            string name = "Player" + player.ToString() + "_win";
            
            camAnim.SetTrigger(name);
            finished = true;

            p1.Part2 = "a gagné!";
            p2.Part2 = "a gagné!";
        }

    }

    private void playSong(int player)
    {
        
        if (player == 1)
        {
            victoryGuylaine.Play();
        }
        else if (player == 2)
        {
            victoryJerry.Play();
        }
    }

	IEnumerator DisplayEnding(){
		yield return new WaitForSeconds(6f);
		showEnding = true;
	}


}
