using UnityEngine;

public class BoostHalo : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.tag == "Player")
			GameManager.instance.PlayerInBoostHalo();
	}
}
