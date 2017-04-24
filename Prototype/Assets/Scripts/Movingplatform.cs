using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingplatform : MonoBehaviour {

	public GameObject platform;
	public float moveSpeed;
	public Transform currentPoint;
	public Transform[] points;
	public int pointSelection = 0;

	void Start () {
		currentPoint = points [pointSelection];
	}
	
	// Update is called once per frame
	void Update () {

		platform.transform.position = Vector3.MoveTowards (platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);


		//after we move towards the position we want the platform to move back
		if (platform.transform.position == currentPoint.position) {
			pointSelection = Random.Range (0, 10);
			currentPoint = points [pointSelection];
		}
	}
}
