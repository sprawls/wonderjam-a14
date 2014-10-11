using UnityEngine;
using System.Collections;

public class ZombieBehaviour : MonoBehaviour {
	
	public float xPosition; //Size of the board on the scene on X
	public float yPosition; //Size of the board on the scene on Y

	private float xBoardSize = 16; //Board from 0 to this on x Axis
	private float yBoardSize = 10; //Board from 0 to this on y Axis

	public ZombieBehaviour(){
		transform.position = new Vector2(0f,0f);
		xPosition = Random.Range (0, xBoardSize);
		yPosition = Random.Range (0, yBoardSize);
		MoveToPosition();
	}

	public void OnBeat(BeatEnum mainInput, BeatEnum otherInput) { 
		Vector2 playerChange = CreatePlayerChange(mainInput);
		Vector2 randomChange = new Vector2(Random.Range(-1f,1f), Random.Range (-1f,1f)).normalized;
		Vector2 ResultChange = playerChange + randomChange;

		ChangePosition(ResultChange); //Change Position on grid
		MoveToPosition (); //Move and Clamp

	}

	Vector2 CreatePlayerChange(BeatEnum pInput){
		if(pInput == BeatEnum.Missed) {
			return Vector2.zero;
		} else if(pInput == BeatEnum.Right) {
			return Vector2.right;
		} else if(pInput == BeatEnum.Left) {
			return -Vector2.right;
		} else if(pInput == BeatEnum.Down) {
			return Vector2.up;
		} else {
			return -Vector2.up;
		}
	}

	void ChangePosition(Vector2 mouvement){
		xPosition += mouvement.x;
		yPosition += mouvement.y;
		xPosition = Mathf.Clamp(xPosition, 0, xBoardSize);
		yPosition = Mathf.Clamp(yPosition, 0, yBoardSize);
	}

	public void MoveToPosition() { //Move to position of current
		transform.localScale = new Vector2(xPosition,yPosition);
	}


}
