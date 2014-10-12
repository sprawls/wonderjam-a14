using UnityEngine;
using System.Collections;

public class RandomIA : IKeyGetter {

    float lastTime;
    float bmp;
    float threshold;
    float changeOfSayingYes;

    BeatEnum next;

    public RandomIA()
    {
        changeOfSayingYes = Random.Range(70, 100)/100.0f;
        bmp = Random.Range(90, 130);
        threshold = 60 / bmp;
        lastTime = Time.realtimeSinceStartup;
        DecideNextKey();
    }

    public bool GetKeys(BeatEnum e)
    {
        float cur = Time.realtimeSinceStartup;
        if(cur - lastTime > threshold && e == next)
        {
            lastTime = cur;
            float r = Random.value;
            if(r < changeOfSayingYes)
            {
                next = DecideNextKey();
                return true;
            }
        }
        return false;

    }

    public static BeatEnum DecideNextKey()
    {
        int r = Random.Range(0, 3);
        switch(r)
        {
            case 0:
                return BeatEnum.Up;
            case 1:
                return BeatEnum.Down;
            case 2:
                return BeatEnum.Left;
            case 3:
                return BeatEnum.Right;
            default:
                return BeatEnum.Empty;
        }
    }
}
