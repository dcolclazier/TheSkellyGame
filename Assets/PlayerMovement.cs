using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
    public float Speed = 5.0f;
    public float Jump = 5f;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    private float GroundCheckRadius = 0.2f;
    private bool _isGrounded = false;
    private Animator _anim;
    
    //private float isGroundedRayLength = 0.1f;

    public float MovementInput { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    public void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void SetDefaults() {

        Rigidbody.velocity = Vector2.zero;
    }

    private void Update() {
        if (!isLocalPlayer) return;
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);
        //_anim.SetBool("Grounded",_isGrounded);
        

        if (!_anim.GetBool("Death")) {
            _anim.SetFloat("Speed", Input.GetAxisRaw("Horizontal"));
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            _anim.SetBool("Death", true);
            StartCoroutine(revive(5));
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
            Rigidbody.AddForce(Vector2.up * Jump);
        }
    }
    //public bool isGrounded
    //{
    //    get
    //    {
    //        Vector3 position = transform.position;
    //        position.y = GetComponent<Collider2D>().bounds.min.y + 0.1f;
    //        float length = isGroundedRayLength + 0.1f;
    //        Debug.DrawRay(position, Vector3.down * length);
    //        bool grounded = Physics2D.Raycast(position, Vector3.down, length, GroundLayer.value);
    //        return grounded;
    //    }
    //}
    IEnumerator revive(int time)
    {
        yield return new WaitForSeconds(time);
        _anim.SetBool("Death", false);
    }
    private void FixedUpdate() {
        if(!isLocalPlayer) return;

        Move();
    }
    public void Move() {

        var move = Input.GetAxis("Horizontal");
        Rigidbody.velocity = new Vector2(move * Speed, Rigidbody.velocity.y);
        
    }
    
    
}