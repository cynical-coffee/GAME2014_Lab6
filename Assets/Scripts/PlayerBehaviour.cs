using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")] 
    public float fHorizontalForce;
    public float fVerticalForce;
    public bool bIsGrounded;
    public Transform mGroundOrigin;
    public float fGroundRadius;
    public LayerMask MGroundLayerMask;
    //*******************************
    private Rigidbody2D mRigidbody2D;

    private void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void Move()
    {
        if (bIsGrounded)
        {
            float fDeltaTime = Time.deltaTime;

            float fX = Input.GetAxisRaw("Horizontal");
            float fY = Input.GetAxisRaw("Vertical");
            float fJump = Input.GetAxisRaw("Jump");

            Vector2 vWorldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                vWorldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float fHorizontalMoveForce = fX * fHorizontalForce; // * fDeltaTime;
            float fJumpMoveForce = fJump * fVerticalForce; // * fDeltaTime;
            float fMass = mRigidbody2D.mass * mRigidbody2D.gravityScale;
            mRigidbody2D.AddForce(new Vector2(fHorizontalMoveForce, fJumpMoveForce) * fMass);
            mRigidbody2D.velocity *= 0.99f;
        }
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(mGroundOrigin.position, fGroundRadius, Vector2.down, fGroundRadius, MGroundLayerMask);
        bIsGrounded = (hit) ? true : false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mGroundOrigin.position, fGroundRadius);
    }
}
