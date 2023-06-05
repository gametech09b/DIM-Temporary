using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIM
{
    public class TamanMovement : MonoBehaviour
    {
        private Rigidbody2D rb;
        public Animator animator;

        public float MovementSpeed = 1;
        float horizontal;
        float vertical;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            transform.position += new Vector3 (horizontal, vertical, 0) * Time.deltaTime * MovementSpeed;

            if (horizontal == 0f && vertical == 0f)
            {
                rb.velocity = new Vector2(0, 0);
                animator.SetBool("isMoving", false);
                animator.SetBool("isIdle", true);
            }
            
            else
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isIdle", false);
            }

            PlayerAnimController();

        }

        private void PlayerAnimController()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                animator.SetBool("aimRight", true);
                animator.SetBool("aimLeft", false);
                animator.SetBool("flipLeft", false);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                animator.SetBool("aimLeft", true);
                animator.SetBool("aimRight", false);
                animator.SetBool("flipLeft", true);
            }
        }


    }
}
