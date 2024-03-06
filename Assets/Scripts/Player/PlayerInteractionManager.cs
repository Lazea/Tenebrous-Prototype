using UnityEngine;
using SOGameEventSystem.Events;

public class PlayerInteractionManager : MonoBehaviour
{
    public float interactionRange = 4f;
    public float interactionRadius = 1.5f;
    public Interactable interactable;

    public LayerMask mask;

    Controls.GameplayActions controls;

    [Header("Events")]
    public StringGameEvent onInteractable;

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
            interactable = hit.collider.gameObject.GetComponentInParent<Interactable>();

            if(interactable != null)
            {
                string contextMsg = "";
                switch (interactable.interactableType)
                {
                    case Interactable.InteractableType.Submarine:
                        contextMsg = "[E] Enter Submarine";
                        break;
                    case Interactable.InteractableType.Mineral:
                        contextMsg = "Mineral"; // TODO: Assign a mineral name once mineral class exists
                        break;
                }
                onInteractable.Raise(contextMsg);
            }
        }
        else
        {
            if (interactable != null)
                onInteractable.Raise("");
            interactable = null;
        }
    }

    public void Interact()
    {
        if (interactable == null)
            return;

        interactable.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 startPoint = Camera.main.transform.position;
        Vector3 endPoint = startPoint + Camera.main.transform.forward * interactionRange;
        Gizmos.DrawWireSphere(endPoint, interactionRadius);
        Gizmos.DrawLine(endPoint, startPoint);
    }
}
