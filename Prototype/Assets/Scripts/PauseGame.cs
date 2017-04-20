using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public Transform pausemenu;



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}

	}
	public void Pause(){
		if (pausemenu.gameObject.activeInHierarchy == false) {
			pausemenu.gameObject.SetActive (true);
			Time.timeScale = 0;
		} else {
			pausemenu.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}

}