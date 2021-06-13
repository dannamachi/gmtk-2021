using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneController : MonoBehaviour
{
    // disable/enable
    bool myEnabled = false;

    public bool isEnabled() { return myEnabled; }
    public void setEnabled(bool en) { myEnabled = en; }
    public void setDisplay(bool canSee)
    {
        SpriteRenderer ren = GetComponent<SpriteRenderer>();
        ren.enabled = canSee;
    }

    // set location
    public void setLocation(Vector2 vec)
    {
        transform.position = vec;
    }

    // On collision with player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnabled())
        {
            // if player enters safe zone
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.setSafe(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (isEnabled())
        {
            // if player leaves safe zone
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.setSafe(false);
            }
        }
    }
}
