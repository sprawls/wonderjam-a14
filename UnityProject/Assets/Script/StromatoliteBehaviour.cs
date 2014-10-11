using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StromatoliteBehaviour : MonoBehaviour, IBeatReceiver {
	private float bpm;
	private BeatManager bm;
	private bool createNewGem = false;
	private bool BeatSprite = false;
	private List<Transform> gems = new List<Transform>();

	public Transform gemPrefab;
	public float xLimit = 100;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.requestBeat (this);
		bm = BeatManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		// Create new gem at end of line
		if (createNewGem) {
			gems.Add (GameObject.Instantiate (gemPrefab) as Transform);
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
		//BEat the stromatolite sprite if needed
		if(BeatSprite == true) {
			StartCoroutine (BeatUpAndDown());
			BeatSprite = false;
		}

	}

    public void OnQuarterBeat()
    {
    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		bpm = bm.interval;
		createNewGem = true;
		BeatSprite = true;
	}
	public IEnumerator BeatUpAndDown(){
		StartCoroutine (BeatTheSprite(0.2f,1.4f));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine (BeatTheSprite(0.2f,1.4f));
	}

	public IEnumerator BeatTheSprite(float time, float targetScale){
		Vector3 startScale = transform.localScale;
		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = step; // finding smooth step
			transform.localScale = new Vector3 (Mathf.Lerp(startScale.x, targetScale, (smoothStep)),
			                                    Mathf.Lerp(startScale.y, targetScale, (smoothStep)),
			                                    Mathf.Lerp(startScale.z, targetScale, (smoothStep))); //lerp position
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
		//complete rotation
		if(step > 1.0) transform.localPosition = new Vector3 (Mathf.Lerp(startScale.x, targetScale, (1)),
		                                                      Mathf.Lerp(startScale.y, targetScale, (1)),
		                                                      Mathf.Lerp(startScale.z, targetScale, (1))); 
	}

}
