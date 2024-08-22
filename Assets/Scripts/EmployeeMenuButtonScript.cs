using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenuButtonScript : MonoBehaviour
{
    public Button openEmployeeMenuButton;
    public GameObject employeeMenu;
    public SpriteRenderer darkenedSquare;
    private float squareAlpha;
    public Button closeEmployeeMenuButton;

    // Get all of the book's animation states
    public AnimationClip opening;
    public AnimationClip closing;
    public AnimationClip open;
    public AnimationClip closed;
    private int UP_SPEED = 16;
    private int DOWN_SPEED = 7;
    public Animator animator;
    public PlayerMovement playerMovement;
    public int state;
    public EmployeeMenuPagination employeeMenuPagination;
    void Start()
    {
        openEmployeeMenuButton.onClick.AddListener(() => {state = 1;});
        closeEmployeeMenuButton.onClick.AddListener(() => {state = 5;});
        closeEmployeeMenuButton.gameObject.SetActive(false);
        opening.wrapMode = WrapMode.Once; // Only play opening animation once
        animator.Play(closed.name);
        state = 0;
        squareAlpha = 0;
        darkenedSquare.color = new Color(0, 0, 0, 0);
    }

    void Update() {
        float menuYCoord = employeeMenu.transform.position.y;
        // Darken the background behind the book to make it more
        // easily visible against the similarly coloured floor.
        if (state >= 1 && state < 5 && squareAlpha <= 0.5) {
            squareAlpha += 0.01f;
            darkenedSquare.color = new Color(0, 0, 0, squareAlpha);
        }
        // Reverse background darkening when the book is closed.
        if (state >= 5 && squareAlpha > 0) {
            squareAlpha -= 0.01f;
            darkenedSquare.color = new Color(0, 0, 0, squareAlpha);
        }

        // Play book opening animations step by step.
        if (state == 1 && menuYCoord < 1) {
            employeeMenu.transform.Translate(new Vector3(0, 1, 0) * UP_SPEED * Time.deltaTime);
            playerMovement.movementEnabled = false;
            playerMovement.Stop();
        }
        else if ((state == 1 || state == 2) && menuYCoord > 0) {
            state = 2;
            employeeMenu.transform.Translate(new Vector3(0, -1, 0) * DOWN_SPEED * Time.deltaTime);
        }
        else if (state == 2) {
            animator.Play(opening.name);
            state = 3;
        } // Note: stop playing animation at 95% to avoid flickering.
        else if (state == 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95) {
            animator.Play(open.name);
            state = 4;
            closeEmployeeMenuButton.gameObject.SetActive(true);
            employeeMenuPagination.Init();
        }

        // Play book closing animations step by step.
        if (state == 5) {
            employeeMenuPagination.Close();
            animator.Play(closing.name);
            state = 6;
            closeEmployeeMenuButton.gameObject.SetActive(false);
        }
        else if (state == 6 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95) {
            animator.Play(closed.name);
            state = 7;
        }
        else if (state == 7 && menuYCoord > -7.69) {
            employeeMenu.transform.Translate(new Vector3(0, -1, 0) * UP_SPEED * Time.deltaTime);
        }
        else if (state == 7) {
            state = 0;
            playerMovement.movementEnabled = true;
        }
    }
}
