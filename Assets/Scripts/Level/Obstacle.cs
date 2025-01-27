using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
			GameManager.instance.PlayerHitObstacle(collision);
	}
}
