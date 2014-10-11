using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour, IBeatReceiver {
	public List<Sprite> frames = new List<Sprite>();
	public GUISkin gSkin;
	public int framesUntilAnim = 2;
	public int paddingZeroes = 7;
	public int score = 0;

	private int i = 0;
	private int currentFrame = 0;
	private SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
		sRenderer = GetComponent<SpriteRenderer>();
		GameManager.Instance.requestBeat (this);
	}
	
	// Update is called once per frame
	void Update () {
		// Refresh frame
		sRenderer.sprite = frames[currentFrame];
	}

    public void OnQuarterBeat()
    {
    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		if (i < framesUntilAnim)
			i++;

		// Switch sprite frame index
		else { 
			i = 0;
			currentFrame++;
			if (currentFrame == frames.Count) currentFrame = 0;
		}
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));
	}
}
