using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private
        Animator anim;
        Vector3 trans;
        Rigidbody2D rb;
    public float Speed = 10f;
    public float Jump = 1500f;
    public float isGroundedRayLength = 0.1f;
    public LayerMask groundLayer;

    void Start ()
    {
        anim = this.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
	
	void Update ()
    {
        if (anim.GetBool("Death") == false)
        {
 
           anim.SetFloat("Speed", Input.GetAxisRaw("Horizontal"));
           var x = Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime;
           transform.Translate(x, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.E))
        { 
            anim.SetBool("Death", true);
            StartCoroutine(revive(5));
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {                      
            print("Jump");
            rb.AddForce(Vector2.up * Jump);
        }
    }

    public bool isGrounded
    {
        get
        {
            Vector3 position = transform.position;
            position.y = GetComponent<Collider2D>().bounds.min.y + 0.1f;
            float length = isGroundedRayLength + 0.1f;
            Debug.DrawRay(position, Vector3.down * length);
            bool grounded = Physics2D.Raycast(position, Vector3.down, length, groundLayer.value);
            return grounded;
        }
    }
        IEnumerator revive(int time)
        { 
            yield return new WaitForSeconds(time);
           anim.SetBool("Death", false);
        }

}
