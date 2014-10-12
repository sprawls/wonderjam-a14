using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerBehaviour : MonoBehaviour, IBeatReceiver {

	private BeatManager bm;
	private bool createNewGem = false;
	private bool createActiveGem = false;
	private bool aboutToSwitch = false;
	private List<Transform> gems = new List<Transform>();
	private List<int> gemsToDestroy = new List<int> ();
	private float bpm;
	private float totalDistance;
	private bool turnP1 = false;
	private string tag;

	//Fever
	private bool removeFever = false; //If True , remove fever
	private bool CheckFever = false; //If true , check for fever
	private bool FeverStartedP1 = false;
	private bool FeverStartedP2 = false;
	private int FeverThreshold = 20;

	public GUISkin gSkin;
    public GUISkin gName;

    public string Part1;
    public string Part2;

	public int paddingZeroes = 7;   
	public int score = 0;
	public float zLimit = 100;
	public float speed;
	public Vector3 gemSpawningPoint;

	public CursorScript cursorP1;
	public CursorScript cursorP2;
	public Transform gemPrefab;
	public Transform neutralGemPrefab;
	//Player score 
	public float P1Combo = 0;
	public float P2Combo = 0;

	
	// Use this for initialization
	void Start () {
		bm = BeatManager.Instance;
		GameManager.Instance.requestBeat (this);

		tag = gameObject.tag;
		totalDistance = Math.Abs(gemSpawningPoint.z - zLimit);

		//Get Cursors
		cursorP1 = GameObject.Find ("Cursor").GetComponent<CursorScript>();
		cursorP2 = GameObject.Find ("CursorP2").GetComponent<CursorScript>();
	}

	// Update is called once per frame
	void Update () {
		bpm = bm.interval;
		aboutToSwitch = bm.aboutToSwitch;
		//Fever 
		if(CheckFever == true) {
			CheckForFever();
			CheckFever = false;
		}
		if(removeFever == true) {
			if(P1Combo < 15) RemoveFever(1);
			if(P2Combo < 15) RemoveFever(2);
			removeFever = false;
		}
		// Create new gem at end of line
		if (createNewGem) {
			Transform newGem;
			if (createActiveGem)
				newGem = Transform.Instantiate(gemPrefab) as Transform;
			else
				newGem = Transform.Instantiate(neutralGemPrefab) as Transform;

			newGem.parent = this.transform;
			newGem.position = gemSpawningPoint;
			gems.Add (newGem);

			createNewGem = false;
		}

		foreach (Transform g in gems) {
			// Move all gems towards stromatolite
			float speed = (bpm * totalDistance * Time.deltaTime)/480;
			if (tag == "Player2") speed *= -1;

			g.Translate(new Vector3(0, 0, speed));
			
			// Destroy gems on stromatolite
			if ((tag == "Player1" && (g.position.z - zLimit) > 0) || (tag == "Player2" && (g.position.z - zLimit) < 0)) {
				gemsToDestroy.Add (gems.IndexOf (g));
			}
		}

		foreach (int i in gemsToDestroy) {
			Transform g = gems[i];
			gems.RemoveAt(i);
			Destroy(g.gameObject);
		}
		gemsToDestroy.Clear ();
	}

    public void OnQuarterBeat() {}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		createNewGem = true;
		this.turnP1 = turnP1;
		createActiveGem = ((turnP1 ^ aboutToSwitch) && tag == "Player1") || ((turnP1 == aboutToSwitch) && tag == "Player2");
		CheckFever = true;
		//Look For P1 Combo
		if(turnP1 && p1 != BeatEnum.Missed){
			if(p1 != BeatEnum.Empty) P1Combo ++;
		} else if(!turnP1 && p2 != BeatEnum.Missed) {
			if(p1 != BeatEnum.Empty) P1Combo ++;
		} else {
			P1Combo = 0;
			FeverStartedP1 = false;
			removeFever = true;
		}
		//Look For P2 Combo
		if(turnP1 && p2 != BeatEnum.Missed){
			if(p2 != BeatEnum.Empty) P2Combo ++;
		} else if(!turnP1 && p1 != BeatEnum.Missed) {
			if(p2 != BeatEnum.Empty) P2Combo ++;
		} else {
			P2Combo = 0;
			FeverStartedP2 = false;
			removeFever = true;
		}
	
	}

	void RemoveFever(int player){
		if(player == 1) cursorP1.calmdown();
		else if(player == 2) cursorP2.calmdown();
	}

	void CheckForFever(){
		//Look to Start Stromatolite Fever
		if(cursorP1.superpower == false && FeverStartedP1 == false && P1Combo >= FeverThreshold) {
			cursorP1.POWERUP();
			FeverStartedP1 = true;
		}
		if(cursorP2.superpower == false && FeverStartedP2 == false && P2Combo >= FeverThreshold) {
			cursorP2.POWERUP();
			FeverStartedP2 = true;
		}
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));

        GUI.skin = gName;
        Vector3 v = transform.position;
       
        Vector3 start = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(v);
        start.x += 380 * (whoami() == 1 ? -1 : 1) - 70;
        start.y += 50;

        GUI.TextArea(new Rect(start.x, start.y, 400, 80), Part1, GUI.skin.textArea);
        start.y += 80;
        GUI.TextArea(new Rect(start.x, start.y, 400, 80), Part2, GUI.skin.textArea);
	}

    int whoami()
    {
        if (tag == "Player1")
        {
            return 1;

        }
        else
        {
            return 2;
        }
    }

	public int Score {
		get {
			return this.score;
		}
		set {
			if (!GameManager.Instance.IsAnimating && !GameManager.Instance.Finished)
				this.score = value;
		}
	}
}
