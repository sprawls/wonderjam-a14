using UnityEngine;
using System.Collections;

public class RandomIA : IKeyGetter {

    float lastTime;
    float bmp;
    float threshold;
    float changeOfSayingYes;

    public RandomIA()
    {
        changeOfSayingYes = Random.Range(60, 100)/400.0f;
        bmp = Random.Range(90, 130);
        threshold = 60 / bmp;
        lastTime = Time.realtimeSinceStartup;
    }

    public bool GetKeys(BeatEnum e)
    {
        Debug.Log("HEY!");
        float cur = Time.realtimeSinceStartup;
        if(cur - lastTime > threshold)
        {
            lastTime = cur;
            float r = Random.value;
            if(r < changeOfSayingYes)
            {
                return true;
            }
        }
        return false;

    }
}
