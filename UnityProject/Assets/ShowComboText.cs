using UnityEngine;
using System.Collections;

public class ShowComboText : MonoBehaviour {

	public bool isP1 = true;

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
	}
}
