using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public grumbleAMP soundPlayer;

	private int indexBackgroundMusic;

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	private void Start()
	{
		StartBackgroundMusic();
	}

	public void StartBackgroundMusic()
	{
		soundPlayer.PlaySong(0, 0);

		indexBackgroundMusic = 0;
	}

	public void SwitchBackgroudMusic(bool inAction)
	{
		if (!inAction && indexBackgroundMusic == 1)
		{
			soundPlayer.CrossFadeToNewLayer(0, 1f);
			indexBackgroundMusic = 0;
		}
		else if (inAction && indexBackgroundMusic == 0)
		{
			soundPlayer.CrossFadeToNewLayer(1, 1f);
			indexBackgroundMusic = 1;
		}
	}
}
