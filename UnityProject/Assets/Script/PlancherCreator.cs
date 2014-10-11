using UnityEngine;
using System.Collections;

public class PlancherCreator : MonoBehaviour {

    public GameObject black;
    public GameObject white;

	// Use this for initialization
	void Start () {
        float x = 0;
        float y = 0;
        float z = 0;

        GameManager gm = FindObjectOfType<GameManager>();

        for(int i = 0; i < 40; i++)
        {
            GameObject go = Instantiate(i % 2 ==0 ? black : white) as GameObject;
            TileAnimation ta = go.GetComponent<TileAnimation>();
            gm.requestBeat(ta);
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(x, y, z);
            
            x += 10;
            if(x > 40)
            {
                x = 0;
                z += 10;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
