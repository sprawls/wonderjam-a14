using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public GUISkin skinMenu;
	public Texture2D bgMenu;
	public Camera logoCamera;
	public Color textColor;

    private List<string> PathSongs=new List<string>();
    private List<string> SongNames = new List<string>();
    private List<int> BPMSongs = new List<int>();

    private int SongIndice = 0;
    private float BPMModifier = 1;
	private bool aiMode = false;
	private bool optionsMode = false;

    private bool AddSongMenu = false;

    private float Swidth;
    private float Sheight;

	private bool feverMode = false;
	// Use this for initialization
	void Start () {
        addSong("Music/approaching-nirvana-305","Approaching Nirvana - 305",128);
        addSong("Music/bayslick-tokyo-dinner", "Bayslick - Tokyo Dinner", 128);
        addSong("Music/bitch-clap", "Truxton - Bitch Clap", 145);
		addSong ("Music/ourautobiography-codebreaker", "OurAutobiography - CodeBreaker", 88);
		addSong ("Music/eric-lam-gta", "Eric Lam - GTA", 128);
		addSong ("Music/emotional-titanic-flute", "James Horner - My Heart Will Go On", 104);

		audio.clip = Resources.Load (PathSongs [SongIndice] + "-sample") as AudioClip;
		audio.Play ();
	}
	
	// Update is called once per frame
	void OnGUI () {
		Swidth=Screen.width;
		Sheight = Screen.height;
		GUI.skin = skinMenu;
        float curWidth;
        float curHeight;
		// Background
		GUI.DrawTexture (new Rect (0, 0, Swidth, Sheight), bgMenu);


            // Choix du beat
            curWidth = 75;
            curHeight = 20;
            GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f - 25, curWidth, curHeight), "Choix du beat", skinMenu.GetStyle("BPM label"));

            // Authors
            curWidth = 900;
            curHeight = 20;
            GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f - 250, curWidth, curHeight), "Alex Arsenault-Desjardins            Frédéric Bolduc            Martin Lavoie            Alexis Lessard", skinMenu.GetStyle("Auteurs"));

            // Song label
            curWidth = 150;
            curHeight = 20;
            GUIStyle songLabelStyle = skinMenu.GetStyle("Song label");
            songLabelStyle.normal.textColor = textColor;
            //Debug.Log (skinMenu.GetStyle ("Song label").normal.textColor);
            GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), SongNames[SongIndice], songLabelStyle);

            // Previous song
            curWidth = 48;
            curHeight = 48;
            if (SongIndice > 0 && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 425, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", skinMenu.GetStyle("Previous song")))
            {
                ChangeSong(-1);
            }

            // Next song
            curWidth = 48;
            curHeight = 48;
            if (SongIndice + 1 < PathSongs.Count && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 425, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", skinMenu.GetStyle("Next song")))
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
            if (BPMModifier < 2.0f && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 75, ((Sheight - curHeight) / 2.0f) + 50, curWidth, curHeight), "", skinMenu.GetStyle("Higher BPM")))
            {
                ChangeBPM(0.1f);
            }

            // Lower BPM
            curWidth = 24;
            curHeight = 24;
            if (BPMModifier > 0.1f && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 75, ((Sheight - curHeight) / 2.0f) + 50, curWidth, curHeight), "", skinMenu.GetStyle("Lower BPM")))
            {
                ChangeBPM(-0.1f);
            }

            // Instructions
            curWidth = 720;
            curHeight = 60;
            GUI.Label(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "31 octobre 2052, la race humaine a été exterminée par la menace zombie. La musique est morte, mais la légendaire bataille entre les deux dieux du rythme, DJ Guylaine Grosse-Soirée et DJ Jerry Ox, se poursuit encore après la mort. Le combat décisif se tiendra ce soir au célèbre Zombeat Stromatolite Turbo Party Club, pour la dernière chance pour le monde de retrouver un peu de couleur. Qui triomphera?", skinMenu.GetStyle("Instructions"));

            // Start button
            curWidth = 160;
            curHeight = 60;
            if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 240, curWidth, curHeight), "go!", skinMenu.GetStyle("Start button")))
            {
                StartGame();
            }

            // Render logo
            logoCamera.Render();

            //Fever Button 
            curWidth = 200;
            curHeight = 60;
            if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 200, ((Sheight - curHeight) / 2.0f) + 240, curWidth, curHeight), "FEVER MODE", skinMenu.GetStyle("Fever Button")))
            {
                feverMode = !feverMode;
                GUIStyle myFeverStyle = skinMenu.GetStyle("Fever Button");
                if (feverMode)
                {
                    myFeverStyle.normal.textColor = new Color(20f / 255f, 0, 1);
                }
                else
                {
                    myFeverStyle.normal.textColor = new Color(111f / 255f, 111f / 255f, 111f / 255f);
                }
            }


		// Next song
        curWidth = 48;
        curHeight = 48;
		if (SongIndice+1 < PathSongs.Count && GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 425, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", skinMenu.GetStyle("Next song")))
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
		GUI.Label(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "31 octobre 2052, la race humaine a été exterminée par la menace zombie. La musique est morte, mais la légendaire bataille entre les deux dieux du rythme, DJ Guylaine Grosse-Soirée et DJ Jerry Ox, se poursuit encore après la mort. Le combat décisif se tiendra ce soir au célèbre Zombeat Stromatolite Turbo Party Club, pour la dernière chance pour le monde de retrouver un peu de couleur. Qui triomphera?", skinMenu.GetStyle("Instructions"));
		
		// Start button
        curWidth = 80;
        curHeight = 80;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 230, curWidth, curHeight), "go!", skinMenu.GetStyle("Start button")))
		{
            StartGame();
        }

		// Render logo
		logoCamera.Render ();

		// Fever Button 
		curWidth = 150;
		curHeight = 60;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 120, ((Sheight - curHeight) / 2.0f) + 245, curWidth, curHeight), "fever mode", skinMenu.GetStyle("Fever Button"))) 
		{
			feverMode = !feverMode;
			GUIStyle myFeverStyle = skinMenu.GetStyle("Fever Button");
			if(feverMode){
				myFeverStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myFeverStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myFeverStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myFeverStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}

		// Vs AI 
		curWidth = 60;
		curHeight = 60;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 75, ((Sheight - curHeight) / 2.0f) + 204, curWidth, curHeight), "VS. ai", skinMenu.GetStyle("AI Button"))) 
		{
			aiMode = !aiMode;
			GUIStyle myAIStyle = skinMenu.GetStyle("AI Button");
			if(aiMode){
				myAIStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myAIStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myAIStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myAIStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}

		// Options 
		curWidth = 80;
		curHeight = 60;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 81, ((Sheight - curHeight) / 2.0f) + 245, curWidth, curHeight), "options", skinMenu.GetStyle("Options Button"))) 
		{
			optionsMode = !optionsMode;
			GUIStyle myOptionsStyle = skinMenu.GetStyle("Options Button");
			if(optionsMode){
				myOptionsStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myOptionsStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myOptionsStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myOptionsStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}
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
		persistentScript.OptAiMode = aiMode;
		if(feverMode == false) Application.LoadLevel("Game");
		else Application.LoadLevel ("FeverMode");
    }

}
