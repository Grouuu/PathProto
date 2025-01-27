using UnityEngine;

public class FlamesHit : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			GameManager.instance.PlayerHitFlames();
	}
}
