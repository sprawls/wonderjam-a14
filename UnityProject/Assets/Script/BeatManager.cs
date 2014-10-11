using UnityEngine;
using System.Collections;
using System.Timers;

public class BeatManager {
    
    private IBeatReceiver IbeatReceiverRef;
    private int tempo;
    private BeatEnum p1Input;
    private BeatEnum p2Input;
    private bool p1Turn=true;


    private Timer t_Beat_update = new Timer();

    public BeatManager(IBeatReceiver Beat)
    {
        IbeatReceiverRef = Beat;
        t_Beat_update.Elapsed += new ElapsedEventHandler(doBeat);
        changeTempo();
        t_Beat_update.Enabled = true;
        t_Beat_update.Start();
    }

   

    private void doBeat(object source, ElapsedEventArgs e)
    {

        if (p1Turn)
        {
            IbeatReceiverRef.OnBeat(p1Input, p2Input);
        }
        else
        {
            IbeatReceiverRef.OnBeat(p1Input, p2Input);
        }
    }
	
    public void changeTempo(int newTempo=140)
    {
        tempo = newTempo;
        Debug.Log((60 * 1000) / tempo);
        t_Beat_update.Interval = ((60*1000)/tempo);
    }

}
