
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float DampTime = 0.2f;                 // Approximate time for the camera to refocus.
	public float ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
	public float MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
    
	private Camera Camera;                        // Used for referencing the camera.
	private float ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 MoveVelocity;                 // Reference velocity for the smooth damping of the position.

    /* Script Load */
	private void Awake()
	{
        Camera = GetComponentInChildren<Camera>();
	}

    /* Every Frame */
	private void FixedUpdate()
	{
        var players = GameObject.FindGameObjectsWithTag("Player");

        var desiredPosition = FindAveragePlayerPosition(players);
        float targetSize = FindDesiredCameraSize(desiredPosition, players);

        MoveTowardsLocation(desiredPosition);
        ZoomTowardsLocation(targetSize);
    }


	private void MoveTowardsLocation(Vector3 desiredLocation)
	{
		transform.position = Vector3.SmoothDamp(transform.position, desiredLocation, ref MoveVelocity, DampTime);
	}

	private Vector3 FindAveragePlayerPosition(GameObject[] players)
	{
		Vector3 totalPosition = new Vector3 ();
		int numActivePlayers = 0;        

        if (players.Length == 0)
            return transform.position;

        /* sum all player positions */
        foreach (var player in players)
        {
            if (player.activeSelf)
            {
                totalPosition += player.transform.position;
                numActivePlayers++;
            }
        }

        var averagePosition = totalPosition / numActivePlayers;
        averagePosition.y = transform.position.y;
        return averagePosition;
	}

	private void ZoomTowardsLocation(float targetSize)
	{
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, targetSize, ref ZoomSpeed, DampTime);
	}


	private float FindDesiredCameraSize(Vector3 desiredPosition, GameObject[] players)
	{
		var desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
		var size = 0f;

        /* set camera size based on farthest player away from camera */
        foreach (var player in players)
        {
            if (player.activeSelf)
            {
                var targetLocalPos = transform.InverseTransformPoint(player.transform.position);
                var desiredPosToTarget = targetLocalPos - desiredLocalPos;
                size = Mathf.Max(new float[]
                {
                    size,
                    Mathf.Abs(desiredPosToTarget.y),
                    Mathf.Abs(desiredPosToTarget.x) / Camera.aspect
                });
            }
        }
        
		size += ScreenEdgeBuffer;
		size = Mathf.Max(size, MinSize);

		return size;
	}
}