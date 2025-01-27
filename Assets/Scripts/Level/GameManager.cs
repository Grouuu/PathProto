using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject player;
	public UIManager uiManager;
	public PoolsManager poolsManager;
	public ObstaclesManager obstacleManager;
	public ParticlesManager particlesManager;
	public ScrollFlamesManager scrollManager;
	public SkyboxManager skyboxManager;
	[Range(0.1f, 2f)]
	public float difficulty = 1f;
	public float difficultyMax = 2f;
	public float difficutySpeed = 0.001f;
	public bool ignoreObstacles = false;
	public bool ignoreParticles = false;
	public bool ignoreScrollFlames = false;
	public bool ignoreDeath = false;

	private float distanceTraveled;
	private float boost = 10f;

	private void Awake()
	{
		if (!instance)
			instance = this;
	}

	private void Start()
	{
		if (SoundManager.instance != null)
			SoundManager.instance.SwitchBackgroudMusic(true);
	}

	private void Update()
	{
		if (player != null)
		{
			distanceTraveled = player.transform.position.x;

			difficulty += difficutySpeed * Time.deltaTime;

			if (difficulty > difficultyMax)
				difficulty = difficultyMax;

			if (!ignoreObstacles)
				obstacleManager.UpdateSpawnObstacles(player.transform.position);

			if (!ignoreParticles)
				particlesManager.UpdateParticles(player);

			if (!ignoreScrollFlames)
				scrollManager.UpdateScroll();

			uiManager.UpdateDistance(distanceTraveled);
		}
	}

	public void PlayerInBoostHalo()
	{
		boost++;
	}

	public bool UseBoost()
	{
		if(boost > 0)
		{
			boost--;
			return true;
		}

		return false;
	}

	public void PlayerHitObstacle(Collision collision)
	{
		PlayerDeath();
		particlesManager.AddImpactParticles(collision);
	}

	public void PlayerHitFlames()
	{
		PlayerDeath();
	}

	//

	private void PlayerDeath()
	{
		if (ignoreDeath)
			return;

		Destroy(player);

		SetScore((int)distanceTraveled);

		StartCoroutine(ChangeScene());
	}

	private IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(2);

		SceneManager.LoadScene(2);
	}

	private void SetScore(int score)
	{
		PlayerPrefs.SetInt("score", score);
	}

	// TOOLS ---------------------------------------------------------------------

	public void DrawDebugSquare(float leftX, float bottomY, float z, float width, float height, float duration = 100f)
	{
		float LX = leftX;
		float RX = LX + width;
		float BY = bottomY;
		float TY = BY + height;
		float Z = z;
		float d = 100f;

		Debug.DrawLine(new Vector3(LX, TY, Z),
									new Vector3(RX, TY, Z), Color.red, d);
		Debug.DrawLine(new Vector3(LX, BY, Z),
						new Vector3(RX, BY, Z), Color.red, d);
		Debug.DrawLine(new Vector3(LX, TY, Z),
						new Vector3(LX, BY, Z), Color.red, d);
		Debug.DrawLine(new Vector3(RX, TY, Z),
						new Vector3(RX, BY, Z), Color.red, d);
	}
}
