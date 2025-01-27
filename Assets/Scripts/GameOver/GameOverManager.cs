using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
	public Text scoreText;
	public GameObject record;

	private void Start()
	{
		int score = PlayerPrefs.GetInt("score", 0);
		int highscore = PlayerPrefs.GetInt("highscore", 0);

		scoreText.text = score.ToString();

		if (score > highscore)
		{
			record.SetActive(true);
			PlayerPrefs.SetInt("highscore", score);
		}

		if (SoundManager.instance != null)
			SoundManager.instance.SwitchBackgroudMusic(false);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			RestartGame();
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(1);
	}
}
