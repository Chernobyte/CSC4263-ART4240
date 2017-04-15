using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour {

	public Vector3 screenPos;

	private Vector3 charSize;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponentsInChildren<SpriteRenderer> () [1];
		//this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		screenPos = Camera.main.WorldToScreenPoint (transform.position);
		screenPos.y = Screen.height - screenPos.y;
	}

	void OnGUI()
	{
		//boxSize = Camera.main.WorldToScreenPoint(gameObject.GetComponentsInChildren<SpriteRenderer> () [1].bounds.size);
		Vector3 tmp1 = new Vector3(sr.bounds.max.x, sr.bounds.max.y, 0);
		Vector3 tmp2 = new Vector3(sr.bounds.min.x, sr.bounds.min.y, 0);
		charSize = Camera.main.WorldToScreenPoint (tmp1) - Camera.main.WorldToScreenPoint (tmp2);

		//the size of the shop menu defined as a Rect
		Rect shopRect = new Rect (	screenPos.x - charSize.x / 2, 
			screenPos.y - charSize.y * 1.5f - 20f, 
			charSize.x, 
			charSize.y);

		//GUI.Label(labelRect, "Health");
		GUI.Box (shopRect, "SHOP");

		/*GameObject[] shopItems;
		foreach (GameObject item in shopItems) 
		{

		}*/
	}

	void HandleInput()
	{

	}
}
