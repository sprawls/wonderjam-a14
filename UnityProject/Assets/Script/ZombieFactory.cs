using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieFactory : Singleton<ZombieFactory>, IBeatReceiver , IUpdate {
	protected ZombieFactory () {}

	public List<ZombieBehaviour>  Zombies;
	public GameObject TileParticlesColor_blue;
	public GameObject TileParticlesColor_green;

	private GameObject ZombiePrefab;
	private GameObject ZombieFeverPrefab;
	private GameObject emptyObject;
	private GameObject zombieAnchorObject;
	private IBeatReceiver IbeatReceiverRef;
	private int NumZombies = 50;


	public void Start(){
		TileParticlesColor_green = (GameObject) Resources.Load("TileSplash_green");
		TileParticlesColor_blue = (GameObject) Resources.Load("TileSplash_blue");
	}

	public void OnUpdate(){

	}

	private void SetZombieAmount(){
		NumZombies = GameManager.Instance.getZombieCount();
	}

	public void SetBeat(IBeatReceiver Beat){
		IbeatReceiverRef = Beat;

		ZombiePrefab = (GameObject) Resources.Load ("prefab/ZombiePrefab");
		ZombieFeverPrefab = (GameObject) Resources.Load ("prefab/ZombieFeverPrefab");
		zombieAnchorObject = (GameObject) Resources.Load ("prefab/ZombieAnchor");
		Zombies = new List<ZombieBehaviour>();
		//instantiate empty game object for grid
		Instantiate (zombieAnchorObject);
		//Set Zombie Amount Depending on options
		SetZombieAmount();
		emptyObject = GameObject.Find("ZombieAnchor(Clone)");

		for(int i = 0; i < NumZombies; i++) {
			if(Application.loadedLevel == 2) Zombies.Add (((GameObject)Instantiate(ZombieFeverPrefab)).GetComponent<ZombieBehaviour>());
			else Zombies.Add (((GameObject)Instantiate(ZombiePrefab)).GetComponent<ZombieBehaviour>());		
			Zombies[i].transform.parent = emptyObject.transform;
			Zombies[i].transform.localRotation = Quaternion.Euler(new Vector3(90,180,0)); //Orient sprites with camera
		}

		// Register to beat
		GameManager.Instance.requestBeat (this);
	}

    public void OnQuarterBeat()
    {
    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool p1turn){
		for(int i = 0; i < Zombies.Count; i++) {
			Zombies[i].OnBeat (p1,p2,p1turn);
		}
	}


	

	void Update () {
	
	}

	public void OnPeinture(int[] tile,int cur_col){
		Vector3[] tilesPositions = GetTileList(tile);
		CreateTileParticles(tilesPositions, cur_col);
		for(int i = 0; i < Zombies.Count; i++) {
			for(int j = 0; j < tilesPositions.Length; j++){
				if(Zombies[i].transform.position.x < (tilesPositions[j].x+5) 
				   && Zombies[i].transform.position.x > (tilesPositions[j].x-5)
				   && Zombies[i].transform.position.z < (tilesPositions[j].z+5) 
				   && Zombies[i].transform.position.z > (tilesPositions[j].z-5)) {
					Zombies[i].ChangeType(cur_col);
					//Debug.Log ("Changed Type of " + i+ "    to Type : " + cur_col);
				}
			}
		}
	} 

	private Vector3[] GetTileList(int[] tiles) {
		Vector3[] myTilesPositions = new Vector3[tiles.Length];
		for(int i=0; i< tiles.Length; i++){
			myTilesPositions[i] = GameManager.Instance.getTile(tiles[i]).transform.position;
		}
		return myTilesPositions;
	}

	private void CreateTileParticles(Vector3[] positions, int blueOrGreen){
		GameObject ToInstantiate;
		if(blueOrGreen == 1) {
			ToInstantiate = TileParticlesColor_green;
		} else {
			ToInstantiate = TileParticlesColor_blue;
		}
		for(int i=0; i< positions.Length; i++) {
			Instantiate (ToInstantiate, positions[i], Quaternion.identity);
		}
	}

}
