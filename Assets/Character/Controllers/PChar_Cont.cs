using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PChar_Cont : MonoBehaviour {

    private Rigidbody2D pBody;
    private bool grounded;
    private bool facingRight = true;
    private bool isRolling;
    private SpriteRenderer pSprite;
    private Animator anim;

    public float moveSpeed;
    public float maxSpeed;
    public float jumpPower;
    public float rollLength;
    public float rollSpeed;
    public LayerMask groundLayer;

    // Use this for initialization
    void Start () {

        pBody = gameObject.GetComponent<Rigidbody2D>();
        pSprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

        grounded = Physics2D.OverlapArea(new Vector2 (transform.position.x-0.5f, transform.position.y - 1.7f),
            new Vector2 (transform.position.x-0.5f, transform.position.y -0.6f), groundLayer);

        anim.SetBool("onGround", grounded);

        float move = Input.GetAxis("Horizontal");
        pBody.velocity = new Vector2(move * moveSpeed, pBody.velocity.y);

        float tempY = pBody.velocity.y;

        if(pBody.velocity.magnitude > maxSpeed)
        {
            pBody.velocity = pBody.velocity.normalized * maxSpeed;
        }
        pBody.velocity = new Vector2(pBody.velocity.x, tempY);

        anim.SetFloat("Speed", Mathf.Abs(pBody.velocity.x));

        var localVelocity = transform.InverseTransformDirection(pBody.velocity);

        if (localVelocity.x < 0)
        {
            facingRight = false;
            pSprite.flipX = true;
        }
        else if (localVelocity.x > 0)
        {
            facingRight = true;
            pSprite.flipX = false;
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            pBody.velocity = new Vector2(pBody.velocity.x, jumpPower);
        }

        if(Input.GetButtonDown("Fire2") && !isRolling)
        {
            isRolling = true;
            anim.SetBool("isRolling", true);
            DodgeRoll();
        }

        if (isRolling)
        {
            if (facingRight)
            {
                pBody.velocity = new Vector2(rollSpeed, -rollSpeed / 2);
            }
            else
            {
                pBody.velocity = new Vector2(-rollSpeed, -rollSpeed/2);
            }
        }


	}

    void DodgeRoll(){
        StartCoroutine(StartRoll(rollLength));
    }

    IEnumerator StartRoll(float time)
    {
        float temp = maxSpeed;
        maxSpeed = rollSpeed;
        yield return new WaitForSeconds(time);
        isRolling = false;
        anim.SetBool("isRolling", false);
        maxSpeed = temp;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color (0, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 1.7f), new Vector2(1, 0.1f));
    }

}
