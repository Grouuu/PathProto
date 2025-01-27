using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
	private PlayerMovement mov;

	private void Awake()
	{
		mov = GetComponent<PlayerMovement>();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			mov.AcceleratePlayer();
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			mov.DeceleratePlayer();
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			mov.RotatePlayer(1);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			mov.RotatePlayer(-1);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			if (GameManager.instance.UseBoost())
			{
				mov.BoostPlayer();
			}
			else
				mov.isBoosting = false;
		}
		else
			mov.isBoosting = false;
	}
}
