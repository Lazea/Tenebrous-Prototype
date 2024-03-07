using UnityEngine;
using UnityEngine.Events;

public class Submarine : Interactable, IDamageable
{
    public SubmarineData data;

    public int Health
    {
        get => data.hullHealth;
    }
    public int MaxHealth
    {
        get => data.hullMaxHealth;
    }

    [Header("Cockpit")]
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

    #region [Player Enter/Exit Control]
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
    #endregion

    #region [Damage]
    public void DealDamage(GameObject damageSource, int damage)
    {
        data.hullHealth = Mathf.Max(
            0,
            data.hullHealth - damage);

        if(data.hullHealth <= 0)
        {
            Kill();
        }
    }

    [ContextMenu("Deal 10 Damage")]
    void Deal10Damage()
    {
        DealDamage(null, 10);
    }

    [ContextMenu("Deal 50 Damage")]
    void Deal50Damage()
    {
        DealDamage(null, 50);
    }

    public void Repair(int repairAmount)
    {
        data.hullHealth = Mathf.Min(
            data.hullHealth + repairAmount,
            data.hullMaxHealth);
    }

    [ContextMenu("Kill")]
    public void Kill()
    {
        Destroy(gameObject);
    }
    #endregion
}
