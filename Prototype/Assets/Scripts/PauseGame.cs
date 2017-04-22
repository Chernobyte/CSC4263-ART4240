using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {

	public Transform pausemenu;
	public AudioSource audi;


	void Start(){
	}

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
			audi.volume = .1f;
		} else {
			pausemenu.gameObject.SetActive (false);
			Time.timeScale = 1;
			audi.volume = .5f;
		}
	}

}