using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerToolsManager : MonoBehaviour
{
    [Tooltip("List of all tools that can be unlocked")]
    public GameObject[] tools;
    [Tooltip("List of all tools that are available to be used")]
    public List<GameObject> availableTools;
    public GameObject currentTool;
    public GameObject previousTool;
    public int i = 0;

    AnimatorStateInfo AnimState { get { return anim.GetCurrentAnimatorStateInfo(0); } }
    public bool IsBusy
    {
        get
        {
            var animState = AnimState;
            return !animState.IsTag("Idle");
        }
    }

    Controls.GameplayActions controls;
    Animator anim;
    public DrillTool drillTool;
    public BlowTorchTool blowTorchTool;
    public HarpoonGun harpoonGun;
    public ExplosiveTool explosiveTool;

    [Header("Events")]
    public UnityEvent<ITool> onToolSwitch = new UnityEvent<ITool>();

    private void Awake()
    {
        controls = new Controls().Gameplay;

        controls.Switch.performed += ctx =>
        {
            float dir = ctx.ReadValue<float>();
            if (dir > 0f)
                SwitchToNextTool();
            else if(dir < 0f)
                SwitchToPreviousTool();
        };
        controls.Switch0.started += ctx => SwitchToTool(0);
        controls.Switch1.started += ctx => SwitchToTool(1);
        controls.Switch2.started += ctx => SwitchToTool(2);
        controls.Switch3.started += ctx => SwitchToTool(3);

        anim = GetComponent<Animator>();

        i = 0;
        previousTool = availableTools[i];
        currentTool = availableTools[i];

        // Assign tool components
        drillTool = GetComponentInChildren<DrillTool>(true);
        blowTorchTool = GetComponentInChildren<BlowTorchTool>(true);
        harpoonGun = GetComponentInChildren<HarpoonGun>(true);
        explosiveTool = GetComponentInChildren<ExplosiveTool>(true);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region [Tool Switching]
    public void SwitchToTool(int i)
    {
        if (this.i == i)
        {
            Debug.LogFormat(
                "this.i {0} == i {1}",
                this.i,
                i);
            return;
        }

        if(i >= availableTools.Count)
        {
            Debug.LogFormat(
                "i {0} >= availableTools.Count {1}",
                i,
                availableTools.Count);
            return;
        }

        if(IsBusy)
        {
            Debug.Log("Anim is Busy");
            return;
        }

        this.i = i;
        anim.SetTrigger("Switch");
    }

    public void SwitchToolGameobject()
    {
        previousTool = currentTool;
        Debug.LogFormat(
            "Previous tool {0} set to false and Current Tool {1} set to true",
            previousTool.name,
            availableTools[i].name);
        previousTool.SetActive(false);

        currentTool = availableTools[i];
        currentTool.SetActive(true);

        onToolSwitch.Invoke(currentTool.GetComponent<ITool>());
    }

    public void SwitchToNextTool()
    {
        int _i = i;
        if (_i + 1 >= availableTools.Count)
            _i = 0;
        else
            _i++;

        SwitchToTool(_i);
    }

    public void SwitchToPreviousTool()
    {
        int _i = i;
        if (_i - 1 < 0)
            _i = availableTools.Count - 1;
        else
            _i--;

        SwitchToTool(_i);
    }
    #endregion

    #region [Tool Unlock and Lock]
    public void UnlockTool(int toolIdx)
    {
        if (availableTools.Contains(tools[toolIdx]))
            return;

        availableTools.Add(tools[toolIdx]);
    }

    public void LockTool(int toolIdx)
    {
        availableTools.RemoveAt(toolIdx);
    }

    [ContextMenu("Unlock Drill")]
    public void UnlockDrill()
    {
        UnlockTool(0);
    }

    [ContextMenu("Unlock Blow Torch")]
    public void UnlockBlowTorch()
    {
        UnlockTool(1);
    }

    [ContextMenu("Unlock Harpoon Gun")]
    public void UnlockHarpoonGun()
    {
        UnlockTool(2);
    }

    [ContextMenu("Unlock Explosive")]
    public void UnlockExplosive()
    {
        UnlockTool(3);
    }
    #endregion

    #region [Tool Relay Methods]
    public void Drill()
    {
        drillTool.Drill();
    }

    public void Repair()
    {
        blowTorchTool.Repair();
    }

    public void ShootHarpoon()
    {
        harpoonGun.ShootHarpoon();
    }

    public void ThrowExplosive()
    {
        explosiveTool.ThrowExplosive();
    }
    #endregion
}
