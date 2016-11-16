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
    
    [Command]
    private void CmdUpdateAnimations(float direction) {
        RpcUpdateAnimations(direction);
    }

    [ClientRpc]
    public void RpcUpdateAnimations(float direction) {
        _anim.SetBool("MovingRight", direction > 0);
        _anim.SetBool("MovingLeft", direction < 0);
    }
    private void Update() {
        if (!isLocalPlayer) return;

        _currentHorizInput = Input.GetAxis("Horizontal");
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);

        CmdUpdateAnimations(_currentHorizInput);
        
        if (!_anim.GetBool("Death")) {
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            _anim.SetBool("Death", true);
            StartCoroutine(revive(5));
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
            Rigidbody.AddForce(Vector2.up * Jump);
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
    }

    public void Move() {
        Rigidbody.velocity = new Vector2(_currentHorizInput * Speed, Rigidbody.velocity.y);
    }
    
    
}