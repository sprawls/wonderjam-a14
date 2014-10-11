using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour, IBeatReceiver {
	public List<Sprite> frames = new List<Sprite>();
	public int framesUntilAnim = 2;

	private int i = 0;
	private int currentFrame = 0;
	private SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
		sRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		if (i < framesUntilAnim)
			i++;

		// Switch sprite frame
		else { 
			i = 0;
			currentFrame++;
			if (currentFrame == frames.Count) currentFrame = 0;
			sRenderer.sprite = frames[currentFrame];
			Debug.Log ("Current frame : " + currentFrame);
		}
	}
}
