using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool lockedOnStart;

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
}
