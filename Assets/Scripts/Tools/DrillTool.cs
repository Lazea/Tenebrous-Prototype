using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillTool : MonoBehaviour
{
    bool useDrill;

    Controls.GameplayActions controls;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.started += ctx => useDrill = true;
        controls.Action.canceled += ctx => useDrill = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        useDrill = false;
        anim.SetBool("Drilling", false);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Drilling", useDrill);
    }

    public void Drill()
    {
        // TODO: Drill surface using a raycast
        Debug.Log("Drill");
    }
}
