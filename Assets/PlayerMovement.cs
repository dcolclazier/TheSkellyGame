using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
    public Rigidbody2D Rigidbody;
    public float Speed = 5.0f;
    public float Jump = 1500f;
    public LayerMask GroundLayer;

    private Animator _anim;
    private float isGroundedRayLength = 0.1f;

    public float MovementInput { get; private set; }

    public void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

    }

    public void SetDefaults() {

        Rigidbody.velocity = Vector2.zero;
    }

    private void Update() {
        if (!isLocalPlayer) return;

        if (!_anim.GetBool("Death")) {
            _anim.SetFloat("Speed", Input.GetAxisRaw("Horizontal"));
            MovementInput = Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            _anim.SetBool("Death", true);
            StartCoroutine(revive(5));
        }
        if (Input.GetKeyDown(KeyCode.Space)/* && isGrounded*/) {
            //Rigidbody.AddForce(Vector2.up * Jump);
            //Rigidbody.velocity += new Vector2(0, Jump);
            Rigidbody.AddForce(Vector2.up * Jump);
        }
        //MovementInput = Input.GetAxis("Horizontal");
    }
    public bool isGrounded
    {
        get
        {
            Vector3 position = transform.position;
            position.y = GetComponent<Collider2D>().bounds.min.y + 0.1f;
            float length = isGroundedRayLength + 0.1f;
            Debug.DrawRay(position, Vector3.down * length);
            bool grounded = Physics2D.Raycast(position, Vector3.down, length, GroundLayer.value);
            return grounded;
        }
    }
    IEnumerator revive(int time)
    {
        yield return new WaitForSeconds(time);
        _anim.SetBool("Death", false);
    }
    private void FixedUpdate() {
        if(!isLocalPlayer) return;
        Move();
        Turn();
    }
    public void Move() {
        Vector2 movement = transform.right * MovementInput * Speed * Time.fixedDeltaTime;
        Rigidbody.MovePosition((Rigidbody.position + movement));
    }

    public void Turn() {
        
    }
}