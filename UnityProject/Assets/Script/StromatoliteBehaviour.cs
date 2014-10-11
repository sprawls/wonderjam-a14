using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StromatoliteBehaviour : MonoBehaviour, IBeatReceiver {

	private bool BeatSprite = false;
	private Light flashinLight;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.requestBeat (this);
		flashinLight = (Light) GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
		BeatSprite = true;
	}
	public IEnumerator BeatUpAndDown(){
		StartCoroutine (BeatTheSprite(0.05f,1.25f));
		StartCoroutine (FlashTheLight(0.05f,0.8f));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine (BeatTheSprite(0.05f,1f));
		StartCoroutine (FlashTheLight(0.05f,0.2f));
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

	public IEnumerator FlashTheLight(float time, float targetIntensity){
		float startingIntensity = flashinLight.intensity;
		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = Mathf.SmoothStep(0f, 1f, step);// finding smooth step
			flashinLight.intensity = Mathf.Lerp(startingIntensity, targetIntensity, (smoothStep));
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
		//complete rotation
		if(step > 1.0) flashinLight.intensity = Mathf.Lerp(startingIntensity, targetIntensity, (1));
	}

}
