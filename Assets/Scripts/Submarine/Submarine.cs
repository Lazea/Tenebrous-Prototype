using UnityEngine;
using UnityEngine.Events;
using SOGameEventSystem;
using UnityEngine.SceneManagement;

public class Submarine : Interactable, IDamageable
{
    public SubmarineData data;

    public int Health
    {
        get => data.hullHealth;
    }
    public int MaxHealth
    {
        get => data.HullMaxHealth;
    }

    [Header("Cockpit")]
    public GameObject cockpit;
    public bool playerInControl;

    public Transform playerDismountPoint;
    GameObject player;

    [Header("Events")]
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    public BaseGameEvent PlayerEnterSubmarine;
    public BaseGameEvent PlayerExitSubmarine;
    public BaseGameEvent SubmarineDestroyed;

    // Components
    Controls.SubGameplayActions controls;

    private void Awake()
    {
        interactableType = InteractableType.Submarine;

        player = GameObject.Find("Player");

        controls = new Controls().SubGameplay;
        controls.ExitPilot.performed += ctx => PlayerExit();

        data.hullHealth = data.HullMaxHealth;
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
        if (PlayerExitSubmarine != null)
            PlayerExitSubmarine.Raise();
    }

    public override void Interact()
    {
        if (playerInControl)
            return;

        onPlayerEnter.Invoke();
        if (PlayerEnterSubmarine != null)
            PlayerEnterSubmarine.Raise();

        player.SetActive(false);
        cockpit.SetActive(true);

        playerInControl = true;
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
            data.HullMaxHealth);
    }

    [ContextMenu("Destroy Submarine")]
    public void Kill()
    {
        SubmarineDestroyed.Raise();
        Destroy(gameObject);
    }
    #endregion

    #region [Data Reset]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetData();
    }

    private void OnApplicationQuit()
    {
        ResetData();
    }

    void ResetData()
    {
        data.ResetData();
    }
    #endregion
}
