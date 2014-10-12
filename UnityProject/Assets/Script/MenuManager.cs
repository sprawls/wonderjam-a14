using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public GUISkin skinMenu;
	public Texture2D bgMenu;
	public Camera logoCamera;

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

		audio.clip = Resources.Load (PathSongs [SongIndice] + "-sample") as AudioClip;
		audio.Play ();
	}
	
	// Update is called once per frame
	void OnGUI () {
		Swidth=Screen.width;
		Sheight = Screen.height;
		GUI.skin = skinMenu;

		// Background
		GUI.DrawTexture (new Rect (0, 0, Swidth, Sheight), bgMenu);

		float curWidth = 75;
		float curHeight = 20;
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f - 25, curWidth, curHeight), "Choix du beat", skinMenu.GetStyle ("BPM label"));

		// Song label
        curWidth=150;
        curHeight=20;
		GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), SongNames[SongIndice], skinMenu.GetStyle("Song label"));
		
		// Previous song
        curWidth = 48;
        curHeight = 48;
        if (SongIndice > 0 && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 350, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", skinMenu.GetStyle("Previous song")))
        {
            ChangeSong(-1);
        }

		// Next song
        curWidth = 48;
        curHeight = 48;
		if (SongIndice+1 < PathSongs.Count && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 350, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", skinMenu.GetStyle("Next song")))
		{
            ChangeSong(1);
        }

		// BPM label
        curWidth = 75;
        curHeight = 20;
		GUI.Label(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 50, curWidth, curHeight), Mathf.Round((BPMSongs[SongIndice] * BPMModifier)) + " BPM", skinMenu.GetStyle("BPM label"));
		
		// Higher BPM
        curWidth = 24;
        curHeight = 24;
		if (BPMModifier<2.0f && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 75, ((Sheight - curHeight) / 2.0f) + 50, curWidth, curHeight), "", skinMenu.GetStyle("Higher BPM")))
		{
            ChangeBPM(0.1f);
        }

		// Lower BPM
        curWidth = 24;
        curHeight = 24;
		if (BPMModifier > 0.1f && GUI.Button (new Rect (((Swidth - curWidth) / 2.0f) - 75, ((Sheight - curHeight) / 2.0f) + 50, curWidth, curHeight), "", skinMenu.GetStyle("Lower BPM"))) {
			ChangeBPM (-0.1f);
		}

		// Instructions
		curWidth = 720;
		curHeight = 60;
		GUI.Label(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "Instructions Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum mLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsumLorem ipsum", skinMenu.GetStyle("Instructions"));
		
		// Start button
        curWidth = 350;
        curHeight = 60;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 240, curWidth, curHeight), "go!", skinMenu.GetStyle("Start button")))
		{
            StartGame();
        }

		// Render logo
		logoCamera.Render ();
	}

    public void ChangeSong(int value)
    {
        SongIndice += value;
        BPMModifier = 1;

		audio.Stop ();
		audio.clip = Resources.Load (PathSongs [SongIndice] + "-sample") as AudioClip;
		audio.Play ();
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
