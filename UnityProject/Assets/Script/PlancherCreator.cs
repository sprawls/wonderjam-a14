using UnityEngine;
using System.Collections;

public class PlancherCreator  {
    

    public static TileAnimation[] CreatePlancher(GameObject black, GameObject white)
    {
        float x = 0;
        float y = 0;
        float z = 0;

        GameManager gm = GameManager.Instance;
        TileAnimation[] tiles = new TileAnimation[40];

        for (int i = 0; i < 40; i++)
        {
            GameObject go = Object.Instantiate(i % 2 == 0 ? black : white) as GameObject;
            TileAnimation ta = go.GetComponent<TileAnimation>();
            tiles[i] = ta;
            gm.requestBeat(ta);
            go.transform.position = new Vector3(x-10, y, z);

            x += 10;
            if (x > 40)
            {
                x = 0;
                z += 10;
            }
        }
        return tiles;
    }
}
