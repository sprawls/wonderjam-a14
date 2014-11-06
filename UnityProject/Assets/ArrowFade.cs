using UnityEngine;
using System.Collections;

public class ArrowFade : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	float fadeTime = 1f;
	public int orientation = 0; //0 : right, 1: left, 2: up, 3: down

	public Sprite Arrow;
	public Sprite Error;

	void Awake() {
		spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
	}

	void Start () {

		StartCoroutine (FadeAway ());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeOrientation (int newO) {
		orientation = newO;
		if (orientation == 0) {
			spriteRenderer.sprite = Arrow;
			spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,	90, 0));
		} else if (orientation == 1) {
			spriteRenderer.sprite = Arrow;
			spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,	90, 180));
		} else if (orientation == 2) {
			spriteRenderer.sprite = Arrow;
			spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,	90, 90));
		} else if (orientation == 3) {
			spriteRenderer.sprite = Arrow;
			spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,	90, 270));
		} else {
			spriteRenderer.sprite = Error;
			spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,	90, 0));
		}
	}

	IEnumerator FadeAway(){
		Color StartColor = spriteRenderer.color;
		Color EndColor = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0);
		for (float i = 0; i < 1; i+= Time.deltaTime/fadeTime){
			spriteRenderer.color = Color.Lerp(StartColor,EndColor,i);
			yield return null;
		}
		Destroy (gameObject);
	}
}
