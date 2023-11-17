using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    Animator animator;
    private int stompCount = 0;
    private int stompsTotal = 5;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("t") && stompCount < 5 && !isPlaying) {
            StartCoroutine("PlayStompAnimation");            
        }
    }

    IEnumerator PlayStompAnimation() {
        animator.SetBool("Stomping", true);

        while (stompCount < stompsTotal) {
            animator.SetTrigger("StompTrigger");
            stompCount++;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        
        animator.SetBool("Stomping", false);
        stompCount = 0;
    }
}
