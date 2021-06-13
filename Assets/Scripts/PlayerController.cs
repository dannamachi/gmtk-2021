using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // movement stuff
    Animator anim;
    Rigidbody2D body;
    float hori;
    float vert;
    Vector2 lookDir = new Vector2(1,0);

    // item stuff
    public GameObject itemDisplayObj;
    GameObject pieceDetected;
    GameObject pieceInHand;

    // game ref
    public GameObject gameObj;

    // Start is called before the first frame update (not called when instantiate)
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // movement & movement animation
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(hori, vert);
                
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDir.Set(move.x, move.y);
            lookDir.Normalize();
        }
                
        anim.SetFloat("Look X", lookDir.x);
        anim.SetFloat("Look Y", lookDir.y);
        anim.SetFloat("Speed", move.magnitude);

        // pick up piece
        if(Input.GetKeyDown(KeyCode.Z))
        {
            pickUp();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            putDown();
        }
    }

    // pick up piece
    void pickUp()
    {
        if (pieceDetected != null && !hasPickUp())
        {
            // update holding sprite
            pieceInHand = pieceDetected;
            PieceController myPiece = pieceInHand.GetComponent<PieceController>();
            itemDisplayObj.GetComponent<Image>().sprite = myPiece.getSprite();
            // hide item
            myPiece.hide();
            // unsnap from map slot if any
            gameObj.GetComponent<MyGameController>().checkSlotSnapping(myPiece);
        }
    }
    void putDown()
    {
        if (hasPickUp())
        {
            // update holding sprite
            PieceController myPiece = pieceInHand.GetComponent<PieceController>();
            itemDisplayObj.GetComponent<Image>().sprite = null;
            myPiece.setLocation(transform.position);
            // show placed item
            myPiece.show();
            // remove item ref
            pieceInHand = null;
            // snap into map slot if any
            gameObj.GetComponent<MyGameController>().checkSlotSnapping(myPiece);
        }
    }
    bool hasPickUp()
    {
        return pieceInHand != null;
    }
    public void detectPiece(GameObject obj)
    {
        pieceDetected = obj;
    }
    public void escapePiece(GameObject obj)
    {
        if (pieceDetected == obj)
        {
            pieceDetected = null;
        }
    }

    // Update for physics
    void FixedUpdate() 
    {
        Vector2 pos = body.position;
        pos.x += 3.0f * hori * Time.deltaTime;
        pos.y += 3.0f * vert * Time.deltaTime;

        body.MovePosition(pos);
    }
}
