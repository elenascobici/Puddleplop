using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("t")) {
            switch (animator.GetInteger("Direction")) {
                case 0:
                    animator.Play("StompBack");
                    break;
                case 90:
                case 270:
                    animator.Play("StompSide");
                    break;
                case 180:
                    animator.Play("StompFront");
                    break;
                default:
                    break;
            }
            animator.SetBool("Stomping", true);
        }
        else {
            animator.SetBool("Stomping", false);
        }
    }
}
