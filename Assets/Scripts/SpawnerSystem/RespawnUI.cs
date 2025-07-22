using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnUI : MonoBehaviour
{
    [SerializeField] private GameObject respawnPanel;
    [SerializeField] private Button respawnButton;
    void OnEnable()
    {
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);
    }
    void OnRespawnButtonClicked()
    {
        respawnPanel.SetActive(false);
        EventManager<int>.Trigger(EventKeys.RespawnPlayer, 0);
    }
}
