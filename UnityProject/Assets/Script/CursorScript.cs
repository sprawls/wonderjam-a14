using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorScript : MonoBehaviour, IBeatReceiver 
{

    MeshRenderer rend;
	private LineRenderer linerenderer;
    public int pos = 2;
    public bool player;

    Queue<int> colored = new Queue<int>();

    bool curr = true;

    public bool superpower = false;

    bool needtomove = true;
    int countdownSuper = -1;
    int maxcountdownSuper = 4;

    delegate void UpdateDelegate();
    Queue<UpdateDelegate> update = new Queue<UpdateDelegate>();

    AudioSource fever;

	// Use this for initialization
	void Start () {
        GameManager.Instance.requestBeat(this);
        rend = GetComponentInChildren<MeshRenderer>();
        fever = GetComponentInChildren<AudioSource>();
		linerenderer = GetComponentInChildren<LineRenderer>();
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
            if (superpower)
            {
                countdownSuper--;
				//Debug.Log (gameObject.name + "I am calming down now.");
                if (countdownSuper < 0)
                {
                    calmdown();
                }
            }
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
            //Debug.Log(pos);
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
        
        if(needtomove)
        {
            var p = GameManager.Instance.getTile(pos).transform.position;
            StartCoroutine(MoveBitch(0.1f, new Vector3(p.x,transform.position.y,p.z)));
            needtomove = false;
        }
    }

    private void gain_access(BeatEnum p)
    {
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
        int p = player ? 1 : 2;;
        needtomove = true;
        if (!superpower)
        {
            colored.Enqueue(pos);
            GameManager.Instance.getTile(pos).color = p;
        }
        else
        {
            int posy = pos / 5;
            int posx = pos % 5;
            for (int i = posx -1 ; i < posx+2; i++)
            {
                for (int j = posy-1; j < posy+2; j++)
                {
                    if (i < 0 || i > 4)
                        continue;
                    if(j< 0 || j>8)
                        continue;

                    int index = i + 5 * j;
                    TileAnimation ta = GameManager.Instance.getTile(index);
                    if(ta.color != p)
                    {
                        colored.Enqueue(index);
                        GameManager.Instance.getTile(index).color = p;
                    }                    
                }
            }
        }
    }

    public void POWERUP()
    {
		if(superpower == false) {
            Debug.Log("SUpoerpower og");
			linerenderer.transform.localScale = new Vector3(3, 3, 3); //Make cursor size bigger
			linerenderer.SetWidth(5,5);//Make Line Renderer Bigger
        	superpower = true;
        	countdownSuper = maxcountdownSuper;
            fever.Play();
		}
    }

    public void calmdown()
    {
		if(superpower == true) {
			linerenderer.transform.localScale = new Vector3(1, 1, 1); //Make cursor size normal
			linerenderer.SetWidth(1,1);//Make Line Renderer normal
			superpower = false;
		}
    }

    public IEnumerator MoveBitch(float time, Vector3 pos)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetpos = pos;

        float step = 0f; //raw step
        float rate = 1f / time; //amount to add to raw step
        float smoothStep = 0f; //current smooth step
        float lastStep = 0f; //previous smooth step
        while (step < 1f)
        { // until we're done
            step += Time.deltaTime * rate;
            smoothStep = Mathf.SmoothStep(0f, 1f, step);// finding smooth step
            transform.localPosition = new Vector3(Mathf.Lerp(startPos.x, targetpos.x, (smoothStep)),
                                                Mathf.Lerp(startPos.y, targetpos.y, (smoothStep)),
                                                Mathf.Lerp(startPos.z, targetpos.z, (smoothStep))); //lerp position
            lastStep = smoothStep; //get previous last step
            yield return null;
        }
        //complete rotation
        if (step > 1.0) transform.localPosition = new Vector3(Mathf.Lerp(startPos.x, targetpos.x, (1)),
                                                               Mathf.Lerp(startPos.y, targetpos.y, (1)),
                                                               Mathf.Lerp(startPos.z, targetpos.z, (1)));
    }
}
