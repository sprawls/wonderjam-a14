using UnityEngine;
using System.Collections;

public class ZombieBehaviour : MonoBehaviour {
	
	public float xPosition; //Size of the board on the scene on X
	public float yPosition; //Size of the board on the scene on Y
	public bool isOnBeat = false;

	private float xBoardSize = 78; //Board from 0 to this on x Axis
	private float yBoardSize = 48; //Board from 0 to this on y Axis

	private BeatEnum mainInput;
	private BeatEnum otherInput;

	public void Start(){
		transform.position = new Vector2(0f,0f);
		xPosition = Random.Range (0, xBoardSize);
		yPosition = Random.Range (0, yBoardSize);
		MoveToPosition();
	}

	public void LateUpdate() {
		if(isOnBeat == true) {
			Vector2 playerChange = CreatePlayerChange(otherInput);

			Vector2 randomChange = new Vector2(Random.Range(-1f,1f), Random.Range (-1f,1f)).normalized;
			Vector2 ResultChange = playerChange + randomChange;

			//Debug.Log ("pChange : " + playerChange + "   rChange " + randomChange + "   tChange " + ResultChange);
			
			ChangePosition(ResultChange); //Change Position on grid
			MoveToPosition (); //Move and Clamp
			isOnBeat = false;
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2) { 
		mainInput = p1;
		otherInput = p2;
		isOnBeat = true;
	}

	Vector2 CreatePlayerChange(BeatEnum pInput){
		if(pInput == BeatEnum.Missed) {
			return Vector2.zero;
		} else if(pInput == BeatEnum.Right) {
			return Vector2.right;
		} else if(pInput == BeatEnum.Left) {
			return -Vector2.right;
		} else if(pInput == BeatEnum.Down) {
			return -Vector2.up;
		} else {
			return Vector2.up;
		}
	}

	void ChangePosition(Vector2 mouvement){
		xPosition += mouvement.x;
		yPosition += mouvement.y;
		xPosition = Mathf.Clamp(xPosition, 0, xBoardSize);
		yPosition = Mathf.Clamp(yPosition, 0, yBoardSize);
	}

	public void MoveToPosition() { //Move to position of current
		//transform.localPosition = new Vector2(xPosition,yPosition,1f/2.033f);
		StartCoroutine (PlaySmoothAnimation(transform.localPosition,new Vector2(xPosition,yPosition),1f/2.033f));
	}

	//Coroutine that smooths the mouvement animation
	IEnumerator PlaySmoothAnimation(Vector3 startingPosition, Vector3 endingPosition, float time) {
		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = Mathf.SmoothStep(0f, 1f, step); // finding smooth step
			transform.localPosition = new Vector3 (Mathf.Lerp(startingPosition.x, endingPosition.x, (smoothStep)),
			                                       Mathf.Lerp(startingPosition.y, endingPosition.y, (smoothStep)),
			                                       Mathf.Lerp(startingPosition.z, endingPosition.z, (smoothStep))); //lerp position
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
		//complete rotation
		if(step > 1.0) transform.localPosition = new Vector3 (Mathf.Lerp(startingPosition.x, endingPosition.x, (1f )),
		                                                      Mathf.Lerp(startingPosition.y, endingPosition.y, (1f )),
		                                                      Mathf.Lerp(startingPosition.z, endingPosition.z, (1f )));
	}


}
