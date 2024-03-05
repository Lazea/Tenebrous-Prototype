using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public GameObject harpoonProjectilePrefab;

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
        // TODO: Shoot harpoon prefab
        Debug.Log("Shoot Harpoon");
    }
}
