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

	public bool IsRightButtonPressedByAnyPlayer () {
		foreach (Player p in playerList) {
			if (p.controls.GetHorizontalAxis () > 0) {
				return true;
			}
		}
		return false;
	}

	public bool IsLeftButtonPressedByAnyPlayer () {
		foreach (Player p in playerList) {
			if (p.controls.GetHorizontalAxis () < 0) {
				return true;
			}
		}
		return false;
	}

	public bool IsActionButtonPressedByAnyPlayer () {
		foreach (Player p in playerList) {
			if (p.controls.GetActionKeyDown ()) {
				return true;
			}
		}
		return false;
	}

	public bool IsMenuButtonPressedByAnyPlayer () {
		foreach (Player p in playerList) {
			if (p.controls.GetMenuKeyDown ()) {
				return true;
			}
		}
		return false;
	}

	public void ResetPlayers () {
		playerList = new List<Player> ();
	}


}



