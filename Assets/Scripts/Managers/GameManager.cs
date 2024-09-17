using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public delegate void OnScoreChanged(int score);
	public OnScoreChanged ScoreChanged;

	public delegate void OnPlayerDied(int score);
	public OnPlayerDied PlayerDied;

	public delegate void OnPlayerLifeChanged(float ratio);
	public OnPlayerLifeChanged PlayerLifeChanged;

	public delegate void OnCollectibleCollected(Collectible collectible);
	public OnCollectibleCollected CollectibleCollected;

	public delegate void OnWeaponChanged(WeaponData weaponData);
	public OnWeaponChanged WeaponChanged;

	[HideInInspector]
    public InputManager InputManager;
	[HideInInspector]
	public WaveManager WaveManager;
	[HideInInspector]
	public PoolManager PoolManager;
	[HideInInspector]
	public CollectibleManager CollectibleManager;
	[HideInInspector]
	public AudioManager AudioManager;

	public WeaponData[] WeaponDatas;
    public GameObject FloatingDamagePrefab;
    public GameObject DamageParticleSystem;

    public CinemachineVirtualCamera VirtualCamera;
    public GameObject PlayerPrefab;

    private GameObject playerInstance;

    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
		}

		SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);

		InputManager = gameObject.GetComponent<InputManager>();
        WaveManager = gameObject.GetComponent<WaveManager>();
        PoolManager = gameObject.GetComponent<PoolManager>();
		CollectibleManager = gameObject.GetComponent<CollectibleManager>();
		AudioManager = gameObject.GetComponent<AudioManager>();

		DontDestroyOnLoad(gameObject);
	}

	private void OnDestroy()
	{
		WaveManager.EnemyDied -= OnEnemyDiedEvent;
		UIManager.GameLaunched -= LaunchGame;
	}

	private void Start()
	{
		WaveManager.EnemyDied += OnEnemyDiedEvent;
        UIManager.GameLaunched += LaunchGame;
	}

    public Vector3 GetPlayerPosition()
    {
        if(playerInstance == null)
        {
            return Vector3.zero;
        }
        else
        { 
            return playerInstance.transform.position; 
        }
    }

	private void OnEnemyDiedEvent(GameObject enemy, bool isKilled)
	{
        if (!isKilled)
        {
            return;
        }

        score++;
        ScoreChanged.Invoke(score);
	}

    private void LaunchGame()
	{
        score = 0;
		playerInstance = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
		VirtualCamera.Follow = playerInstance.transform;
	}

    public void DisplayDamageParticuleSystem(Vector3 position)
    {
        PoolManager.GetInstanciedPrefab(DamageParticleSystem, position + Vector3.up / 2, typeof(DamagePSRecycler));
    }

    public void OnPlayerHit(float lifeRatio)
    {
		PlayerLifeChanged.Invoke(lifeRatio);
        if(lifeRatio <= 0)
        {
            Destroy(playerInstance);
            PlayerDied.Invoke(score);
        }
    }
}
