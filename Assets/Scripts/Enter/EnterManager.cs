using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterManager : MonoBehaviour
{
	public Text highscoreText;

	private void Start()
	{
		highscoreText.text = PlayerPrefs.GetInt("highscore", 0).ToString();
	}

	private void Update()
	{
		if(Input.GetKey(KeyCode.Space))
		{
			StartGame();
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
}
