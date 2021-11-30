using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossTrigger : MonoBehaviour
{
    public event EventHandler OnPlayerEnterTrigger;

    // script to capture physics triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player is inside the trigger area
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }
}
