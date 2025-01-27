using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	private const float G = 667.4f;

	private List<GravityBody> listGravityBodies;

	private float maxDistanceInterac = 50;

	public Vector3 GetGravityForce(Vector3 pos, float mass)
	{
		Vector3 force = Vector3.zero;

		listGravityBodies = GravityBody.listBodies;

		if (listGravityBodies == null) // if no obstacle
			return force;

		Vector3 dir;
		float dist;
		float forceMagnitude;

		foreach (GravityBody body in listGravityBodies)
		{
			dir = body.transform.position - pos;
			dist = dir.magnitude;

			if (dist > 0 && dist < maxDistanceInterac)
			{
				// F = ((m1 * m2) / d²) * G
				forceMagnitude = G * (body.GetMass() * mass) / Mathf.Pow(dist, 2);

				force += dir.normalized * forceMagnitude;
			}
		}

		return force;
	}
}
