using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
    public Rigidbody Rigidbody;
    public float Speed = 5.0f;
    public float MovementInput { get; private set; }

    public void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDefaults() {

        Rigidbody.velocity = Vector3.zero;
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
        var movement = transform.forward * MovementInput * Speed * Time.deltaTime;
        Rigidbody.MovePosition(Rigidbody.position + movement);
    }

    public void Turn() {
        
    }
}