using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Submarine : Interactable
{
    public GameObject cockpit;
    public bool playerInControl;

    public Transform playerDismountPoint;
    GameObject player;

    [Header("Events")]
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    // Components
    Controls.SubGameplayActions controls;

    private void Awake()
    {
        interactableType = InteractableType.Submarine;

        player = GameObject.Find("Player");

        controls = new Controls().SubGameplay;
        controls.ExitPilot.performed += ctx => PlayerExit();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void PlayerExit()
    {
        if (!playerInControl)
            return;

        playerInControl = false;

        player.transform.position = playerDismountPoint.position;
        player.transform.localRotation = playerDismountPoint.localRotation;
        player.SetActive(true);
        cockpit.SetActive(false);

        onPlayerExit.Invoke();
    }

    public override void Interact()
    {
        if (playerInControl)
            return;

        player.SetActive(false);
        cockpit.SetActive(true);

        playerInControl = true;
        onPlayerEnter.Invoke();
    }
}
