using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector]
    public InputManager InputManager;
	[HideInInspector]
	public WaveManager WaveManager;
	[HideInInspector]
	public PoolManager PoolManager;

    public WeaponData[] WeaponDatas;
    public GameObject FloatingDamagePrefab;

    public CinemachineVirtualCamera VirtualCamera;
    public GameObject PlayerPrefab;

    private GameObject playerInstance;

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
	}

	private void Start()
	{
		playerInstance = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        VirtualCamera.Follow = playerInstance.transform;
	}
}
