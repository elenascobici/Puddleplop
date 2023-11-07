using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    int SPEED = 4;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKey("a")) {
            Vector3 position = this.transform.position;
            position.x -= SPEED * Time.deltaTime;
            this.transform.position = position;
            spriteRenderer.flipX = true;
            animator.SetFloat("Speed", SPEED);
            animator.SetInteger("Direction", 270);
        }

        else if (Input.GetKey("d")) {
            Vector3 position = this.transform.position;
            position.x += SPEED * Time.deltaTime;
            this.transform.position = position;
            spriteRenderer.flipX = false;
            animator.SetFloat("Speed", SPEED);
            animator.SetInteger("Direction", 90);
        }

        else if (Input.GetKey("w")) {
            Vector3 position = this.transform.position;
            position.y += SPEED * Time.deltaTime;
            this.transform.position = position;
            animator.SetFloat("Speed", SPEED);
            animator.SetInteger("Direction", 0);
        }

        else if (Input.GetKey("s")) {
            Vector3 position = this.transform.position;
            position.y -= SPEED * Time.deltaTime;
            this.transform.position = position;
            animator.SetFloat("Speed", SPEED);
            animator.SetInteger("Direction", 180);
        }

        else {
            animator.SetFloat("Speed", 0);
        }
    }
}
