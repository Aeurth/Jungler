using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Info")]
    public int NPC_ID;
    public string npcName;
    public float interactionRange = 3f;
    private Inventory inventory;

    private Transform player;
    private bool isPlayerInRange = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        player = GameManager.Instance.GetPlayer().transform;
    }

    void Update()
    {
        if (player == null) return;

        float sqrDist = (transform.position - player.position).sqrMagnitude;
        float sqrRange = interactionRange * interactionRange;
        bool wasInRange = isPlayerInRange;
        isPlayerInRange = sqrDist <= sqrRange;

        if (isPlayerInRange && !wasInRange)
        {
            UIManager.Instance.OnPlayerNearNPC(npcName);
            EventManager<string>.Trigger("PlayerNearNPC", npcName);
        }

        if (!isPlayerInRange && wasInRange)
        {
            EventManager<string>.Trigger("PlayerLeftNPC", npcName);
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EventManager<Inventory>.Trigger("NPCShopOpened", inventory);
        }
    }
}
