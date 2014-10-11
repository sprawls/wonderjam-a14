using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorScript : MonoBehaviour, IBeatReceiver {

    MeshRenderer rend;
    public int pos = 2;

    public int switch_pos = 37;
    Queue<int> colored = new Queue<int>();

    BeatEnum dir = BeatEnum.Missed;
    bool want_switch = false;
    bool curr = true;

	// Use this for initialization
	void Start () {
        //GameManager.Instance.requestBeat(this); //MARTIN FAIT MARCHE RCE TRUC LA DEMIAN MATIN
        rend = GetComponentInChildren<MeshRenderer>();
	}

    public void OnQuarterBeat()
    { 
    }   

    public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1)
    {
        Debug.Log("DEB");
        if(turnP1)
        {
            dir = p1;
            if(curr)
            {
                Debug.Log("beyond start");
                continuetogototheinfinityandbeyong(p1);
                Debug.Log("Beyond end");
            }
            else
            {
                want_switch = true;

            }
        }
        else
        {
            dir = p2;
            if(!curr)
            {
                continuetogototheinfinityandbeyong(p2);
            }
            else
            {
                want_switch = true;
            }
        }
        curr = turnP1;
    }

    private void continuetogototheinfinityandbeyong(BeatEnum p)
    {
        
        if(p == BeatEnum.Missed)
        {
            if (colored.Count > 0)
            {
                int torem = colored.Dequeue();
                GameManager.Instance.getTile(torem).setColor(0);
            }
        }
        else 
        {
            switch (p)
            {
                case BeatEnum.Up:
                    pos --;
                    break;
                case BeatEnum.Down:
                    pos ++;
                    break;
                case BeatEnum.Right:
                    pos+=5;
                    break;
                case BeatEnum.Left:
                    pos-=5;
                    break;
            }
            Debug.Log(pos);
            colored.Enqueue(pos);
            GameManager.Instance.getTile(pos).setColor(curr ? 1 : 2);
        }
    }

    void LateUpdate()
    {
        if(want_switch)
        {
            switchCursor();
            want_switch = false;
        }
        else 
        {
            var p = GameManager.Instance.getTile(pos).transform.position;
            //if(p != transform.position)
            {
                transform.position = p;
            }
        }
    }

    void switchCursor()
    {

    }
}
