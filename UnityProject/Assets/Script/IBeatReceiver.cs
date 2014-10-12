using UnityEngine;
using System.Collections;

public interface IBeatReceiver {
    void OnBeat(BeatEnum mainPlayer, BeatEnum offPlayer, bool turnP1);
    void OnQuarterBeat();
    //void LateBeat(BeatEnum mainPlayer, BeatEnum offPlayer, bool turnP1);
}


