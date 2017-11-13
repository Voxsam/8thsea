using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour {

	public static PlayerList Obj;
	public int numPlayers
    {
        get { return playerList.Count; }
    }
	public List<Player> playerList;

	void Awake ()
	{
		// Passes list on to next scene.
		if (Obj == null ) {
			Obj = this;
			playerList = new List<Player> ();

		} else {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (this);

	}

	public void AddPlayer (Player p)
	{
		playerList.Add (p);
	}

}



