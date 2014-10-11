using UnityEngine;
using System.Collections;

public class SplashParticles : MonoBehaviour, IBeatReceiver {

	private ParticleSystem splashSystem;
	public bool isTurnP1 = true;
	public bool isP1 = true;

	private bool spawnParticles = false;

	void Awake() {
		splashSystem =  gameObject.GetComponent<ParticleSystem>();
	}

	void Start () {
		GameManager.Instance.requestBeat (this);
	}

	void FixedUpdate () {
		if(spawnParticles == true) {
			splashSystem.Emit(40);
			spawnParticles = false;
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		Debug.Log ("called");
		if(isTurnP1 != turnP1 && isP1 != turnP1) {
			spawnParticles = true;
		}
		isTurnP1 = turnP1;
	}

}
