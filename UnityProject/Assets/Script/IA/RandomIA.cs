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
                DecideNextKey();
                return true;
            }
        }
        return false;

    }

    private void DecideNextKey()
    {
        int r = Random.Range(0, 3);
        switch(r)
        {
            case 0:
                next = BeatEnum.Up;
                break;
            case 1:
                next = BeatEnum.Down;
                break;
            case 2:
                next = BeatEnum.Left;
                break;
            case 3:
                next = BeatEnum.Right;
                break;
        }
    }
}
