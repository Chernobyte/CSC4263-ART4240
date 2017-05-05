using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour {

	public PlayerController pc;
	public Transform win;
	public Text WhoWins;

	void Start () {
	
		pc = gameObject.GetComponent<PlayerController> ();

	}
	
	// Update is called once per frame
	void Update () {
		Winscreen ();
	}

	public void Winscreen(){

		if(pc.isP1 && pc.isDead && win.gameObject.activeInHierarchy==false){

			win.gameObject.SetActive (true);
			Time.timeScale = 0;
			WhoWins.text = "Player 2 Wins";


		}
		if(!pc.isP1 && pc.isDead && win.gameObject.activeInHierarchy==false){
			win.gameObject.SetActive (true);
			Time.timeScale = 0;
			WhoWins.text = "Player 1 Wins";
		}

	}
}
