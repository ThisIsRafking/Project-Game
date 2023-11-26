using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terbng : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;
        float vertical = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(horizontal, vertical);
    }
}
