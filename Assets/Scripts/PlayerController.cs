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
