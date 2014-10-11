using UnityEngine;
using System.Collections;

public class TileAnimation: MonoBehaviour, IBeatReceiver{

    public Material[] mats_base;
    public Material[] mats_bleu;
    public Material[] mats_vert;

    bool changeUpdate = false;

    private int _state;
    public int color
    {
        get { return _state; }
        set
        {
            if (value >= 0 && value <= 2)
            {
                _state = value;
            }
        }
    }

    private int number = 0;

    private MeshRenderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponentInParent<MeshRenderer>();
	}

    public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1)
    {
        changeUpdate = true;
    }

    void LateUpdate()
    {
        if (changeUpdate)
        {
            int i = number % 4;
            number++;
            //i = Random.Range(0, 4);
            switch (color)
            {
                case 0:
                    rend.material = mats_base[i];
                    break;
                case 1:
                    rend.material = mats_vert[i];
                    break;
                case 2:
                    rend.material = mats_bleu[i];
                    break;
            }
            changeUpdate = false;
        }
    }
}
