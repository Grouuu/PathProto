using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Gravity))]
public class PlayerPath : MonoBehaviour
{
	public int nbStep = 50;
	public int multiplyStep = 2;
	public LineRenderer line;

	private Rigidbody rb;
	private Gravity g;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		g = GetComponent<Gravity>();
	}

	private void Start()
	{
		if (line == null) Debug.LogError("miss LineRenderer reference");
	}

	private void OnDestroy()
	{
		if (line != null)
			Destroy(line.gameObject);
	}

	public void DrawPath(Vector3 velocity, float deltaTime)
	{
		Vector3 vel = velocity;

		Vector3 pos = rb.position;
		float mass = rb.mass;

		List<Vector3> listVel = new List<Vector3>();
		listVel.Add(pos); // actual position

		Vector3 oldPos;
		Vector3 recordedPos;
		Vector3 dir;
		RaycastHit hit;

		float endWidth = line.startWidth;
		float endAlpha = 1;

		recordedPos = pos;

		for (int i = 1; i <= nbStep * multiplyStep; i++)
		{
			oldPos = pos;

			// calculate new velocity
			vel += g.GetGravityForce(pos, mass) * deltaTime;
			
			// update position
			pos += vel * deltaTime;

			if (i % multiplyStep == 1)
			{
				dir = pos - recordedPos; // direction of the new portion
				endWidth -= line.startWidth / (float)nbStep;
				endAlpha -= 1 / (float)nbStep;

				if (Physics.Raycast(recordedPos, dir.normalized, out hit, dir.magnitude))
				{
					// if the new portion collide an obstacle : stop the line at the contact point
					if (hit.collider.tag == "Obstacle")
					{
						listVel.Add(hit.point);
						break;
					}
				}

				recordedPos = pos;

				listVel.Add(pos);
			}
		}
		
		//line.endWidth = endWidth;
		line.endWidth = 0.2f;

		Gradient grad = line.colorGradient;
		GradientColorKey[] colorKeys = grad.colorKeys;
		GradientAlphaKey[] alphaKeys = grad.alphaKeys;

		alphaKeys[1].alpha = endAlpha;

		grad.SetKeys(colorKeys, alphaKeys);

		line.colorGradient = grad;

		line.positionCount = listVel.Count;
		line.SetPositions(listVel.ToArray());
	}
}
