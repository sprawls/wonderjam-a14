﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieBehaviour : MonoBehaviour {

	public List<Sprite> spriteList;
	public float xPosition; //Size of the board on the scene on X
	public float yPosition; //Size of the board on the scene on Y
	public bool isOnBeat = false;
	public int currentType; //0 : neutral, 1 : P1 , 2 :P2

	private bool isUp = true; // is the zombie in up stance (while dancing)
	private bool isRight = true; //is the zombie facing right (while dancing)
	private float randomWeight = 1.5f; //Weight of the mouvement
	private float playerWeight = 3.5f; //Weight of the mouvment
	private float xBoardSize = 78; //Board from 0 to this on x Axis
	private float yBoardSize = 48; //Board from 0 to this on y Axis
	private BeatEnum mainInput;
	private BeatEnum otherInput;
	private SpriteRenderer sprRenderer;

	public void Start(){
		sprRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
		if(Random.Range (0,100) > 50) isUp = !isUp; //random change to start animation
		if(Random.Range (0,100) > 50) isRight = !isRight; //random change tofacing 

		transform.position = new Vector2(0f,0f);
		xPosition = Random.Range (0, xBoardSize);
		yPosition = Random.Range (0, yBoardSize);
		MoveToPosition();
	}

	public void LateUpdate() {
		if(isOnBeat == true) {
			//Calculate Mouvement
			Vector2 playerChange = CreatePlayerChange(otherInput);
			playerChange *= playerWeight;
			Vector2 randomChange = new Vector2(Random.Range(-1f,1f), Random.Range (-1f,1f)).normalized;
			randomChange *= randomWeight;
			Vector2 ResultChange = playerChange + randomChange;

			//Debug.Log ("pChange : " + playerChange + "   rChange " + randomChange + "   tChange " + ResultChange);

			//Execute Mouvment
			ChangePosition(ResultChange); //Change Position on grid
			MoveToPosition (); //Move and Clamp

			//Update Sprite
			UpdateSprite();

			isOnBeat = false;
		}
	}

	private void  UpdateSprite(){
		//Update  Sprite
		if(currentType == 0){
			if(isUp == true) sprRenderer.sprite = spriteList[0];
			else sprRenderer.sprite = spriteList[1];
		} else if(currentType == 1){
			if(isUp == true) sprRenderer.sprite = spriteList[2];
			else sprRenderer.sprite = spriteList[3];
		} else {
			if(isUp == true) sprRenderer.sprite = spriteList[4];
			else sprRenderer.sprite = spriteList[5];
		}
		//Update Facing
		if(isRight == true) {
			sprRenderer.transform.localScale = new Vector3(1,1,1);
		} else {
			sprRenderer.transform.localScale = new Vector3(-1,1,1);
		}
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2) { 
		isUp = !isUp;
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

	public void ChangeType(int newType) {
		currentType = newType;
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
