using UnityEngine;
using System.Collections;

public class AnimateOnLanguage : MonoBehaviour {

	Persistent persistentScript;
	public Language targetLanguage;

	private bool isAnimated = false;
	private TextMesh myText;

	void Start () {
		myText = gameObject.GetComponent<TextMesh> ();
		persistentScript = GameObject.Find("persistent").GetComponent("Persistent") as Persistent;
		Unanimate ();
	}
	
	// Update is called once per frame
	void Update () {
		if(persistentScript.language == targetLanguage && isAnimated == false) {
			Animate();
		} else if(persistentScript.language != targetLanguage && isAnimated == true) {
			Unanimate();
		}

	}

	void Animate() {
		isAnimated = true;
		myText.color = new Color(52f/255f,219f/255f,122f/255f);
	}

	void Unanimate() {
		isAnimated = false;
		myText.color = new Color (111f/255f,111f/255f,111f/255f);

	}
}
