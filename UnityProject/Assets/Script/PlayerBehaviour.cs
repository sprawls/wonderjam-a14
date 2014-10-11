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

	public GUISkin gSkin;

    public string Part1;
    public string Part2;

	public int paddingZeroes = 7;   
	public int score = 0;
	public float zLimit = 100;
	public float speed;
	public Vector3 gemSpawningPoint;

	public Transform gemPrefab;
	public Transform neutralGemPrefab;
	
	// Use this for initialization
	void Start () {
		bm = BeatManager.Instance;
		bpm = bm.interval;
		GameManager.Instance.requestBeat (this);
		tag = gameObject.tag;

		totalDistance = Math.Abs(gemSpawningPoint.z - zLimit);
	}

	// Update is called once per frame
	void Update () {
		aboutToSwitch = bm.aboutToSwitch;

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
			float speed = (bpm * totalDistance * Time.deltaTime)/240;
			if (gameObject.tag == "Player2") speed *= -1;

			g.Translate(new Vector3(0, 0, speed));
			
			// Destroy gems on stromatolite
			if (Mathf.Abs(g.position.z - zLimit) < 0.5 ) {
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
		turnP1 = turnP1;

		createActiveGem = (((turnP1 && !aboutToSwitch) || (!turnP1 && aboutToSwitch)) && tag == "Player1") || (((!turnP1 && !aboutToSwitch) || (turnP1 && aboutToSwitch)) && tag == "Player2");
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));

        Vector3 v = transform.position;
       
        Vector3 start = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(v);
        start.x += 450 * (whoami() == 1 ? -1 : 1) - 80;
        start.y += 50;
        GUI.TextArea(new Rect(start.x, start.y, 80, 80), Part1, GUI.skin.GetStyle("score"));
        start.y += 50;
        GUI.TextArea(new Rect(start.x, start.y, 80, 80), Part2, GUI.skin.GetStyle("score"));


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
