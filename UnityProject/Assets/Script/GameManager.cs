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
    public PlayerBehaviour p1 { get; private set; }
    public PlayerBehaviour p2 { get; private set; }

    private bool finished = false;
	private bool showEnding = false;

    private AudioSource victoryGuylaine;
    private AudioSource victoryJerry;

    IKeyGetter k1;
    IKeyGetter k2;

	// Use this for initialization
	void Start () {

        GameObject persistentObj = GameObject.Find("persistent");
        PersistentScript = persistentObj.GetComponent("Persistent") as Persistent;
        maxScore = getObjScore();

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

        k1 = new HumanKey("w", "s", "a", "d");
		if(PersistentScript.language == Language.french){
			p1.Part2 = "Humain";
			p1.Part1 = "DJ Guylaine grosse-soirée";
			p2.Part1 = "DJ Jerry Ox";
		} else {
			p1.Part2 = "Human";
			p1.Part1 = "DJ Big Bash Bertha";
			p2.Part1 = "DJ Jerry Ox";
		}


		if (isAIMode()) {

			switch(getAiDifficulty()) {
			case 0:
				k2 = new RandomIA();
				if(PersistentScript.language == Language.french) p2.Part2 = "AI poche";
				else p2.Part2 = "bad AI";
				break;
			case 1:
				k2 = new PerfectIA(PersistentScript.songBPM, p2, p1, 0.9f);
				if(PersistentScript.language == Language.french)p2.Part2 = "AI pas pire";
				p2.Part2 = "not so bad AI";
				break;
            case 2:
                k2 = new HeatMapIA(PersistentScript.songBPM, p2, p1, 0.9f);
				if(PersistentScript.language == Language.french)p2.Part2 = "AI nice";
				p2.Part2 = "Sweet AI";
				break;
			}


		} else {
			k2 = new HumanKey (KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
			if(PersistentScript.language == Language.french) p2.Part2 = "Humain";
			else p2.Part2 = "Human";
		}
    }
	
	// Update is called once per frame
    void Update()
    { 
		// @TODO
        if (k1.GetKeys(BeatEnum.Up))
        {
            BeatManagerRef.setInputP1(0);
        }
        if (k1.GetKeys(BeatEnum.Left))
        {
            BeatManagerRef.setInputP1(1);
        }
        if (k1.GetKeys(BeatEnum.Down))
        {
            BeatManagerRef.setInputP1(2);
        }
        if (k1.GetKeys(BeatEnum.Right))
        {
            BeatManagerRef.setInputP1(3);
        }

        if (k2.GetKeys(BeatEnum.Up))
        {
            BeatManagerRef.setInputP2(0);
        }
        if (k2.GetKeys(BeatEnum.Left))
        {
            BeatManagerRef.setInputP2(1);
        }
        if (k2.GetKeys(BeatEnum.Down))
        {
            BeatManagerRef.setInputP2(2);
        }
        if (k2.GetKeys(BeatEnum.Right))
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


		if(showEnding) {
			if(PersistentScript.language == Language.french){
				if(GUI.Button (new Rect((Screen.width - 100) / 2.0f, (Screen.height - 100) / 2.0f - 160, 100, 100), "Menu principal", GUI.skin.GetStyle ("Return to menu"))) {
					Application.LoadLevel(0);
				}
			} else {
				if(GUI.Button (new Rect((Screen.width - 100) / 2.0f, (Screen.height - 100) / 2.0f - 160, 100, 100), "Main Menu", GUI.skin.GetStyle ("Return to menu"))) {
					Application.LoadLevel(0);
				}
			}

		}
	}

    public void requestBeat(IBeatReceiver b)
    {
        beats.Add(b);
    }

    public void requestUpdate(IUpdate b)
    {
        component.Add(b);
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

			if(PersistentScript.language == Language.french) {
				p1.Part2 = "a gagné!";
				p2.Part2 = "a gagné!";
			} else {
				p1.Part2 = "has won!";
				p2.Part2 = "has won!";
			}
            
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

	public int getAiDifficulty() {
		return PersistentScript.OptAiDifficulty;
	}
    public int getZombieCount()
    {
        return PersistentScript.OptZombiesCount;
    }
    public int getSpeedTurn()
    {
        return PersistentScript.OptSpeedTurn;
    }
    public int getObjScore()
    {
        return PersistentScript.OptBaseScore*PersistentScript.OptZombiesCount;
    }
    public int getZombieCtrl()
    {
        return PersistentScript.OptZombieCtrl;
    }
    public bool isFeverMode()
    {
        return PersistentScript.OptFeverMode;
    }
    public bool isChaosMode()
    {
        return PersistentScript.OptChaosMode;
    }
    public bool isTacticMode()
    {
        return PersistentScript.OptTacticMode;
    }
    public bool isAIMode()
    {
        return PersistentScript.OptAiMode;
    }


}
