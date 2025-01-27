using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactParticles : MonoBehaviour
{
	public GameObject impactParticlesPrefab;
	public Transform parent;

	public void AddImpact(ContactPoint contact)
	{
		GameObject impact = Instantiate(impactParticlesPrefab, contact.point, Quaternion.FromToRotation(Vector3.forward, Vector3.Reflect(contact.point, contact.normal)), parent);
	}
}
