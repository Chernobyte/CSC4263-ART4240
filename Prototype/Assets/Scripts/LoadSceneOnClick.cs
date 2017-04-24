using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
	{
		//Load Scene by build settings index
		SceneManager.LoadScene (sceneIndex);

		//if we pause the game and restart we need to return the game back to
		//normal
		if(Time.timeScale==0){
			Time.timeScale = 1;
		}
	}
}