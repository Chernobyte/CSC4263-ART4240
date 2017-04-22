using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShopItem : ScriptableObject {
	public GameObject bulletPrefab;
	public string itemName;
	public int cost;
	public string description;

	public float fireRate;
	public int damage;
	public float bulletForce;
}
