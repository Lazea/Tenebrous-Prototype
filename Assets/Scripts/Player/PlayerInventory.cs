using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void PickupMineral();

public class PlayerInventory : MonoBehaviour
{
    public PlayerInventoryData inventoryData;

    [Header("Resource Pickup")]
    public float pickupRange = 2f;
    public float pickupSmooth = 1f;
    public float minPickupRange = 0.5f;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var pickups = Physics.OverlapSphere(
            transform.position,
            pickupRange,
            mask);
        if(pickups.Length > 0)
        {
            foreach(var pickup in pickups)
            {
                var mineralChunk = pickup.GetComponent<MineralChunk>();
                if(mineralChunk != null)
                {
                    mineralChunk.Pickup(
                        transform,
                        pickupSmooth,
                        minPickupRange,
                        AddResource);
                }
            }
        }
    }

    void AddResource(int resourceValue, Mineral.MineralType mineralType)
    {
        string mineralPickedUp = "";
        switch(mineralType)
        {
            case Mineral.MineralType.Iron:
                inventoryData.ironResourceCount += resourceValue;
                break;
            case Mineral.MineralType.Quartz:
                inventoryData.quartzResourceCount += resourceValue;
                break;
            case Mineral.MineralType.Gold:
                inventoryData.goldResourceCount += resourceValue;
                break;
            case Mineral.MineralType.Diamond:
                inventoryData.diamondResourceCount += resourceValue;
                break;
        }

        Debug.LogFormat(
            "Picked up {0} mineral {1}",
            resourceValue,
            mineralType.ToString());
    }

    #region [Data Reset]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetInventoryData();
    }

    private void OnApplicationQuit()
    {
        ResetInventoryData();
    }

    void ResetInventoryData()
    {
        inventoryData.ResetData();
    }
    #endregion

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minPickupRange);
    }
}
