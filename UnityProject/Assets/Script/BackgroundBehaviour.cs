using UnityEngine;
using System.Collections.Generic;

public class BackgroundBehaviour : MonoBehaviour, IBeatReceiver {
	public List<Sprite> frames = new List<Sprite>();
	public int framesUntilAnim = 2;

	private int i = 0;
	private int currentFrame = 0;
	private SpriteRenderer[] sRenderers;

	// Use this for initialization
	void Start () {
		GameManager.Instance.requestBeat (this);
		sRenderers = GetComponentsInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Refresh frames in all children
		foreach (SpriteRenderer r in sRenderers) {
			r.sprite = frames[currentFrame];
		}
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
}
