using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public delegate void OnScoreChanged(int score);
	public OnScoreChanged ScoreChanged;

    [HideInInspector]
    public InputManager InputManager;
	[HideInInspector]
	public WaveManager WaveManager;
	[HideInInspector]
	public PoolManager PoolManager;

    public WeaponData[] WeaponDatas;
    public GameObject FloatingDamagePrefab;
    public GameObject DamageParticleSystem;

    public CinemachineVirtualCamera VirtualCamera;
    public GameObject PlayerPrefab;

    private GameObject playerInstance;

    private int points;

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

        InputManager = gameObject.GetComponent<InputManager>();
        WaveManager = gameObject.GetComponent<WaveManager>();
        PoolManager = gameObject.GetComponent<PoolManager>();

        DontDestroyOnLoad(gameObject);
		WaveManager.EnemyDied += OnEnemyDiedEvent;
	}

	private void OnDestroy()
	{
		WaveManager.EnemyDied -= OnEnemyDiedEvent;
	}

	private void Start()
	{
		playerInstance = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        VirtualCamera.Follow = playerInstance.transform;
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

        points++;
        ScoreChanged.Invoke(points);
	}

    public void DisplayDamageParticuleSystem(Vector3 position)
    {
        PoolManager.GetInstanciedPrefab(DamageParticleSystem, position + Vector3.up / 2, typeof(DamagePSRecycler));
    }
}
