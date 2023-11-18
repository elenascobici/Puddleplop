using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private const int WalkSpeed = 5;
    private string currentState;
    private bool isStomping = false;
    private System.DateTime stompEndTime;
    private enum Direction {
        Front,
        Back,
        Side,
    };
    private Direction currentDirection = Direction.Front;

    // Animation name constants
    private string IDLE_FRONT = "IdleFront";
    private string IDLE_SIDE = "IdleSide";
    private string IDLE_BACK = "IdleBack";
    private string BOUNCE_FRONT = "BounceFront";
    private string BOUNCE_SIDE = "BounceSide";
    private string BOUNCE_BACK = "BounceBack";
    private string STOMP_FRONT = "StompFront";
    private string STOMP_SIDE = "StompSide";
    private string STOMP_BACK = "StompBack";

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
        if (Input.GetKeyDown("space") && !isStomping) {
            stompEndTime = System.DateTime.Now.AddSeconds(1.5);
            isStomping = true;
            // Stop moving
            body.velocity = new Vector2(0, 0);
        }

        // Control player movement and animation
        if (!isStomping) {
            xAxis = Input.GetAxisRaw("Horizontal");
            yAxis = Input.GetAxisRaw("Vertical");

            // Normalize the vector to ensure that the player doesn't
            // move faster diagonally.
            Vector2 input = new Vector2(xAxis, yAxis).normalized;
            Vector2 movement = input * WalkSpeed;
            body.velocity = movement;

            if (xAxis < 0) {
                transform.localScale = new Vector2(-1, 1);
                ChangeAnimationState(BOUNCE_SIDE);
                currentDirection = Direction.Side;
            }
            else if (xAxis > 0) {
                transform.localScale = new Vector2(1, 1);
                ChangeAnimationState(BOUNCE_SIDE);
                currentDirection = Direction.Side;
            }
            else if (yAxis < 0) { 
                ChangeAnimationState(BOUNCE_FRONT);
                currentDirection = Direction.Front;
            }
            else if (yAxis > 0) {
                ChangeAnimationState(BOUNCE_BACK);
                currentDirection = Direction.Back;
            }
            else {
                GoToIdle();
            }
        }
        else {
            GoToStomp();
        }
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void GoToIdle() {
        if (currentDirection == Direction.Front) {
            ChangeAnimationState(IDLE_FRONT);
        }
        else if (currentDirection == Direction.Side) {
            ChangeAnimationState(IDLE_SIDE);
        }
        else if (currentDirection == Direction.Back) {
            ChangeAnimationState(IDLE_BACK);
        }
    }

    void GoToStomp() {
        if (System.DateTime.Now < stompEndTime) {
            if (currentDirection == Direction.Front) {
                ChangeAnimationState(STOMP_FRONT);
            }
            else if (currentDirection == Direction.Side) {
                ChangeAnimationState(STOMP_SIDE);
            }
            else if (currentDirection == Direction.Back) {
                ChangeAnimationState(STOMP_BACK);
            }
        }
        else {
            isStomping = false;
        }
    }
}
