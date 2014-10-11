using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorScript : MonoBehaviour, IBeatReceiver 
{

    MeshRenderer rend;
    public int pos = 2;
    public bool player;

    Queue<int> colored = new Queue<int>();

    bool curr = true;

    delegate void UpdateDelegate();
    Queue<UpdateDelegate> update = new Queue<UpdateDelegate>();
    

	// Use this for initialization
	void Start () {
        GameManager.Instance.requestBeat(this); //MARTIN FAIT MARCHE RCE TRUC LA DEMIAN MATIN
        rend = GetComponentInChildren<MeshRenderer>();
	}

    public void OnQuarterBeat()
    { 
    }   

    public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1)
    {
        update.Enqueue(
            delegate()
            {
                beat_work(p1, p2, turnP1);
            });
    }

    private void beat_work(BeatEnum snake, BeatEnum other, bool turnP1)
    {
        bool isMe = turnP1 == player;
        
        if (curr & isMe)
        {
            continuetogototheinfinityandbeyong(snake);
        }
        else if (curr & !isMe)
        {
            revoke_access();
        }
        else if (!curr & isMe)
        {
            gain_access(snake);
        }
  
    }

    private void continuetogototheinfinityandbeyong(BeatEnum p)
    {
        if(p == BeatEnum.Missed)
        {
            if (colored.Count > 0)
            {
                int torem = colored.Dequeue();
                GameManager.Instance.getTile(torem).color = 0;
            }
        }
        else 
        {
            int old = pos;
            int n = pos;
            switch (p)
            {
                case BeatEnum.Up:
                    if (old % 5 != 0)
                    {
                        n = old - 1;
                    }
                    break;
                case BeatEnum.Down:
                    if (old % 5 != 4)
                    {
                        n = old+1;
                    }
                    break;
                case BeatEnum.Right:
                    if (old < 35)
                    {
                        n = old + 5;
                    }
                    break;
                case BeatEnum.Left:
                    if (old > 4)
                    {
                        n = old - 5;
                    }
                    break;
            }
            pos = n;
            Debug.Log(pos);
            if (old != pos)
            {
                EnqueueCurrent();
            }
        }
    }

    void LateUpdate()
    {
        while(update.Count != 0)
        {
            update.Dequeue()();
        }
        var p = GameManager.Instance.getTile(pos).transform.position;
        if(p != transform.position)
        {
            transform.position = new Vector3(p.x,transform.position.y,p.z);
        }
    }

    private void gain_access(BeatEnum p)
    {
        Debug.Log("Gain" + (player?"1":"2"));
        curr = true;
        EnqueueCurrent();
        continuetogototheinfinityandbeyong(p);
    }

    private void revoke_access()
    {
        curr = false;
        int cur_col = player?1:2;
        int[] tile = colored.ToArray();


        foreach (var t in tile)
        {
            var ta = GameManager.Instance.getTile(t);
            if (ta.color == cur_col)
            {
                ta.color = 0;
            }
        }
        ZombieFactory.Instance.OnPeinture(tile, cur_col);
        colored = new Queue<int>();
    }

    private void EnqueueCurrent()
    {
        colored.Enqueue(pos);
        GameManager.Instance.getTile(pos).color = player ? 1 : 2;
    }
}
