using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private const int WalkSpeed = 5;
    private string currentState;

    // Animation name constants
    private string IDLE_FRONT = "IdleFront";
    private string IDLE_SIDE = "IdleSide";
    private string IDLE_BACK = "IdleBack";
    private string BOUNCE_FRONT = "BounceFront";
    private string BOUNCE_SIDE = "BounceSide";
    private string BOUNCE_BACK = "BounceBack";

    // Keyboard inputs
    private float xAxis;
    private float yAxis;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        // Normalize the vector to ensure that the player doesn't
        // move faster diagonally.
        Vector2 input = new Vector2(xAxis, yAxis).normalized;
        Vector2 movement = input * WalkSpeed;
        body.velocity = movement;

        // Control player movement and animation

        if (xAxis < 0) {
            transform.localScale = new Vector2(-1, 1);
            ChangeAnimationState(BOUNCE_SIDE);
        }
        else if (xAxis > 0) {
            transform.localScale = new Vector2(1, 1);
            ChangeAnimationState(BOUNCE_SIDE);
        }
        else if (yAxis < 0) { 
            ChangeAnimationState(BOUNCE_FRONT);
        }
        else if (yAxis > 0) {
            ChangeAnimationState(BOUNCE_BACK);
        }
        else {
            GoToIdle();
        }
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void GoToIdle() {
        if (currentState == BOUNCE_FRONT) {
            ChangeAnimationState(IDLE_FRONT);
        }
        else if (currentState == BOUNCE_SIDE) {
            ChangeAnimationState(IDLE_SIDE);
        }
        else if (currentState == BOUNCE_BACK) {
            ChangeAnimationState(IDLE_BACK);
        }
    }
}
