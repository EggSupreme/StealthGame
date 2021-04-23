using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum PlayerInteractableType
{
    arrow,
    rock,
    ammoStation,
}

class InteractableData
{
    public InteractableData(string _popupText, Vector3 _popupWorldOffset)
    {
        popupText = _popupText;
        popupWorldOffset = _popupWorldOffset;
    }

    public string popupText = "Interact";
    public Vector3 popupWorldOffset = Vector3.zero;
}

public class PlayerInteractable : MonoBehaviour
{
    static InteractableData[] dataArray = { new InteractableData("Collect", new Vector3(0.0f, 0.2f, 0.0f)), null, new InteractableData("Search", new Vector3(0.0f, 0.8f, 0.0f)) };
    InteractableData MyTypeData() { return dataArray[(int)type]; }

    public GameObject popupParent = null;
    public Vector3 popupWorldOffset = Vector3.zero;

    static GameObject popupPrefab = null;
    public GameObject _popupPrefab;
    GameObject popup;

    [SerializeField]
    PlayerInteractableType type = PlayerInteractableType.arrow;

    public PlayerInteractableType GetInteractableType() { return type; }
    public void ShowPopup(bool _shouldShow) { popup.SetActive(_shouldShow); }
    public Transform PopupTransform() { return popup.transform; }

    private void Awake()
    {
        if (!popupPrefab && _popupPrefab)
        {
            popupPrefab = _popupPrefab;
        }
    }

    private void Start()
    {
        if (!_popupPrefab)
        {
            popup = Instantiate(popupPrefab, transform.position, transform.rotation, popupParent.transform);
            popup.GetComponentInChildren<Text>().text = MyTypeData().popupText;
            popup.transform.Translate(MyTypeData().popupWorldOffset, Space.World);
            ShowPopup(false);
        }
    }
}
