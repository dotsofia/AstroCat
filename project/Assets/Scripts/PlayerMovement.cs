using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;

    public bool dashUnlocked = false;
    public bool wallJumpUnlocked = false;

    public UIController ui;
    BoxCollider2D playerCollider;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    float lookingDirection;

    [Header("Dashing")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    bool firstDash = true;
    bool firstWallJump = true;
    bool firstDoubleJump = true;
    TrailRenderer trailRenderer;


    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 1;
    int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer; 
    bool isGrounded;
    bool isOnPlatform;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;
    
    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        Gem.OnGemCollect += UnlockSkill;
    }

    private void UnlockSkill(int skill)
    {
        switch (skill)
        {
            case 0:
                dashUnlocked = true;
                break;
            case 1:
                wallJumpUnlocked = true;
                break;
            case 2:
                maxJumps = 2;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        lookingDirection = context.ReadValue<Vector2>().y;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && dashUnlocked)
        {
            StartCoroutine(DashCoroutine());

            if (firstDash)
            {
                firstDash = false;
                ui.NextInstruction();
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;

        Vector2 dashDirection = new Vector2(horizontalMovement, lookingDirection).normalized;

        rb.gravityScale = 0;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        float lerpDuration = 0.3f;
        float elapsed = 0f;
        Vector2 startVelocity = rb.velocity;
        Vector2 targetVelocity = Vector2.zero;

        while (elapsed < lerpDuration)
        {
            rb.velocity = Vector2.Lerp(startVelocity, targetVelocity, elapsed / lerpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = baseGravity;
        rb.velocity = targetVelocity;

        isDashing = false;
        trailRenderer.emitting = false;
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisablePlayerCollider(0.25f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Droppable Platform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Droppable Platform"))
        {
            isOnPlatform = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {    
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
                smokeFX.Play();
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
                smokeFX.Play();
            }

            if (jumpsRemaining == 1 && context.performed && firstDoubleJump)
            {
                firstDoubleJump = false;
                ui.NextInstruction();
            }
        }

        if (context.performed && wallJumpTimer > 0f && wallJumpUnlocked)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;
            animator.SetTrigger("jump");
            smokeFX.Play();

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
            
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);

            if (firstWallJump)
            {
                firstWallJump = false;
                ui.NextInstruction();
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            canDash = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);     
    }
    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rb.velocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tutorial"))
        {
            Destroy(collision.gameObject);
            ui.NextInstruction();
        }
    }
}
