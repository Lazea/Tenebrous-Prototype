using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTool : MonoBehaviour
{
    public GameObject explosivePrefab;

    Controls.GameplayActions controls;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.performed += ctx => StartThrowExplosive();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        anim.ResetTrigger("ExplosiveThrow");
    }

    void StartThrowExplosive()
    {
        anim.SetTrigger("ExplosiveThrow");
    }

    public void ThrowExplosive()
    {
        // TODO: Throw explosive prefab
        Debug.Log("Throw Explosive");
    }
}
