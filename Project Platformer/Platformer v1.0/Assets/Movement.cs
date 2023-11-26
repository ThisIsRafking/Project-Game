using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    Rigidbody2D rb;
    private float move;

    public LayerMask JG;
    public LayerMask WG;

    public Transform wallcheck;

    private BoxCollider2D coll;
    
    private bool IsFacingRight = true;
private bool isWallSliding;
    public float wallSlidingSpeed = 2f;

     private bool isWallJumping;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
     public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && Isgrounded())
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpforce));
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
        WallSlide();
        WallJump();
    }

    void FixedUpdate() {
         rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    private bool Isgrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, JG);
    }

    private bool IsWalled()
    {
        return Physics2D.Raycast(wallcheck.position, Vector2.left, 0.2f, WG) ||
            Physics2D.Raycast(wallcheck.position, Vector2.right, 0.2f, WG);
    }

    private void WallSlide()
    {
        if (IsWalled() && !Isgrounded() && move != 0f)
        {
            isWallSliding = true;
            rb.AddForce(new Vector2(0, -wallSlidingSpeed));
        }
        else
        {
            isWallSliding = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        if (IsFacingRight && move < 0f || !IsFacingRight && move > 0)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

      private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
