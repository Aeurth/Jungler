using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum UIContext
{
    Equipment,
    Loot,
    Shop,
    Storage,
    Default
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Interaction Prompt")]
    [SerializeField] private GameObject interactionPromptRoot;
    [SerializeField] private TMP_Text interactionPromptText;

    [Header("UI Windows")]
    [SerializeField] private LootWindowUI lootWindow;
    [SerializeField] private GameObject PlayerInventory;
    [SerializeField] private ShopInventoryUI ShopInventory;
    [SerializeField] private PlayerEquipmentUI playerEquipment;
    [SerializeField] private GameObject respawnPanel;
    [SerializeField] private GameObject ESCMenu;

    public UIContext currentContext = UIContext.Default;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        EventManager<Inventory>.Subscribe("LootBagClicked", OnLootBagClicked);
        EventManager<Inventory>.Subscribe("NPCShopOpened", OnNPCShopOpened);
        EventManager<string>.Subscribe("PlayerNearNPC", OnPlayerNearNPC);
        EventManager<string>.Subscribe("PlayerLeftNPC", OnPlayerLeftNPC);
        EventManager<int>.Subscribe(EventKeys.PlayerDied, OnPlayerDied);
    }

    private void OnDisable()
    {
        EventManager<Inventory>.Unsubscribe("LootBagClicked", OnLootBagClicked);
        EventManager<Inventory>.Unsubscribe("NPCShopOpened", OnNPCShopOpened);
        EventManager<string>.Unsubscribe("PlayerNearNPC", OnPlayerNearNPC);
        EventManager<string>.Unsubscribe("PlayerLeftNPC", OnPlayerLeftNPC);
        EventManager<int>.Unsubscribe(EventKeys.PlayerDied, OnPlayerDied);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ESCMenu.activeSelf)
            {
                ESCMenu.SetActive(false);
                Time.timeScale = 1f; // Resume game
            }
            else
            {
                ESCMenu.SetActive(true);
                Time.timeScale = 0f; // Pause game
            }
        }
    }
    public void Init()
    {
        HideInteractionPrompt();
        HideShopMenu();

        // Optionally reset other UI panels like inventory, health, quests, etc.
        Debug.Log("UI Manager initialized.");
    }


    // INTERACTION PROMPT
    public void ShowInteractionPrompt(string message)
    {
        interactionPromptRoot.SetActive(true);
        interactionPromptText.text = message;
    }

    public void HideInteractionPrompt()
    {
        interactionPromptRoot.SetActive(false);
    }

    // SHOP
    public void ShowShopMenu(Inventory inventory)
    {
        ShopInventory.Show(inventory);
    }

    public void HideShopMenu()
    {
        ShopInventory.Hide();
    }
    public void OnNPCShopOpened(Inventory inventory)
    {
        ShowShopMenu(inventory);
    }

    // LOOT WINDOW
    private void OnLootBagClicked(Inventory data)
    {
        OpenLootWindow(data);
    }
    private void OpenLootWindow(Inventory inventory)
    {
        lootWindow.Show(inventory);
    }

    //PLAYER INVENTORY
    public void ToggleInventory()
    {
        bool isActive = PlayerInventory.activeSelf;
        PlayerInventory.SetActive(!isActive);
    }
    public void TogglePlayerEquipment()
    {
        bool isActive = playerEquipment.gameObject.activeSelf;
        playerEquipment.gameObject.SetActive(!isActive);


    }
    public void OnPlayerNearNPC(string npcName)
    {
        if (npcName != null)
            ShowInteractionPrompt($"Press E to trade with {npcName}");
    }

    public void OnPlayerLeftNPC(string npcName)
    {
        HideInteractionPrompt();
        HideShopMenu();
    }
    public void OnPlayerDied(int respawnTime)
    {
        respawnPanel.SetActive(true);
    }
    

   
}
