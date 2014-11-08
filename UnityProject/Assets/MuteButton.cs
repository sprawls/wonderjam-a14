using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	bool isOn = true;

	public Sprite on;
	public Sprite off;
	public AudioSource audioToMute;

	Color normalColor;
	Color hoverColor; 
	Color onColor;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();

		normalColor = new Color (52f/255f,219f/255f,122f/255f);
		hoverColor = new Color (0f/255f,99f/255f,235f/255f); 
		onColor = new Color (1, 1, 1);

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		spriteRenderer.color = hoverColor;
	}

	void OnMouseExit() {
		if(isOn) {
			spriteRenderer.color = onColor;
		} else {
			spriteRenderer.color = normalColor;
		}
	}

	void OnMouseDown() {
		isOn = !isOn;
		if (isOn) {
			spriteRenderer.color = onColor;
			spriteRenderer.sprite = on;
			audioToMute.volume = 1;
		} else {
			spriteRenderer.color = normalColor;
			spriteRenderer.sprite = off;
			audioToMute.volume = 0;
		}
	}
}
