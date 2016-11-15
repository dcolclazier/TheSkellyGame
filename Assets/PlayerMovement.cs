using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
    public Rigidbody2D Rigidbody;
    public float Speed = 5.0f;
    public float MovementInput { get; private set; }

    public void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDefaults() {

        Rigidbody.velocity = Vector2.zero;
    }

    private void Update() {
        if (!isLocalPlayer) return;
        MovementInput = Input.GetAxis("Horizontal");
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