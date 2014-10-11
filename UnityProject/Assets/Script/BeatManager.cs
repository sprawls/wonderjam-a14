using UnityEngine;
using System.Collections;
using System.Timers;

public class BeatManager {
    
    private IBeatReceiver IbeatReceiverRef;
    private int tempo;
    private BeatEnum p1Input;
    private BeatEnum p2Input;
    private bool p1Turn=true;
    private int beatCpt = 0;
    private float interval;
    public float _interval { get { return this.interval; } }

    private float intervalCpt = 0;

    private Timer t_Beat_update = new Timer();
    private Timer t_Beat_acuracy = new Timer();


    public BeatManager(IBeatReceiver Beat)
    {
        IbeatReceiverRef = Beat;
        t_Beat_update.Elapsed += new ElapsedEventHandler(doBeat);
        t_Beat_acuracy.Elapsed += new ElapsedEventHandler(IncreaseInvervalCpt);
        t_Beat_acuracy.Enabled = true;
        t_Beat_acuracy.Interval = (50);

        changeTempo();
        t_Beat_update.Enabled = true;
        t_Beat_update.Start();
        t_Beat_acuracy.Start();

    }

    private void IncreaseInvervalCpt(object source, ElapsedEventArgs e)
    {
        intervalCpt += 50;
    }

    public void setInputP1(int value)
    {
        if (intervalCpt >= interval * 0.8f)
        {
            switch (value)
            {
                case 0:
                        p1Input = BeatEnum.Up;
                    break;
                case 1:
                        p1Input = BeatEnum.Left;
                    break;
                case 2:
                        p1Input = BeatEnum.Down;
                    break;
                case 3:
                        p1Input = BeatEnum.Right;
                    break;
            }
        }
        else
        {
            p1Input = BeatEnum.Missed;
        }
        Debug.Log(p1Input);

    }

    public void setInputP2(int value)
    {

        if (intervalCpt >= interval * 0.9f || intervalCpt <= interval * 0.1f)
        {
            switch (value)
            {
                case 0:
                    p2Input = BeatEnum.Up;
                    break;
                case 1:
                    p2Input = BeatEnum.Left;
                    break;
                case 2:
                    p2Input = BeatEnum.Down;
                    break;
                case 3:
                    p2Input = BeatEnum.Right;
                    break;
            }
        }
        else
        {
            p2Input = BeatEnum.Missed;
        }
        Debug.Log(p2Input);
    }

    private void doBeat(object source, ElapsedEventArgs e)
    {
        intervalCpt = 0;
        beatCpt++;
        if (beatCpt >= 16)
        {
            beatCpt = 0;
            if (p1Turn)
            {
                p1Turn = false;
            }
            else
            {
                p1Turn = true;
            }
            
        }
        if (p1Turn)
        {
            IbeatReceiverRef.OnBeat(p1Input, p2Input, p1Turn);
        }
        else
        {
            IbeatReceiverRef.OnBeat(p2Input, p1Input, p1Turn);
        }
        Debug.Log("tour de p1:"+ p1Turn+" | "+ beatCpt);
    }
	
    public void changeTempo(int newTempo=140)
    {
        tempo = newTempo;
        interval = (60 * 1000) / tempo;
        t_Beat_update.Interval = ((60*1000)/tempo);
    }

}
