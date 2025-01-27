using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform player;
	public float smoothSpeed = 0.125f;

	private Vector3 camOffset;

	private void Start()
	{
		camOffset =  transform.position - player.position;
	}

	private void FixedUpdate()
	{
		//Vector3 desiredPos = target.position + camOffset;
		//Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
		
		if(player != null)
			transform.position = player.position + camOffset;
		//transform.LookAt(target);
	}
}
