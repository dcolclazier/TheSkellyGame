using System.Collections;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerMovement : NetworkBehaviour {
    public float Speed = 5.0f;
    public float Jump = 5f;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    private float GroundCheckRadius = 0.2f;
    private bool _isGrounded;
    private Animator _anim;
    private float _currentHorizInput;

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

        _currentHorizInput = Input.GetAxis("Horizontal");
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);

        
        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
            Rigidbody.AddForce(Vector2.up * Jump);
        }
    }
   
    private void FixedUpdate() {
        if(!isLocalPlayer) return;

        Move();
    }

    public void Move() {
        Rigidbody.velocity = new Vector2(_currentHorizInput * Speed, Rigidbody.velocity.y);
    }
    
    
}