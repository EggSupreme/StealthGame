using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    KeyCode[] interactableKeycodes = { KeyCode.E, KeyCode.R, KeyCode.T };
    List<PlayerInteractable> currentInteractables = new List<PlayerInteractable> { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteractable _p))
        {
            _p.ShowPopup(true);
            currentInteractables.Add(_p);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteractable _p))
        {
            _p.ShowPopup(false);
            currentInteractables.Remove(_p);
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentInteractables.Count; i++)
        {
            if (currentInteractables[i] == null)
            {
                currentInteractables.RemoveAt(i);
                i--;
            }
            else
            {
                currentInteractables[i].PopupTransform().rotation = Camera.main.transform.rotation;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractables.Count > 0)
        {
            PlayerInteractable chosenInteractable = currentInteractables[0];
            switch (chosenInteractable.GetInteractableType())
            {
                case PlayerInteractableType.arrow:
                    GetComponentInParent<Tools_Abilities>().CollectAmmo(AmmoType.arrow, 1);
                    currentInteractables.Remove(chosenInteractable);
                    Destroy(chosenInteractable.popupParent);
                    break;

                case PlayerInteractableType.rock:
                    GetComponentInParent<Tools_Abilities>().CollectAmmo(AmmoType.rock, 1);
                    currentInteractables.Remove(chosenInteractable);
                    Destroy(chosenInteractable.popupParent);
                    break;

                case PlayerInteractableType.ammoStation:
                    GetComponentInParent<Tools_Abilities>().CollectAmmo(AmmoStation.TakeAll());
                    break;

                case PlayerInteractableType.enemy:
                    chosenInteractable.GetComponentInParent<EnemyController>().Damage(-1);
                    break;
            }
        }
    }
}
