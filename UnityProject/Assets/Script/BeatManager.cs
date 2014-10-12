using UnityEngine;
using System.Collections;
using System.Timers;

public class BeatManager : Singleton<BeatManager> {
	protected BeatManager () {}

    private IBeatReceiver IbeatReceiverRef;
    private int tempo;
    private BeatEnum p1Input=BeatEnum.Empty;
    private BeatEnum p2Input=BeatEnum.Empty;
    private bool p1miss = false;
    private bool p2miss = false;
    private bool ctrlLock = true;
    private bool p1Turn=false;
    private int beatCpt = 0;
    private int SpeedTurn;

    private bool p1LateXpect = false;
    private bool p2LateXpect = false;


    private float _interval;
    public float interval { get { return this._interval; } }

    private int subQuarter = 1;

    private float intervalCpt = 0;

    private Timer t_Beat_update = new Timer();
    private Timer t_Beat_acuracy = new Timer();

	private float sweet = 0.0f;

    public void SetBeat(IBeatReceiver Beat)
    {

        IbeatReceiverRef = Beat;
        t_Beat_update.Elapsed += new ElapsedEventHandler(doBeat);
        t_Beat_acuracy.Elapsed += new ElapsedEventHandler(IncreaseInvervalCpt);
        t_Beat_acuracy.Enabled = true;
        t_Beat_acuracy.Interval = (50);
        SpeedTurn = GameManager.Instance.getSpeedTurn();
        changeTempo();
        t_Beat_update.Enabled = true;
        t_Beat_update.Start();
        t_Beat_acuracy.Start();
    }

	public void OnDestroy() {
		t_Beat_update.Stop ();
		t_Beat_acuracy.Stop ();
	}

    private void IncreaseInvervalCpt(object source, ElapsedEventArgs e)
    {
        intervalCpt += 50;
        if (intervalCpt>=(_interval / 4)*subQuarter)
        {
            subQuarter++;
            IbeatReceiverRef.OnQuarterBeat();
        }
    }

    public void setInputP1(int value)
    {
        if (!p1miss && !ctrlLock)
        {
            if (intervalCpt >= interval * sweet && (p1Input == BeatEnum.Empty || p1Input == BeatEnum.Missed))
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
                p1miss = true;
            }
        }
       // Debug.Log(p1Input);

    }

    public void setInputP2(int value)
    {
        if (!p2miss&&!ctrlLock)
        {
            if (intervalCpt >= interval * sweet && (p2Input == BeatEnum.Empty || p2Input == BeatEnum.Missed))
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
                p2miss = true;
            }
        }
        //Debug.Log(p2Input);
    }

    private void doBeat(object source, ElapsedEventArgs e)
    {
        intervalCpt = 0;
        subQuarter = 1;
        beatCpt++;
        if (beatCpt >= SpeedTurn)
        {
            ctrlLock = false;
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

        if (p1Input == BeatEnum.Empty)
        {
            p1Input = BeatEnum.Missed;
        }
        else
        {
            p1Input = BeatEnum.Empty;
        }

        if (p2Input == BeatEnum.Empty)
        {
            p2Input = BeatEnum.Missed;
        }
        else
        {
            p2Input = BeatEnum.Empty;
        }
        p1miss = false;
        p2miss = false;
    }
	
    public void changeTempo(int newTempo =140)
    {
        tempo = newTempo;
        _interval = (60 * 1000) / tempo;
        t_Beat_update.Interval = ((60*1000)/tempo);
    }

	public bool aboutToSwitch { 
		get { 
			return (beatCpt >= 12) && (beatCpt < 15);
		}
	}
}
