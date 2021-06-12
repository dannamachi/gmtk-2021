using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variable
    Animator anim;
    Rigidbody2D body;
    float hori;
    float vert;
    Vector2 lookDir = new Vector2(1,0);

    // Start is called before the first frame update (not called when instantiate)
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
