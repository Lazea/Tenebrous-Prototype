using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventListenersDetach : MonoBehaviour
{
    // This is such a dumb workaround for allowing listeners
    // to work when the player is inactive...
    void Start()
    {
        transform.SetParent(null);
    }
}
