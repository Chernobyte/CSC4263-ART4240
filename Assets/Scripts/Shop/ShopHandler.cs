using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopHandler : MonoBehaviour {

	public Vector3 screenPos;
	public List<ShopItem> shopList;

	private Vector3 charSize;
	private SpriteRenderer sr;
	private PlayerController pc;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponentsInChildren<SpriteRenderer> () [1];
		pc = gameObject.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		screenPos = Camera.main.WorldToScreenPoint (transform.position);
		screenPos.y = Screen.height - screenPos.y;

		HandleInput ();
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

		/*Rect shopRect = new Rect (	screenPos.x - charSize.x / 1.5f, 
									screenPos.y - charSize.y * 1.75f - 20f, 
									charSize.x * 1.5f, 
									charSize.y * 1.4f);*/

		Rect shopRect = new Rect (	screenPos.x - Screen.width / 16f, //screenPos.x - charSize.x / 1.5f, 
			screenPos.y - charSize.y * 1.75f - 20f,
			Screen.width / 8f,
			Screen.height / 3f);

		GUI.Box (shopRect, "");

		GUIStyle custom = new GUIStyle ("button");
		custom.fontSize = 8;


		if(shopList.Count == 1) 
		{
			GUILayout.BeginArea (new Rect (shopRect.x + 5f, shopRect.y * 2 - 5f, shopRect.width - 10f, shopRect.height / 3 + 10f));

			GUILayout.Space (10f);

			GUILayout.BeginHorizontal ();

			GUILayout.Button (shopList[0].name);
			GUILayout.Space (5f);
			GUILayout.Button (shopList[0].cost.ToString());

			GUILayout.EndHorizontal ();

			//GUILayout.Space (5f);
			GUILayout.Button (shopList[0].description, custom);
			GUILayout.Space (10f);
			GUILayout.EndArea ();
		}
		else if(shopList.Count > 0) 
		{
			GUILayout.BeginArea (new Rect (shopRect.x + 5f, shopRect.y - 5f, shopRect.width - 10f, shopRect.height + 10f));
			GUILayout.BeginVertical ();

			GUILayout.Space (10f);

			GUILayout.BeginHorizontal ();

			GUILayout.Button (shopList[0].name);
			GUILayout.Space (5f);
			GUILayout.Button (shopList[0].cost.ToString());

			GUILayout.EndHorizontal ();

			//GUILayout.Space (5f);
			GUILayout.Button (shopList[0].description, custom);
			GUILayout.Space (10f);

			if(shopList.Count > 1)
			{
				GUILayout.BeginHorizontal ();

				GUILayout.Button (shopList[1].name);
				GUILayout.Space (5f);
				GUILayout.Button (shopList[1].cost.ToString());

				GUILayout.EndHorizontal ();

				//GUILayout.Space (5f);
				GUILayout.Button (shopList[1].description, custom);
				GUILayout.Space (10f);
		
				if(shopList.Count > 2)
				{
					GUILayout.BeginHorizontal ();

					GUILayout.Button (shopList[2].name);
					GUILayout.Space (5f);
					GUILayout.Button (shopList[2].cost.ToString());

					GUILayout.EndHorizontal ();

					//GUILayout.Space (5f);
					GUILayout.Button (shopList[2].description, custom);
					GUILayout.Space (10f);
				}
			}
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}
	}

	void HandleInput()
	{
		// W/I & S/K control scrolling
		//fire key to select, which substracts cost if can afford and removes ShopItem from shopList
		bool up, down, confirm;
		ShopItem tmp;

		// Player 1 controls
		if(pc.isP1)
		{
			up 		= Input.GetKeyDown(KeyCode.W);
			down 	= Input.GetKeyDown(KeyCode.S);
			confirm	= Input.GetKeyDown(KeyCode.F);
		}
		// Player 2 controls
		else
		{
			up 		= Input.GetKeyDown(KeyCode.I);
			down 	= Input.GetKeyDown(KeyCode.K);
			confirm	= Input.GetKeyDown(KeyCode.Semicolon);
		}

		//rearrange list if given input
		if(shopList.Count > 1)
		{
			if (up) 
			{
				shopList.Reverse ();
				tmp = shopList [0];
				shopList.RemoveAt (0);
				shopList.Add(tmp);
				shopList.Reverse ();
			} 
			else if (down) 
			{
				tmp = shopList [0];
				shopList.RemoveAt (0);
				shopList.Add(tmp);
			}

			//remove item if purchased
			if(confirm && pc.currentCurrency >= shopList[1].cost)
			{
				pc.weapons.Add (shopList [1]);
				pc.currentWeapon = pc.weapons.Count - 1; //switches current weapon to this weapon
				pc.currentCurrency -= shopList[1].cost;
				//pc.currentWeaponTxt.text = pc.weapons [pc.currentWeapon].ToString ().Replace(" (ShopItem)", "");
				shopList.RemoveAt (1);
			}
		} 
		//remove item if purchased
		else if (shopList.Count > 0 && confirm && pc.currentCurrency >= shopList[0].cost) 
		{
			pc.weapons.Add (shopList [0]);
			pc.currentWeapon = pc.weapons.Count - 1; //switches current weapon to this weapon
			pc.currentCurrency -= shopList[0].cost;
			//pc.currentWeaponTxt.text = pc.weapons [pc.currentWeapon].ToString ().Replace(" (ShopItem)", "");
			shopList.RemoveAt (0);
		}
	}
}
