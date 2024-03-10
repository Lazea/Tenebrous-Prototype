using System;
using UnityEngine;

public class MineralChunk : MonoBehaviour
{
    [Header("Stats")]
    public int value;
    [SerializeField]
    Mineral.MineralType mineralType;
    public Mineral.MineralType MineralType {  get { return mineralType; } }

    [Header("Pickup")]
    public float pickupDelayTime;
    bool pickupReady;
    public bool PickupReady { get { return pickupReady; } }
    Transform destination;
    float t;
    float minDistance;
    Action<int, Mineral.MineralType> onPickupCallback;

    // Component
    Rigidbody rb;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetPickupReady", pickupDelayTime);
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(destination != null)
        {
            rb.isKinematic = true;
            collider.enabled = false;

            transform.position = Vector3.Lerp(
                transform.position,
                destination.position,
                t);
            float distance = Vector3.Distance(transform.position, destination.position);
            if(distance <= minDistance)
            {
                onPickupCallback?.Invoke(value, mineralType);
                Destroy(gameObject);
            }
        }
    }

    void SetPickupReady()
    {
        pickupReady = true;
    }

    public void Pickup(
        Transform destination,
        float t,
        float minDistance,
        Action<int, Mineral.MineralType> onPickupCallback)
    {
        if (pickupReady)
        {
            pickupReady = false;
            this.t = t;
            this.minDistance = minDistance;
            this.destination = destination;
            this.onPickupCallback = onPickupCallback;
        }
    }
}
