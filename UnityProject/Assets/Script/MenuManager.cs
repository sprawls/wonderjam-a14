using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    public GUISkin Bg_menu;
    public GUISkin bg_ui;
    public GUISkin fontL;
    public GUISkin fontR;
    public GUISkin btL;
    public GUISkin btS;

    private List<string> PathSongs=new List<string>();
    private List<string> SongNames = new List<string>();
    private List<int> BPMSongs = new List<int>();

    private int SongIndice = 0;
    private float BPMModifier = 1;

    private float Swidth;
    private float Sheight;
	// Use this for initialization
	void Start () {
        addSong("Music/approaching-nirvana-305","Approaching Nirvana - 305",128);
        addSong("Music/bayslick-tokyo-dinner", "Bayslick - Tokyo Dinner", 128);
        addSong("Music/bitch-clap", "Truxton - Bitch Clap", 145);



	    Swidth=Screen.width;
        Sheight = Screen.height;
	}
	
	// Update is called once per frame
	void OnGUI () {
        //GUI.skin = Title_song;
        float curWidth=150;
        float curHeight=20;
        GUI.Label(new Rect((Swidth - curWidth - 40) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), SongNames[SongIndice]);
        //GUI.skin = Arrow_Left;
        curWidth = 20;
        curHeight = 20;
        if (SongIndice > 0 && GUI.Button(new Rect(((Swidth - curWidth - 40) / 2.0f) - 100, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "<"))
        {
            ChangeSong(-1);
        }
        //GUI.skin = Arrow_Right;
        curWidth = 20;
        curHeight = 20;
        if (SongIndice+1 < PathSongs.Count && GUI.Button(new Rect(((Swidth - curWidth - 40) / 2.0f) + 175, (Sheight - curHeight) / 2.0f, curWidth, curHeight), ">"))
        {
            ChangeSong(1);
        }


        //GUI.skin = Bpm_song;
        curWidth = 75;
        curHeight = 20;
        GUI.Label(new Rect(((Swidth - curWidth - 40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f), curWidth, curHeight), Mathf.Round((BPMSongs[SongIndice] * BPMModifier)) + " BPM");
        //GUI.skin = Arrow_Up;
        curWidth = 20;
        curHeight = 20;
        if (BPMModifier<2.0f && GUI.Button(new Rect(((Swidth - curWidth - 40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f) - 20, curWidth, curHeight), "^"))
        {
            ChangeBPM(0.1f);
        }
        //GUI.skin = Arrow_Down;
        curWidth = 20;
        curHeight = 20;
        
        if (BPMModifier>0.1f && GUI.Button(new Rect(((Swidth - curWidth - 40) / 2.0f) + 230, ((Sheight - curHeight) / 2.0f) + 20, curWidth, curHeight), "v"))
        {
            ChangeBPM(-0.1f);
        }

        //GUI.skin = Bt_Start;
        curWidth = 350;
        curHeight = 60;
        if (GUI.Button(new Rect(((Swidth - curWidth + 90) / 2.0f), ((Sheight - curHeight) / 2.0f) + 70, curWidth, curHeight), "Start [Enter]"))
        {
            StartGame();
        }

        //GUI.skin = Infos;
        curWidth = 350;
        curHeight = 180;
        GUI.Label(new Rect(((Swidth - curWidth + 90) / 2.0f), ((Sheight - curHeight) / 2.0f) + 210, curWidth, curHeight), "Instructions Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum mLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum");

	}

    public void ChangeSong(int value)
    {
        SongIndice += value;
        BPMModifier = 1;
    }

    public void ChangeBPM(float value)
    {
        if (BPMModifier + value > 0 && BPMModifier + value <= 2) 
        {
            BPMModifier += value;
        }
    }

    private void addSong(string path, string name, int bpm)
    {
        PathSongs.Add(path);
        SongNames.Add(name);
        BPMSongs.Add(bpm);
    }

    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            ChangeBPM(0.1f);
        }
        if (SongIndice > 0 && Input.GetKeyDown("a"))
        {
            ChangeSong(-1);
        }
        if (Input.GetKeyDown("s"))
        {
            ChangeBPM(-0.1f);
        }
        if (SongIndice + 1 < PathSongs.Count && Input.GetKeyDown("d"))
        {
            ChangeSong(1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeBPM(0.1f);
        }
        if (SongIndice > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSong(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeBPM(-0.1f);
        }
        if (SongIndice + 1 < PathSongs.Count && Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSong(1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        Persistent persistentScript = GameObject.Find("persistent").GetComponent("Persistent") as Persistent;
        persistentScript.songBPM = (int)Mathf.Round((BPMSongs[SongIndice] * BPMModifier));
        persistentScript.songPath = PathSongs[SongIndice];
        persistentScript.songMulti = BPMModifier;
        Application.LoadLevel("Game");
    }

}
