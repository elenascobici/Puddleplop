using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variable that allows other objects to disable movement,
    // for example when a menu is opened.
    public static bool movementEnabled;
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
    private Camera mainCamera;

    // Camera-related and world bounds.
    private float cameraX;
    private const float MIN_X = -7.5F;
    private const float MAX_X = 18.5F;
    private const float MIN_Y = -4.5F;
    private const float MAX_Y = 3.8F;

    // Start is called before the first frame update
    void Start()
    {
        movementEnabled = true;
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        cameraX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && !isStomping && movementEnabled) {
            stompEndTime = System.DateTime.Now.AddSeconds(0.5);
            isStomping = true;
        }

        // Player movement.
        if (movementEnabled) {
            xAxis = Input.GetAxisRaw("Horizontal");
            yAxis = Input.GetAxisRaw("Vertical");

            // Normalize the vector to ensure that the player doesn't
            // move faster diagonally.
            Vector2 input = new Vector2(xAxis, yAxis).normalized;
            Vector2 newPosition = transform.position + new Vector3(input.x * WalkSpeed * Time.deltaTime, input.y * WalkSpeed * Time.deltaTime, 0);
            // Clamp player's coordinates so they can't go over the
            // bounds of the world.
            newPosition.x = Mathf.Clamp(newPosition.x, MIN_X, MAX_X);
            newPosition.y = Mathf.Clamp(newPosition.y, MIN_Y, MAX_Y);
            transform.position = newPosition;

            // Move camera to follow player if needed, also clamping
            // to not go over bounds.
            cameraX = transform.position.x;
            Vector3 newCameraPosition = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, 0, MAX_X - 7.5F);
            mainCamera.transform.position = newCameraPosition;

            // Control player animation.
            if (!isStomping) {
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
