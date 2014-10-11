using UnityEngine;
using System.Collections;

public class RandomMouvementInArea : MonoBehaviour {

	public Vector2 aeraToMoveIn;
	public float time;
	
	private Vector3 startingPosition;
	private float CurveX;
	private float CurveY;
	
	public bool isMoving = false;
	
	
	// Use this for initialization
	void Start () {
		startingPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(isMoving == false){
			StartCoroutine(Move ());
		}
	}
	
	
	public IEnumerator Move() {
		isMoving = true; //Activate Moving Flag
		Vector2 p0 = new Vector2 (transform.localPosition.x, transform.localPosition.y);
		Vector2 p1 = FindTargetDestination();
		Vector2 c0 = new Vector2 (p0.x + ((p1.x - p0.x) / Random.Range (1f, 5f)) + Mathf.Abs (Random.Range (1f,3f)), p0.y + ((p1.y - p0.y) / Random.Range (1f, 5f)) + Mathf.Abs (Random.Range (1f,3f)));
		//Debug.Log ("p0 : " + p0 + "   p1 : " + p1 + "   c0 : " + c0);
		
		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		
		
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; //Add time to step
			//Find Curve X and Y
			CurveX = (((1-step)*(1-step)) * p0.x) + (2 * step * (1 - step) * c0.x) + ((step * step) * p1.x);
			CurveY = (((1-step)*(1-step)) * p0.y) + (2 * step * (1 - step) * c0.y) + ((step * step) * p1.y);
			transform.localPosition = startingPosition + new Vector3(CurveX,CurveY,0 );
			yield return null;
			
		}

		isMoving = false; //Deactivate Moving Flag
	}
	
	private Vector2 FindTargetDestination() {
		Vector2 Destination = new Vector2 (startingPosition.x, startingPosition.z);
		Destination += new Vector2 (Random.Range (0, aeraToMoveIn.x), Random.Range (-4*aeraToMoveIn.y/2f, 2*aeraToMoveIn.y/2f));
		return(Destination);
		
	}

}
