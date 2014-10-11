﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {
	private BeatManager bm;
	private bool createNewGem = false;
	private List<Transform> gems = new List<Transform>();
	private float bpm;

	public GUISkin gSkin;
	public int paddingZeroes = 7;
	public int score = 0;
	public Transform gemPrefab;
	public float xLimit;

	// Use this for initialization
	void Start () {
		bm = BeatManager.Instance;
	}

	// Update is called once per frame
	void Update () {
		// Create new gem at end of line
		if (createNewGem) {
			Transform newGem = Transform.Instantiate(gemPrefab) as Transform;
			newGem.parent = this.transform;
			newGem.position = new Vector3(0, 0, 0);
			gems.Add (newGem);

			createNewGem = false;
		}
		
		foreach (Transform g in gems) {
			// Move all gems towards stromatolite
			g.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
			
			// Destroy gems on stromatolite
			if (g.position.x > xLimit) {
				Destroy(g);
			}
		}
	}

    public void OnQuarterBeat()
    {

    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		bpm = bm.interval;
		createNewGem = true;
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));
	}
}
