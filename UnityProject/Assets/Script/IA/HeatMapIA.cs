using UnityEngine;
using System.Collections.Generic;
using Math = System.Math;

public class HeatMapIA : IKeyGetter , IUpdate, IBeatReceiver{

    int[] heatmap = new int[40];
    bool[] peintured = new bool[40];
    
    BeatEnum next = BeatEnum.Empty;

    private ZombieFactory zf;
    
    PlayerBehaviour pb;
    PlayerBehaviour enemy;

    float precision;
    float firstBeat = 0;
    float threshold = 0;
    bool requestTime = true;
    int cpt = 0;

    bool myTurn;

    Stack<int> objective = new Stack<int>();

    int tourlength;

    public HeatMapIA(int bpm, PlayerBehaviour pb, PlayerBehaviour p2, float bonnete)
    {
        GameManager.Instance.requestBeat(this);
        GameManager.Instance.requestUpdate(this);

        tourlength = GameManager.Instance.getSpeedTurn();

        this.pb = pb;
        enemy = p2;
        threshold = 60.0f / bpm;
        zf = ZombieFactory.Instance;

        precision = 1 - (float)Math.Sqrt(1 - bonnete);
    }

    void refreshHeatmap()
    {
        int who = pb.whoami();
        for(int i = 0 ; i< 40 ; i++)
        {
            heatmap[i] = 0;
            peintured[i] = GameManager.Instance.getTile(i).color == who;

        }
        foreach(var z in zf.Zombies)
        {
            
            var p = z.transform.position;
            int x = ((int)p.x / 10)+1;
            int y = (int)p.z / 10;

            int index = indexing(x, y);   

            int force = 0;
            if (z.currentType == 0)
                force = 1;
            else if (z.currentType != who)
                force = 2;

            if (peintured[index])
                heatmap[index] = 0;
            else
                heatmap[index] += force;
        }
    }

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
        myTurn = newTurn;
        
        if (next == BeatEnum.Empty && Time.time > firstBeat + threshold * cpt)
        {
            if (objective.Count <= 0)
            {
                findObjective();
            }
            decideNextMove();
            cpt++;
        }
    }

    void findObjective()
    {
        refreshHeatmap();
        int cur = pb.cursor.pos;

        int[] parent = new int[40];
        int[] dist = new int[40];
        int[] score = new int [40];
        bool[] done = new bool[40];

        for (int i = 0; i < 40; i++)
        {
            parent[i] = -1;
            score[i] = int.MinValue;
            done[i] = false;
            dist[i] = 0;
        }

        score[cur] = heatmap[cur];
        

        int[] helper = new int[2]{-1,1};
        Debug.Log("DIJISTRA");
        for (int i = 0; i < tourlength/2; i++)
        {
            int node = find_max(score, done);
            Debug.Log("Node " + node.ToString());
            done[node] = true;

            int x = getX(node);
            int y = getY(node);

            foreach(var xx in helper)
            {
                foreach(var yy in helper)
                {
                    int nx = x + xx;
                    int ny = y + yy;

                    if(nx < 0 || nx > 4)
                        continue;
                    if(ny < 0 || ny > 7)
                        continue;

                    int nindex = indexing(nx, ny);

                    if(score[nindex] < score[node] + heatmap[nindex] && !done[nindex])
                    {
                        score[nindex] = score[node] + heatmap[nindex];
                        parent[nindex] = node;
                    }

                }
            }
        }
        
        int last = find_max(score);

        int cptt = 0;
        
        while(last != cur && cptt < 20)
        {
            objective.Push(last);
            last = parent[last];
            cptt++;
        }
    }

    int find_max(int[] score)
    {
        int index = -1;
        int min = int.MinValue;
        for (int i = 0; i < 40; i++)
        {
            if(score[i] > min)
            {
                min = score[i];
                index = i;
            }
        }
        return index;
    
    }

    int find_max(int[] score, bool[] excluded)
    {
        int index = -1;
        int min = int.MinValue;
        for (int i = 0; i < 40; i++)
        {
            if(score[i] > min && !excluded[i])
            {
                min = score[i];
                index = i;
            }
        }
        return index;
    }

    int distance_manhattan(int ind1, int ind2)
    {
        return Math.Abs(getX(ind2) - getX(ind1)) + Math.Abs(getY(ind2) - getY(ind1));
    }

    void decideNextMove()
    {
        int target = objective.Peek();
        
        int cur = pb.cursor.pos;

        int targetx = getX(target);
        int targety = getY(target);

        int curx = getX(cur);
        int cury = getY(cur);

        if(targetx != curx)
        {
            Debug.Log("x not equal");
            if(targetx > curx)
            {
                next = BeatEnum.Down;
                curx++;
            }
            else
            {
                next = BeatEnum.Up;
                curx--;
            }
        }
        else
        {
            Debug.Log("y not equal");
            if(targety > cury)
            {
                next = BeatEnum.Right;
                cury++;
            }
            else
            {
                next = BeatEnum.Left;
                cury--;
            }
        }

        if (indexing(targetx, targety) == indexing(curx, cury))
        {
            objective.Pop();
        }


    }

    int getX(int index)
    {
        return index % 5;
    }

    int getY(int index)
    {
        return index / 5;
    }

    int indexing(int x, int y)
    {
        return x + 5 * y;
    }

    public void OnQuarterBeat() { }

    public void OnBeat(BeatEnum mainPlayer, BeatEnum offPlayer, bool turnP1)
    {
        if (firstBeat == 0)
        {
            requestTime = true;
        }
    }

    public bool GetKeys(BeatEnum e)
    {
        if (next != BeatEnum.Empty && e == next)
        {

            next = BeatEnum.Empty;
            if (UnityEngine.Random.value < precision)
                return true;
        }

        return false;
    }
}
