using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public GameObject harpoonProjectilePrefab;
    public float spawnRange = 0.4f;

    // Components
    Controls.GameplayActions controls;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.performed += ctx => StartShootHarpoon();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        anim.ResetTrigger("ShootHarpoon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartShootHarpoon()
    {
        anim.SetTrigger("ShootHarpoon");
    }

    public void ShootHarpoon()
    {
        Debug.Log("Shoot Harpoon");

        Transform camTransform = Camera.main.transform;
        Instantiate(
            harpoonProjectilePrefab,
            camTransform.position + camTransform.forward * spawnRange,
            camTransform.rotation,
            null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = Camera.main.transform.position;
        Vector3 endPoint = Camera.main.transform.forward * spawnRange;
        endPoint += startPoint;
        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawWireSphere(endPoint, 0.1f);
    }
}
