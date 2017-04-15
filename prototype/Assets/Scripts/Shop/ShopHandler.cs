﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour {

	public Vector3 screenPos;
	public ShopItem[] shopInventory;
	private Vector3 charSize;
	private SpriteRenderer sr;
	//private PlayerController pc;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponentsInChildren<SpriteRenderer> () [1];
		//pc = gameObject.GetComponent<PlayerController> ();
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
		/*Rect shopRect = new Rect (	screenPos.x - charSize.x / 2, 
			screenPos.y - charSize.y * 1.5f - 20f, 
			charSize.x, 
			charSize.y);

		float nameWidth, nameHeight, costWidth, costHeight, descWidth, descHeight;

		nameWidth = shopRect.width * 2f / 3f - 10f;
		nameHeight = shopRect.height / 6f - 5f;



		descWidth = shopRect.width - 10f;

		Rect nameRect1 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect costRect1 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect descRect1 = new Rect (shopRect.x + 5f, shopRect.y, descWidth, shopRect.height);

		Rect nameRect2 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect costRect2 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect descRect2 = new Rect (shopRect.x + 5f, shopRect.y, descWidth, shopRect.height);

		Rect nameRect3 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect costRect3 = new Rect (shopRect.x + 5f, shopRect.y, shopRect.width, shopRect.height);
		Rect descRect3 = new Rect (shopRect.x + 5f, shopRect.y, descWidth, shopRect.height);

		//GUI.Label(labelRect, "Health");
		GUI.Box (shopRect, "SHOP");

		//item 1
		GUI.Box (nameRect1, "name1");
		GUI.Box (costRect1, "$$1");
		GUI.Box (descRect1, "description 1");

		//item 2
		GUI.Box (nameRect1, "name2");
		GUI.Box (costRect1, "$$2");
		GUI.Box (descRect1, "description 2");

		//item 3
		GUI.Box (nameRect1, "name3");
		GUI.Box (costRect1, "$$3");
		GUI.Box (descRect1, "description 3");*/

		Rect shopRect = new Rect (	screenPos.x - charSize.x / 1.5f, 
									screenPos.y - charSize.y * 1.75f - 20f, 
									charSize.x * 1.5f, 
									charSize.y * 1.4f);

		GUI.Box (shopRect, "");

		GUILayout.BeginArea (new Rect(shopRect.x + 5f, shopRect.y - 5f, shopRect.width - 10f, shopRect.height + 10f));
		GUILayout.BeginVertical ();

			GUILayout.Space (10f);

			GUILayout.BeginHorizontal ();

				GUILayout.Button ("Name 1");
				GUILayout.Space (5f);
				GUILayout.Button ("Cost 1");

			GUILayout.EndHorizontal ();

			//GUILayout.Space (5f);
			GUILayout.Button ("desc 1");
			GUILayout.Space (10f);

			GUILayout.BeginHorizontal ();

				GUILayout.Button ("Name 2");
				GUILayout.Space (5f);
				GUILayout.Button ("Cost 2");

			GUILayout.EndHorizontal ();

			//GUILayout.Space (5f);
			GUILayout.Button ("desc 2");
			GUILayout.Space (10f);

			GUILayout.BeginHorizontal ();

				GUILayout.Button ("Name 3");
				GUILayout.Space (5f);
				GUILayout.Button ("Cost 3");

			GUILayout.EndHorizontal ();

			//GUILayout.Space (5f);
			GUILayout.Button ("desc 3");
			GUILayout.Space (10f);

		GUILayout.EndVertical ();
		GUILayout.EndArea ();


		/*GameObject[] shopItems;
		foreach (GameObject item in shopItems) 
		{

		}*/
	}

	void HandleInput()
	{
		// W/up & S/down control scrolling
		//fire key to select, which substracts cost if can afford and removed ShopItem from shopInventory
	}
}
