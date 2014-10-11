using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GUISkin Bg_menu;
    public GUISkin bg_ui;
    public GUISkin fontL;
    public GUISkin fontR;
    public GUISkin btL;
    public GUISkin btS;

    private float Swidth;
    private float Sheight;
	// Use this for initialization
	void Start () {
	    Swidth=Screen.width;
        Sheight = Screen.height;
	}
	
	// Update is called once per frame
	void OnGUI () {
        //GUI.skin = Title_song;
        float curWidth=150;
        float curHeight=20;
        GUI.Label(new Rect((Swidth - curWidth - 40) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "Song Name");
        //GUI.skin = Arrow_Left;
        curWidth = 20;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth - 40) / 2.0f) - 100, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "<");
        //GUI.skin = Arrow_Right;
        curWidth = 20;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth - 40) / 2.0f) + 175, (Sheight - curHeight) / 2.0f, curWidth, curHeight), ">");

        //GUI.skin = Bpm_song;
        curWidth = 75;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth - 40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f), curWidth, curHeight), "140 BPM");
        //GUI.skin = Arrow_Up;
        curWidth = 20;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth - 40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f) - 20, curWidth, curHeight), "^");
        //GUI.skin = Arrow_Down;
        curWidth = 20;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth-40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f)+20, curWidth, curHeight), "v");

        //GUI.skin = Bt_Start;
        curWidth = 350;
        curHeight = 60;
        if (GUI.Button(new Rect(((Swidth - curWidth + 90) / 2.0f), ((Sheight - curHeight) / 2.0f) + 70, curWidth, curHeight), "Start [Enter]"))
        {
            Application.LoadLevel("Game");
        }

        //GUI.skin = Infos;
        curWidth = 350;
        curHeight = 180;
        GUI.Label(new Rect(((Swidth - curWidth + 90) / 2.0f), ((Sheight - curHeight) / 2.0f) + 210, curWidth, curHeight), "Instructions Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum mLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum");



	}
}
