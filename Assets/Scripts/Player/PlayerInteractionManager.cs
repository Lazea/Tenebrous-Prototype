using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public float interactionRange = 4f;
    public float interactionRadius = 1.5f;
    public GameObject interactableObject;

    public LayerMask mask;

    Controls.GameplayActions controls;

    private void Awake()
    {
        controls = new Controls().Gameplay;

        controls.Interact.started += ctx =>
        {
            Interact();
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward);
        RaycastHit hit;
        if(Physics.SphereCast(ray, interactionRadius, out hit, interactionRange, mask))
        {
            interactableObject = hit.collider.gameObject;
            // TODO: Implement a text msg ui for interaction context
        }
        else
        {
            interactableObject = null;
        }
    }

    public void Interact()
    {
        if (interactableObject == null)
            return;

        // TODO: Implement interact
    }
}
