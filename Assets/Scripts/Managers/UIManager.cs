
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text ScoreText;

	private void Start()
	{
		GameManager.Instance.ScoreChanged += OnScoreChanged;
	}

	private void OnDestroy()
	{
		GameManager.Instance.ScoreChanged -= OnScoreChanged;
	}

	private void OnScoreChanged(int score)
	{
		ScoreText.text = $"Score : {score}";
	}
}
