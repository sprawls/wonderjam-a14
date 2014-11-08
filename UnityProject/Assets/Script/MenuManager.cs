using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public GUISkin skinMenu;
	public Texture2D bgMenu;
	public Texture2D bgExtras;
	public Camera logoCamera;
	public List<Texture2D> tutorials = new List<Texture2D>();
	public List<Texture2D> tutorials_EN= new List<Texture2D>();
	public Color textColor = Color.white;
	public float optionsOffset = 0.0f;
	public float iaDifficultyOffset = 0.0f;

	private Animator animator;

    private List<string> PathSongs=new List<string>();
    private List<string> SongNames = new List<string>();
    private List<int> BPMSongs = new List<int>();

	private int tutorialIndex = 0;
    private int SongIndice = 0;
    private float BPMModifier = 1;
	private bool aiMode = false;
	private bool optionsMode = false, tutorialMode = false;

    private bool AddSongMenu = false;

    private float Swidth;
    private float Sheight;

	private bool feverMode = false, chaosMode = false, tacticMode = false, starbucksMode = false;
	private int aiDifficulty = 0, grosseurSoiree = 0, longueurSoiree = 1, beatsParTour = 0;

	//Language
	Persistent persistentScript;

	// Use this for initialization
	void Start () {
		persistentScript = GameObject.Find("persistent").GetComponent("Persistent") as Persistent;
       
		//AddSongAll (); // Add all songs
		AddFreeSong (); //Only add songs with free use

		audio.clip = Resources.Load (PathSongs [SongIndice] + "-sample") as AudioClip;
		audio.Play ();

		animator = GetComponent<Animator> ();
	}

	void AddSongAll(){
		addSong("Music/approaching-nirvana-305","Approaching Nirvana - 305",128);
		addSong("Music/bayslick-tokyo-dinner", "Bayslick - Tokyo Dinner", 128);
		addSong("Music/bitch-clap", "Truxton - Bitch Clap", 145);
		addSong ("Music/ourautobiography-codebreaker", "OurAutobiography - CodeBreaker", 88);
		addSong ("Music/eric-lam-gta", "Eric Lam - GTA", 128);
		addSong ("Music/emotional-titanic-flute", "James Horner - My Heart Will Go On", 104);
	}

	void AddFreeSong(){
		addSong("Music/bayslick-tokyo-dinner", "Bayslick - Tokyo Dinner", 128);
		addSong("Music/bitch-clap", "Truxton - Bitch Clap", 145);
		addSong ("Music/ourautobiography-codebreaker", "OurAutobiography - CodeBreaker", 88);
		addSong ("Music/eric-lam-gta", "Eric Lam - GTA", 128);

	}
	
	// Update is called once per frame
	void OnGUI () {
		Swidth=Screen.width;
		Sheight = Screen.height;
		GUI.skin = skinMenu;
        float curWidth;
        float curHeight;
		//Background
		GUI.DrawTexture (new Rect (0, 0, Swidth, Sheight), bgMenu);

		//Language buttons
		curWidth = 120;
		curHeight = 25;
		if (GUI.Button(new Rect(10, 20, curWidth, curHeight), "     ", skinMenu.GetStyle("Options Button")))
		{
			persistentScript.language = Language.french;
		}
		if (GUI.Button(new Rect(10,55, curWidth, curHeight), "     ", skinMenu.GetStyle("Options Button"))) 
		{
			persistentScript.language = Language.english;
		}


		if(persistentScript.language == Language.french) {
		/************************************** FRENCH LANGUAGE **************************************/
		/**                                                                                        ***/
		/**                                                                                        ***/
		/*********************************************************************************************/
			
			// Choix du beat
			curWidth = 75;
			curHeight = 20;
			GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f - 25, curWidth, curHeight), "Choix du beat", skinMenu.GetStyle("BPM label"));
			
			// Authors
			curWidth = 900;
			curHeight = 20;
			GUI.Label(new Rect((Swidth - curWidth) / 2.0f, 16, curWidth, curHeight), "Alex Arsenault-Desjardins            Frédéric Bolduc            Martin Lavoie            Alexis Lessard", skinMenu.GetStyle("Auteurs"));
			GUI.Label(new Rect((Swidth - curWidth) / 2f, 30, curWidth, curHeight), "                @IronSquirel                                                           @ItsFerdBold                                            @refnil19                                         @Alexis_Lessard", skinMenu.GetStyle("AuteursTwit"));

			// Song label
			curWidth = 150;
			curHeight = 20;
			GUIStyle songLabelStyle = skinMenu.GetStyle("Song label");
			songLabelStyle.normal.textColor = textColor;
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
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f), Sheight - curHeight - 27, curWidth, curHeight), "go!", skinMenu.GetStyle("Start button")))
			{
				StartGame();
			}
			
			// Render logo
			logoCamera.Render ();
			
			// Extras Button 
			curWidth = 100;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 95, Sheight - curHeight - 32, curWidth, curHeight), "extras", skinMenu.GetStyle("Options Button"))) 
			{
				optionsMode = !optionsMode;
			}
			GUIStyle myOptionsStyle = skinMenu.GetStyle("Options Button");
			if(optionsMode){
				myOptionsStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myOptionsStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myOptionsStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myOptionsStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
			
			animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
			
			// Vs AI 
			curWidth = 60;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 75, Sheight - curHeight - 72, curWidth, curHeight), "VS. ai", skinMenu.GetStyle("AI Button"))) 
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
			animator.SetBool (Animator.StringToHash("IAOpen"), aiMode);
			
			curWidth = 240;
			curHeight = 40;
			aiDifficulty = GUI.SelectionGrid (new Rect ((Swidth - curWidth) / 2.0f + 230 + iaDifficultyOffset, Sheight - curHeight - 76, curWidth, curHeight), aiDifficulty, new GUIContent[3] {
				new GUIContent ("Poche"),
				new GUIContent ("Pas pire"),
				new GUIContent ("Nice")
			}, 3, GUI.skin.GetStyle ("List button"));
			
			// Tutorial button 
			curWidth = 160;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 122, Sheight - curHeight - 32, curWidth, curHeight), "instructions", skinMenu.GetStyle("Tutorial Button"))) 
			{
				tutorialMode = !tutorialMode;
				animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
			}
			GUIStyle myTutorialStyle = skinMenu.GetStyle("Tutorial Button");
			if(tutorialMode) {
				myTutorialStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myTutorialStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myTutorialStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myTutorialStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
			
			/*// Quit button
			curWidth = 16;
			curHeight = 16;
			if (GUI.Button (new Rect (Swidth - curWidth - 8, 8, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
				Application.Quit();
			}
			*/
			renderOptionsFrench ();
			
			if (tutorialMode) {
				renderTutorial ();
			}
		} else if(persistentScript.language == Language.english) {
		/************************************* ENGLISH LANGUAGE **************************************/
		/**                                                                                        ***/
		/**                                                                                        ***/
		/*********************************************************************************************/
			// Choix du beat
			curWidth = 75;
			curHeight = 20;
			GUI.Label(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f - 25, curWidth, curHeight), "Beat Choice", skinMenu.GetStyle("BPM label"));
			
			// Authors
			curWidth = 900;
			curHeight = 20;
			GUI.Label(new Rect((Swidth - curWidth) / 2.0f, 16, curWidth, curHeight), "Alex Arsenault-Desjardins            Frédéric Bolduc            Martin Lavoie            Alexis Lessard", skinMenu.GetStyle("Auteurs"));
			GUI.Label(new Rect((Swidth - curWidth) / 2f, 30, curWidth, curHeight), "                @IronSquirel                                                           @ItsFerdBold                                            @refnil19                                         @Alexis_Lessard", skinMenu.GetStyle("AuteursTwitEN"));

			// Song label
			curWidth = 150;
			curHeight = 20;
			GUIStyle songLabelStyle = skinMenu.GetStyle("Song label");
			songLabelStyle.normal.textColor = textColor;
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
			GUI.Label(new Rect(((Swidth - curWidth) / 2.0f), ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "October 31, 2052. The human race has been exterminated by the zombie threat. Music is dead, but the legendary battle between the two rhythm gods, DJ Big Bash Bertha and DJ Jerry Ox, persists through death. The decisive battle happens tonight at the renowned Zombeat Stromatolite Turbo Party Club. It is the last hope for the world to regain some of its colors. Who wil win ? ", skinMenu.GetStyle("Instructions"));

			// Start button
			curWidth = 80;
			curHeight = 80;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f), Sheight - curHeight - 27, curWidth, curHeight), "go!", skinMenu.GetStyle("Start button")))
			{
				StartGame();
			}
			
			// Render logo
			logoCamera.Render ();
			
			// Extras Button 
			curWidth = 100;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 95, Sheight - curHeight - 32, curWidth, curHeight), "extras", skinMenu.GetStyle("Options Button"))) 
			{
				optionsMode = !optionsMode;
			}
			GUIStyle myOptionsStyle = skinMenu.GetStyle("Options Button");
			if(optionsMode){
				myOptionsStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myOptionsStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myOptionsStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myOptionsStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
			
			animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
			
			// Vs AI 
			curWidth = 60;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + 75, Sheight - curHeight - 72, curWidth, curHeight), "VS. ai", skinMenu.GetStyle("AI Button"))) 
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
			animator.SetBool (Animator.StringToHash("IAOpen"), aiMode);
			
			curWidth = 240;
			curHeight = 40;
			aiDifficulty = GUI.SelectionGrid (new Rect ((Swidth - curWidth) / 2.0f + 230 + iaDifficultyOffset, Sheight - curHeight - 76, curWidth, curHeight), aiDifficulty, new GUIContent[3] {
				new GUIContent ("Bad"),
				new GUIContent ("Less Bad"),
				new GUIContent ("Sweet")
			}, 3, GUI.skin.GetStyle ("List button"));
			
			// Tutorial button 
			curWidth = 160;
			curHeight = 40;
			if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) - 122, Sheight - curHeight - 32, curWidth, curHeight), "instructions", skinMenu.GetStyle("Tutorial Button"))) 
			{
				tutorialMode = !tutorialMode;
				animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
			}
			GUIStyle myTutorialStyle = skinMenu.GetStyle("Tutorial Button");
			if(tutorialMode) {
				myTutorialStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myTutorialStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myTutorialStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myTutorialStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
			
			/*// Quit button
			curWidth = 16;
			curHeight = 16;
			if (GUI.Button (new Rect (Swidth - curWidth - 8, 8, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
				Application.Quit();
			}
			*/
			renderOptionsEnglish();
			
			if (tutorialMode) {
				renderTutorialEnglish ();
			}

		}
	}

	public void renderOptionsFrench() {
		// Background
		GUI.DrawTexture (new Rect ((Swidth - 600) / 2.0f + optionsOffset, (Sheight - 400) / 2.0f, 600, 400), bgExtras);

		float curWidth = 300;
		float curHeight = 40;
		GUIStyle mySongStyle = skinMenu.GetStyle ("Song label");
		mySongStyle.normal.textColor = textColor;

		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset, (Sheight - curHeight) / 2.0f - 170, curWidth, curHeight), "Extras", mySongStyle);

		curWidth = 32;
		curHeight = 32;
		if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset + 300, (Sheight - curHeight) / 2.0f - 200, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
			optionsMode = false;
			animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
		}

		// Fever mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) - 120, curWidth, curHeight), "fever mode", skinMenu.GetStyle("Fever Button"))) 
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

		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f - 85, curWidth, curHeight), "Fermez les lumières!", skinMenu.GetStyle ("Instructions"));

		// Chaos mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) - 40, curWidth, curHeight), "chaos mode", skinMenu.GetStyle("Chaos Button"))) 
		{
			chaosMode = !chaosMode;
			GUIStyle myChaosStyle = skinMenu.GetStyle("Chaos Button");
			if(chaosMode){
				myChaosStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myChaosStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myChaosStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myChaosStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}

		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f - 5, curWidth, curHeight), "Les zombis sont incontrôlables!", skinMenu.GetStyle ("Instructions"));

		// Tactic mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) + 40, curWidth, curHeight), "tactic mode", skinMenu.GetStyle("Tactic Button"))) 
		{
			tacticMode = !tacticMode;
			GUIStyle myTacticStyle = skinMenu.GetStyle("Tactic Button");
			if(tacticMode){
				myTacticStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myTacticStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myTacticStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myTacticStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}

		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f + 75, curWidth, curHeight), "Les diagonales à tous les 5 coups!", skinMenu.GetStyle ("Instructions"));

		// Starbucks mode
		curWidth = 200;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 175, ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "starbucks mode", skinMenu.GetStyle("Starbucks Button"))) 
		{
			starbucksMode = !starbucksMode;
			GUIStyle myStarbucksStyle = skinMenu.GetStyle("Starbucks Button");
			if(starbucksMode) {
				myStarbucksStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myStarbucksStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myStarbucksStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myStarbucksStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}

		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 175, (Sheight - curHeight) / 2.0f + 155, curWidth, curHeight), "Les zombis sont speedés!", skinMenu.GetStyle ("Instructions"));

		// Grosseur de la soirée
		curWidth = 200;
		curHeight = 20;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 125, curWidth, curHeight), "Grosseur de la soirée", skinMenu.GetStyle("BPM label"));

		curWidth = 400;
		curHeight = 40;
		grosseurSoiree = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 95, curWidth, curHeight), grosseurSoiree, new GUIContent[5]{new GUIContent("Tupperware"), new GUIContent("Party de fête"), new GUIContent("PU"), new GUIContent("Projet X"), new GUIContent("Wôôôôô")}, 3, skinMenu.GetStyle("List button")); 

		// Longueur de la soirée
		curWidth = 200;
		curHeight = 40;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 35, curWidth, curHeight), "Longueur de la soirée", skinMenu.GetStyle("BPM label"));

		curWidth = 400;
		curHeight = 40;
		longueurSoiree = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 5, curWidth, curHeight), longueurSoiree, new GUIContent[4]{new GUIContent("Courte"), new GUIContent("Moyenne"), new GUIContent("Longue"), new GUIContent("Digne de Guylaine")}, 2, skinMenu.GetStyle("List button")); 

		// Beats par tour
		curWidth = 200;
		curHeight = 40;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f + 65, curWidth, curHeight), "Beats par tour", skinMenu.GetStyle("BPM label"));

		curWidth = 300;
		curHeight = 80;
		beatsParTour = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f + 120, curWidth, curHeight), beatsParTour, new GUIContent[3]{new GUIContent("Équipes dans l'association de l'est"), new GUIContent("Pattes d'Aragog"), new GUIContent("Nombre de doigts sur un gars qui a pas de pouce")}, 1, skinMenu.GetStyle("List button")); 

	}

	public void renderOptionsEnglish() {
		// Background
		GUI.DrawTexture (new Rect ((Swidth - 600) / 2.0f + optionsOffset, (Sheight - 400) / 2.0f, 600, 400), bgExtras);
		
		float curWidth = 300;
		float curHeight = 40;
		GUIStyle mySongStyle = skinMenu.GetStyle ("Song label");
		mySongStyle.normal.textColor = textColor;
		
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset, (Sheight - curHeight) / 2.0f - 170, curWidth, curHeight), "Extras", mySongStyle);
		
		curWidth = 32;
		curHeight = 32;
		if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset + 300, (Sheight - curHeight) / 2.0f - 200, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
			optionsMode = false;
			animator.SetBool (Animator.StringToHash("OptionsOpen"), optionsMode);
		}
		
		// Fever mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) - 120, curWidth, curHeight), "fever mode", skinMenu.GetStyle("Fever Button"))) 
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
		
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f - 85, curWidth, curHeight), "Close The Lights!", skinMenu.GetStyle ("Instructions"));
		
		// Chaos mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) - 40, curWidth, curHeight), "chaos mode", skinMenu.GetStyle("Chaos Button"))) 
		{
			chaosMode = !chaosMode;
			GUIStyle myChaosStyle = skinMenu.GetStyle("Chaos Button");
			if(chaosMode){
				myChaosStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myChaosStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myChaosStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myChaosStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}
		
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f - 5, curWidth, curHeight), "Zombies are uncontrollable!", skinMenu.GetStyle ("Instructions"));
		
		// Tactic mode
		curWidth = 150;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 200, ((Sheight - curHeight) / 2.0f) + 40, curWidth, curHeight), "tactic mode", skinMenu.GetStyle("Tactic Button"))) 
		{
			tacticMode = !tacticMode;
			GUIStyle myTacticStyle = skinMenu.GetStyle("Tactic Button");
			if(tacticMode){
				myTacticStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myTacticStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myTacticStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myTacticStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}
		
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 200, (Sheight - curHeight) / 2.0f + 75, curWidth, curHeight), "Diagonals every 5 moves!", skinMenu.GetStyle ("Instructions"));
		
		// Starbucks mode
		curWidth = 200;
		curHeight = 40;
		if (GUI.Button(new Rect(((Swidth - curWidth) / 2.0f) + optionsOffset - 175, ((Sheight - curHeight) / 2.0f) + 120, curWidth, curHeight), "starbucks mode", skinMenu.GetStyle("Starbucks Button"))) 
		{
			starbucksMode = !starbucksMode;
			GUIStyle myStarbucksStyle = skinMenu.GetStyle("Starbucks Button");
			if(starbucksMode) {
				myStarbucksStyle.normal.textColor = new Color (52f/255f,219f/255f,122f/255f);
				myStarbucksStyle.hover.textColor = new Color (52f/255f,219f/255f,122f/255f);
			} else {
				myStarbucksStyle.normal.textColor = new Color (111f/255f,111f/255f,111f/255f);
				myStarbucksStyle.hover.textColor = new Color (0f/255f,99f/255f,235f/255f);
			}
		}
		
		GUI.Label (new Rect ((Swidth - curWidth) / 2.0f + optionsOffset - 175, (Sheight - curHeight) / 2.0f + 155, curWidth, curHeight), "Zombies are faster!", skinMenu.GetStyle ("Instructions"));
		
		// Grosseur de la soirée
		curWidth = 200;
		curHeight = 20;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 125, curWidth, curHeight), "Party Size", skinMenu.GetStyle("BPM label"));
		
		curWidth = 400;
		curHeight = 40;
		grosseurSoiree = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 95, curWidth, curHeight), grosseurSoiree, new GUIContent[5]{new GUIContent("Tupperware"), new GUIContent("Birthday Party"), new GUIContent("PU"), new GUIContent("Project X"), new GUIContent("Woooooah")}, 3, skinMenu.GetStyle("List button")); 
		
		// Longueur de la soirée
		curWidth = 200;
		curHeight = 40;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 35, curWidth, curHeight), "Party Length", skinMenu.GetStyle("BPM label"));
		
		curWidth = 400;
		curHeight = 40;
		longueurSoiree = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f - 5, curWidth, curHeight), longueurSoiree, new GUIContent[4]{new GUIContent("Short"), new GUIContent("Medium"), new GUIContent("Long"), new GUIContent("Bertha Worthy")}, 2, skinMenu.GetStyle("List button")); 
		
		// Beats par tour
		curWidth = 200;
		curHeight = 40;
		GUI.Label (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f + 65, curWidth, curHeight), "Beats par tour", skinMenu.GetStyle("BPM label"));
		
		curWidth = 300;
		curHeight = 80;
		beatsParTour = GUI.SelectionGrid (new Rect((Swidth - curWidth) / 2.0f + optionsOffset + 100, (Sheight - curHeight) / 2.0f + 120, curWidth, curHeight), beatsParTour, new GUIContent[3]{new GUIContent("Days Of Christmas"), new GUIContent("Amount of fingers on a guy without thumbs"), new GUIContent("Number of Beatles")}, 1, skinMenu.GetStyle("List button")); 
		
	}

	public void renderTutorial() {
		float curWidth = 1000f;
		float curHeight = 557f;
		GUI.DrawTexture(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), tutorials[tutorialIndex]);

		curWidth = 64;
		curHeight = 64;
		if (tutorialIndex > 0) {
			if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f - 500, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", GUI.skin.GetStyle ("Previous song"))) {
				tutorialIndex = Mathf.Max (0, tutorialIndex - 1);
			}
		}

		if (tutorialIndex < tutorials.Count - 1) {
			if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + 500, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", GUI.skin.GetStyle ("Next song"))) {
				tutorialIndex = Mathf.Min (tutorials.Count - 1, tutorialIndex + 1);
			}
		}

		curWidth = 32;
		curHeight = 32;
		if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + 500, (Sheight - curHeight) / 2.0f - 279, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
			tutorialMode = false;
		}
	}

	public void renderTutorialEnglish() {
		float curWidth = 1000f;
		float curHeight = 557f;
		GUI.DrawTexture(new Rect((Swidth - curWidth) / 2.0f, (Sheight - curHeight) / 2.0f, curWidth, curHeight), tutorials_EN[tutorialIndex]);
		
		curWidth = 64;
		curHeight = 64;
		if (tutorialIndex > 0) {
			if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f - 500, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", GUI.skin.GetStyle ("Previous song"))) {
				tutorialIndex = Mathf.Max (0, tutorialIndex - 1);
			}
		}
		
		if (tutorialIndex < tutorials.Count - 1) {
			if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + 500, (Sheight - curHeight) / 2.0f, curWidth, curHeight), "", GUI.skin.GetStyle ("Next song"))) {
				tutorialIndex = Mathf.Min (tutorials_EN.Count - 1, tutorialIndex + 1);
			}
		}
		
		curWidth = 32;
		curHeight = 32;
		if (GUI.Button (new Rect ((Swidth - curWidth) / 2.0f + 500, (Sheight - curHeight) / 2.0f - 279, curWidth, curHeight), "", GUI.skin.GetStyle ("Close button"))) {
			tutorialMode = false;
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
		persistentScript.OptAiDifficulty = aiDifficulty;

		persistentScript.OptChaosMode = chaosMode;
		persistentScript.OptTacticMode = tacticMode;
		persistentScript.OptZombieCtrl = (starbucksMode) ? 2 : 1;

		int zombieCount = 50;
		switch (grosseurSoiree) {
				case 0:
						zombieCount = 50;
						break;
				case 1:
						zombieCount = 100;
						break;
				case 2:
						zombieCount = 200;
						break;
				case 3:
						zombieCount = 500;
						break;
				case 4:
						zombieCount = 1000;
						break;
		}
		persistentScript.OptZombiesCount = zombieCount;
		persistentScript.OptBaseScore = (longueurSoiree + 1) * 500;
		persistentScript.OptSpeedTurn = (int)Mathf.Pow (2, 2-beatsParTour) * 4;
		
		if(feverMode == false) Application.LoadLevel("Game");
		else Application.LoadLevel ("FeverMode");
    }
}
