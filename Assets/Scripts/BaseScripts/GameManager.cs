using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private WorldChunk world;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        limitFPS();

        if (world == null || player == null || uiManager == null)
        {
            Debug.LogError("GameManager is missing references.");
            return;
        }
        Debug.Log("GameManager started");
        world.GenerateMap();
        player.SpawnAtStart(playerSpawnPoint);
        uiManager.Init();
    }

    private void limitFPS()
    {
        // Disable VSync to use targetFrameRate
        QualitySettings.vSyncCount = 0;

        // Set target frame rate to 60 FPS
        Application.targetFrameRate = 60;
    }
    public Inventory GetPlayerInventory()
    {
        return player.GetInventory();//refactor in the future to use a better way to get the player inventory
    }
    public Inventory GetPlayerEquipment()
    {
        return player.GetEquipment();
    }
    public Player GetPlayer() { return player; }
    public bool IsPlayerAlive()
    {
        if (player.GetPlayerState() == PlayerState.Dead)
        {
            return false;
        }
        return true;
    }
    public Camera GetMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("No main camera found.");
            }
        }
        return mainCamera;
    }
}
