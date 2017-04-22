using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAdjuster : MonoBehaviour {

	public Slider MySlider;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		OnValueChanged ();
	}

	public void OnValueChanged(){
		AudioListener.volume = MySlider.value;
	}
}
