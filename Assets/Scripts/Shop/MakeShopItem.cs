using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeShopItem 
{
	[MenuItem("Assets/Create/Shop Item")]
	// Use this for initialization
	public static void Create()
	{
		ShopItem asset = ScriptableObject.CreateInstance<ShopItem> ();
		AssetDatabase.CreateAsset (asset, "Assets/ShopItems/NewShopItem.asset");
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}
}