using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GravityBody : MonoBehaviour
{
	public static List<GravityBody> listBodies;

	public float density = 0.2f;
	public float rotateSpeed = 40f;
	public GameObject obstacle;
	public Transform gravityWaves;

	private AudioSource sound;
	private GameManager gameManager;

	[HideInInspector]
	public float radius;

	private float wavesStartRadius;

	// NOTE : don't use Start because pooling

	private void Awake()
	{
		sound = GetComponent<AudioSource>();

		sound.volume = 0;
	}

	private void Start()
	{
		gameManager = GameManager.instance;
	}

	private void Update()
	{
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
		gravityWaves.LookAt(Camera.main.transform.position, -Vector3.up);

		gravityWaves.transform.localScale -= new Vector3(0.5f, 0.5f, 0) * Time.deltaTime; // TODO : hardcoded values

		if (gravityWaves.transform.localScale.x < obstacle.transform.localScale.x / 2)
			gravityWaves.transform.localScale = new Vector3(wavesStartRadius, wavesStartRadius, 0);

		if(gameManager.player != null) // TODO : no sound after player death ?
		{ 
			float distance = (gameManager.player.transform.position - transform.position).magnitude;

			float distanceMaxVol = 10f;
			float distanceMinVol = 30f;
			float maxVol = 1f;

			if (distance < distanceMinVol)
			{
				if (!sound.isPlaying)
					sound.Play();

				float vol = 1 - (distance - distanceMaxVol) / (distanceMinVol - distanceMaxVol); // inverse proportionality with borders

				sound.volume = Mathf.Clamp(vol, 0, maxVol);
			}
			else
			{
				sound.Stop();
			}
		}
	}

	public void UpdateGravityWaves()
	{
		SpriteRenderer sr = gravityWaves.GetComponent<SpriteRenderer>();

		sr.color = Color.Lerp(Color.white, Color.red, density);
	}

	public void SetRadius(float r)
	{
		float diameter = r * 2; // radius * 2 = diameter
		radius = r;
		wavesStartRadius = radius + 1;

		gravityWaves.transform.localScale = new Vector3(wavesStartRadius, wavesStartRadius, wavesStartRadius);
		obstacle.transform.localScale = new Vector3(diameter, diameter, diameter);
	}

	public float GetMass()
	{
		return density * radius;
	}

	public void Remove()
	{
		Destroy(gameObject);
	}

	private void OnEnable()
	{
		if (listBodies == null)
			listBodies = new List<GravityBody>();

		listBodies.Add(this);
	}

	private void OnDisable()
	{
		listBodies.Remove(this);

		sound.Stop();
	}
}
