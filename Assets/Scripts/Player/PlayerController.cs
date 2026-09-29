using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) movement.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) movement.y -= 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movement.x -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movement.x += 1;

        movement = movement.normalized;

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    private void UpdateAnimation()
    {
        float magnitude = movement.magnitude;
        animator.speed = magnitude > 0.01f ? 1f : 0f; // pausa la animación si no se mueve

        if (magnitude > 0.01f)
        {
            // Prioriza el eje con mayor movimiento (evita animación rara en diagonal)
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                animator.SetInteger("Direction", movement.x > 0 ? 3 : 2); // Right : Left
            }
            else
            {
                animator.SetInteger("Direction", movement.y > 0 ? 1 : 0); // Up : Down
            }
        }
    }
}