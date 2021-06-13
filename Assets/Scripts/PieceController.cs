using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool myEnabled = false;
    // Connections
    string myName;

    // Start is called when instant
    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Piece id
    public string getName()
    {
        return myName;
    }
    public bool isCalled(string sth)
    {
        return myName.Equals(sth);
    }

    // Get sprite for item display
    public Sprite getSprite()
    {
        return spriteRenderer.sprite;
    }

    // Set location
    public void setLocation(Vector2 vec)
    {
        transform.position = vec;
    }

    // Enable/disable
    public void enable()
    {
        myEnabled = true;
        show();
    }
    public void disable()
    {
        myEnabled = false;
        hide();
    }
    public bool isEnabled()
    {
        return myEnabled;
    }
    public void hide()
    {
        spriteRenderer.enabled = false;
    }
    public void show()
    {
        spriteRenderer.enabled = true;
    }

    // Change sprite
    public void changeSprite(Sprite sp, string text)
    {
        spriteRenderer.sprite = sp;
        myName = text;
    }

    // On collision with other stuff
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnabled())
        {
            // if player hits a piece
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.detectPiece(gameObject);
                return;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (isEnabled())
        {
            // if player leaves a piece
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.escapePiece(gameObject);
                return;
            }
        }
    }

    // check & update connection in update
    void Update()
    {
        
    }
    public Vector2 getMyPos()
    {
        Vector2 myPos = transform.position;
        return myPos;
    }
}
