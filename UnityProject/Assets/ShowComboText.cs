using UnityEngine;
using System.Collections;

public class ShowComboText : MonoBehaviour, IBeatReceiver  {

	public bool isP1 = true;

	private bool BeatSprite = false;
	private TextMesh myText;
	private PlayerBehaviour player1;

	void Start () {
		myText = GetComponentInChildren<TextMesh>();
		player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isP1) myText.text = (player1.P1Combo).ToString();
		else myText.text = (player1.P2Combo).ToString ();
		if(BeatSprite == true) {
			BeatSprite = false;
			StartCoroutine (BeatUpAndDown());
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		BeatSprite = true;
	}
	public void OnQuarterBeat(){}

	public IEnumerator BeatUpAndDown(){
		StartCoroutine (BeatTheSprite(0.05f,1.25f));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine (BeatTheSprite(0.05f,1f));
	}
	
	public IEnumerator BeatTheSprite(float time, float targetScale){
		Vector3 startScale = transform.localScale;
		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = Mathf.SmoothStep(0f, 1f, step);// finding smooth step
			transform.localScale = new Vector3 (Mathf.Lerp(startScale.x, targetScale, (smoothStep)),
			                                    Mathf.Lerp(startScale.y, targetScale, (smoothStep)),
			                                    Mathf.Lerp(startScale.z, targetScale, (smoothStep))); //lerp position
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
		//complete rotation
		if(step > 1.0) transform.localScale = new Vector3 (Mathf.Lerp(startScale.x, targetScale, (1)),
		                                                   Mathf.Lerp(startScale.y, targetScale, (1)),
		                                                   Mathf.Lerp(startScale.z, targetScale, (1))); 
	}
}
