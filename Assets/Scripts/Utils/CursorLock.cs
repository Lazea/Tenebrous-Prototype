using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool lockedOnStart;

#if UNITY_EDITOR
    Controls.AppActions controls;

    private void Awake()
    {
        controls = new Controls().App;
        controls.ToggleCursor.performed += ctx => ToggleCursor();
        controls.Enable();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        if(lockedOnStart)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    [ContextMenu("Lock Cursor")]
    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [ContextMenu("Unlock Cursor")]
    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    [ContextMenu("Toggle Cursor")]
    public void ToggleCursor()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }
}
