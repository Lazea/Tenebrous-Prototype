using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowTorchTool : MonoBehaviour
{
    bool useTorch;

    Controls.GameplayActions controls;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.started += ctx => useTorch = true;
        controls.Action.canceled += ctx => useTorch = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        useTorch = false;
        anim.SetBool("BlowTorching", false);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("BlowTorching", useTorch);
    }

    public void Repair()
    {
        // TODO: Repair ship using a raycast
        Debug.Log("Repair");
    }
}
