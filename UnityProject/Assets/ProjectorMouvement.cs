using UnityEngine;
using System.Collections;

public class ProjectorMouvement : MonoBehaviour {

	/// <summary>
	/// QUATERNIONS NEEDED ! ABORT !
	/// </summary>

	public float maxChangeX = -45f;
	public float minChangeX = 45f;
	public float maxChangeY = 45f;
	public float minChangeY = 0f;

	public float time = 2f;

	private bool isMoving = false;
	private float CurveX;
	private float CurveY;
	private Vector3 startingRotation;

	void Start () {
		startingRotation = new Vector3(transform.localRotation.eulerAngles.x,
		                               transform.localRotation.eulerAngles.y, 
		                               transform.localRotation.eulerAngles.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isMoving == false){
			StartCoroutine(Move ());
		}
	}

	public IEnumerator Move() {
		isMoving = true; //Activate Moving Flag
		Vector2 p0 = new Vector2 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.z);
		Vector2 p1 = FindTargetRotation();
		Vector2 c0 = new Vector2 (p0.x + ((p1.x - p0.x) / Random.Range (1f, 5f)) + Mathf.Abs (Random.Range (1f,3f)), p0.y + ((p1.y - p0.y) / Random.Range (1f, 5f)) + Mathf.Abs (Random.Range (1f,3f)));

		float step = 0f; //raw step
		float rate = 1f/time; //amount to add to raw step
		
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; //Add time to step
			//Find Curve X and Y
			CurveX = (((1-step)*(1-step)) * p0.x) + (2 * step * (1 - step) * c0.x) + ((step * step) * p1.x);
			CurveY = (((1-step)*(1-step)) * p0.y) + (2 * step * (1 - step) * c0.y) + ((step * step) * p1.y);
			Vector3 newRotation = startingRotation + new Vector3(CurveX,0, CurveY);
			transform.localRotation = Quaternion.Euler(newRotation);
			yield return null;
			
		}
		isMoving = false; //Deactivate Moving Flag
	}

	private Vector2 FindTargetRotation() {
		Vector2 rotation = new Vector2 (startingRotation.x, startingRotation.z);
		rotation += new Vector2 (Random.Range (minChangeX, maxChangeX), Random.Range (minChangeY, maxChangeY));
		return(rotation);
		
	}


}
