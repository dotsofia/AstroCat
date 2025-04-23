using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public Sprite openSprite;

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        OpenDoor();
    }

    private void OpenDoor()
    {
        SetOpened(true);
        SoundEffectManager.Play("Door");

        GetComponent<BoxCollider2D>().enabled = false;   
    }

    public void SetOpened(bool opened)
    {
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
