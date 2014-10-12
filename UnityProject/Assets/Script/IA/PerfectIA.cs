using UnityEngine;
using System;
using System.Collections;

public class PerfectIA : IKeyGetter, IBeatReceiver, IUpdate {

    float firstBeat = 0;
    float threshold = 0;
    bool requestTime = true;

    int cpt = 0;

    PlayerBehaviour pb;
    PlayerBehaviour enemy;

    bool myTurn;

    BeatEnum next = BeatEnum.Empty;

    private ZombieFactory zf;

    private Nullable<Vector2> objective = null;
    private int objCount = 0;


    public PerfectIA(int bpm, PlayerBehaviour pb, PlayerBehaviour p2)
    {
        GameManager.Instance.requestBeat(this);
        GameManager.Instance.requestUpdate(this);

        this.pb = pb;
        enemy = p2;
        threshold = 60.0f / bpm;
        zf = ZombieFactory.Instance;
    }

    public bool GetKeys(BeatEnum e)
    {
        if(next != BeatEnum.Empty && e == next)
        {
            next = BeatEnum.Empty;
            return true;
        }
            
        return false;
    }

    public void OnBeat(BeatEnum mainPlayer, BeatEnum offPlayer, bool turnP1)
    {
        if (firstBeat == 0)
        {
            requestTime = true;
        }
    }

    public void OnQuarterBeat(){}

    public void OnUpdate()
    {
        if (!GameManager.Instance.Playing)
            return;
        if (requestTime)
        {
            Debug.Log("Putting tempo");
            firstBeat = Time.time + threshold / 2.0f;
            requestTime = false; ;
        }
        bool newTurn = pb.isMyTurn();
        if(newTurn != myTurn)
        {
            objective = null;
        }
        myTurn = newTurn;
        if(!objective.HasValue)
        {
            findObjective();
            objCount = 5;
        }
        if(next == BeatEnum.Empty && Time.time > firstBeat + threshold * cpt )
        {
            decideNextMove();
            cpt++;
            objCount--;;;
        }
    }

    private void findObjective()
    {
        Vector3 sum = new Vector3();
        int who = pb.whoami();
        Vector3 pos;
        if (myTurn)
        {
            pos = pb.cursor.transform.position;

            foreach (var z in zf.Zombies)
            {
                int force = 1;
                if (z.currentType != 0 && z.currentType != who)
                {
                    force = 2;
                }
                sum += (z.transform.position - pos) * force;
            }

            sum = sum.normalized * 50;
            objective = new Vector2(sum.x + pos.x, sum.z + pos.z);
            Debug.Log("OBJECTIVE");
            Debug.Log(objective);
            Debug.Log(pos);
            Debug.Log(sum);
        }
        else
        {

        }
    }

    private void decideNextMove()
    {
        Vector3 pos;
        if(myTurn)
        {
            pos = pb.cursor.transform.position;
            
            Vector2 p = new Vector2(pos.x,pos.z);
            Vector2 s = objective.Value - p;
            
            if(Math.Sign(s.x) * s.x > Math.Sign(s.y) * s.y)
            {
                if(Math.Sign(s.x) == 1)
                {
                    next = BeatEnum.Down;
                }
                else
                {
                    next = BeatEnum.Up;
                }
            }
            else
            {
                if (Math.Sign(s.y) == 1)
                {
                    next = BeatEnum.Right;
                }
                else
                {
                    next = BeatEnum.Left;
                }
            }

            if(Vector2.Distance(s,objective.Value) < 10)
            {
                objective = null;
            }

        }
        else
        {
            pos = enemy.transform.position;
        }
    }
}
