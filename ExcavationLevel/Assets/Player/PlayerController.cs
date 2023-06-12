using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float horizontal;
    public bool onGround;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            onGround = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
         horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }
        rb.velocity = movement;
    }
    private void Update()
    {
        animator.SetFloat("horizontal", horizontal);
    }
}
