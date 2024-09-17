
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIManager : MonoBehaviour
{
	public delegate void OnGameLaunched();
	public static OnGameLaunched GameLaunched;

	public GameObject GameUI;
	public GameObject MainMenu;
	public GameObject EndScreen;

	public HealthBar PlayerHealthBar;
	public Image WeaponImage;

	public Text EndGameScore;
	public Text EndGameBestScore;

	public Text InGameScore;

	private void Start()
	{
		GameManager.Instance.ScoreChanged += OnScoreChanged;
		GameManager.Instance.PlayerDied += OnPlayerDied;
		GameManager.Instance.PlayerLifeChanged += OnPlayerLifeChanged;
		GameManager.Instance.WeaponChanged += OnWeaponChanged;
	}

	private void OnDestroy()
	{
		GameManager.Instance.ScoreChanged -= OnScoreChanged;
		GameManager.Instance.PlayerDied -= OnPlayerDied;
		GameManager.Instance.PlayerLifeChanged -= OnPlayerLifeChanged;
		GameManager.Instance.WeaponChanged -= OnWeaponChanged;
	}

	public void LaunchGame()
	{
		GameUI.SetActive(true);
		MainMenu.SetActive(false);
		EndScreen.SetActive(false);
		PlayerHealthBar.RatioChanged(1);
		UIManager.GameLaunched.Invoke();
	}

	private void OnScoreChanged(int score)
	{
		InGameScore.text = $"Score : {score}";
	}

	private void OnPlayerDied(int score)
	{
		GameUI.SetActive(false);
		MainMenu.SetActive(false);
		EndScreen.SetActive(true);

		int bestScore = PlayerPrefs.GetInt("Score");
		if (score > bestScore)
		{
			PlayerPrefs.SetInt("Score", score);
		}

		bestScore = score;

		EndGameBestScore.text = $"Best score : {bestScore}";
		EndGameScore.text = $"New score : {score}";
	}

	private void OnPlayerLifeChanged(float ratio)
	{
		PlayerHealthBar.RatioChanged(ratio);
	}

	private void OnWeaponChanged(WeaponData weaponData)
	{
		WeaponImage.sprite = weaponData.WeaponSprite;
	}
}
