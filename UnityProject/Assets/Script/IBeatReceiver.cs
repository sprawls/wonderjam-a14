using UnityEngine;
using System.Collections;

public interface IBeatReceiver {
    void OnBeat(BeatEnum p1, BeatEnum p2);
}
