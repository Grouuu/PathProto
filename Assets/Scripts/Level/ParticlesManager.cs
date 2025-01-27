using UnityEngine;

[RequireComponent(typeof(AmbiantParticles))]
[RequireComponent(typeof(ScrollFlamesParticles))]
[RequireComponent(typeof(ImpactParticles))]
public class ParticlesManager : MonoBehaviour
{
	private AmbiantParticles ambiantScript;
	private ScrollFlamesParticles scrollFlamesScript;
	private ImpactParticles impactScript;

	private void Awake()
	{
		ambiantScript = GetComponent<AmbiantParticles>();
		scrollFlamesScript = GetComponent<ScrollFlamesParticles>();
		impactScript = GetComponent<ImpactParticles>();
	}

	public void UpdateParticles(GameObject player)
	{
		if(player != null)
		{
			ambiantScript.UpdateParticles(player.transform.position);
			scrollFlamesScript.UpdateParticles(player.transform.position);
		}
	}

	public void AddImpactParticles(Collision collision)
	{
		impactScript.AddImpact(collision.contacts[0]);
	}
}

// used in ambiant and scroll particles scripts
public class BoxData
{
	public GameObject ob;
	public int x;
	public int y;
}
