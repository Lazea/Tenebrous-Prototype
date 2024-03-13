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
    bool killed = false;

    [Header("Cockpit")]
    public GameObject cockpit;
    public bool playerInControl;

    public Transform playerDismountPoint;
    public Transform playerDismountPointOnSubDestroy;
    GameObject player;

    [Header("Depth")]
    public bool depthDamageEnabled = true;
    [SerializeField]
    float depth;
    public float Depth { get { return depth; } }

    public float destroyImplosionForce = 30f;

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
        InvokeRepeating("HandleHullDepth", 1f, 1f);
    }

    private void OnDisable()
    {
        controls.Disable();
        CancelInvoke("HandleHullDepth");
    }

    #region [Player Enter/Exit Control]
    public void PlayerExit(Vector3 spawnPos, Quaternion spawnRot)
    {
        if (!playerInControl)
            return;

        playerInControl = false;

        player.transform.position = spawnPos;
        player.transform.localRotation = spawnRot;
        player.SetActive(true);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cockpit.SetActive(false);

        onPlayerExit.Invoke();
        if (PlayerExitSubmarine != null)
            PlayerExitSubmarine.Raise();
    }

    public void PlayerExit()
    {
        PlayerExit(playerDismountPoint.position, playerDismountPoint.localRotation);
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
        if (killed)
            return;
        killed = true;

        EnableDestroyedChunks();
        PlayerExit(
            playerDismountPointOnSubDestroy.position,
            playerDismountPointOnSubDestroy.localRotation);
        SubmarineDestroyed.Raise();
        Destroy(gameObject);
    }

    void EnableDestroyedChunks()
    {
        var chunkRbs = transform.Find("DestroyedChunks").GetComponentsInChildren<Rigidbody>(true);
        foreach(var chunk in chunkRbs)
        {
            chunk.transform.parent = null;
            chunk.gameObject.SetActive(true);
            Vector3 dir = transform.position - chunk.transform.position;
            if(chunk.mass > 3)
                chunk.AddForce(dir.normalized * destroyImplosionForce, ForceMode.Impulse);
        }
    }

    void HandleHullDepth()
    {
        if (!depthDamageEnabled)
            return;

        depth = Ocean.GetSamplePointDepth(transform.position);
        float hullMaxDepth = data.hullDepths[data.hullLevel - 1].maxHullDepth;
        float t = (depth - hullMaxDepth) / 200f;
        t = Mathf.Clamp01(t);
        float damage = data.hullDepths[data.hullLevel - 1].hullDamage.Evaluate(t);
        damage *= data.maxDepthDamage;

        if (damage > 0f)
        {
            DealDamage(null, (int)damage);
        }
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
