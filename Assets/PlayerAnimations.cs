using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAnimations : NetworkBehaviour
{

    private Animator _anim;

    public void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    [Command]
    private void CmdUpdateAnimations(float direction)
    {
        RpcUpdateAnimations(direction);
    }
    [ClientRpc]
    public void RpcUpdateAnimations(float direction)
    {
        _anim.SetBool("MovingRight", direction > 0);
        _anim.SetBool("MovingLeft", direction < 0);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        CmdUpdateAnimations(Input.GetAxis("Horizontal"));
    }

    //IEnumerator revive(int time)
    //{
    //    yield return new WaitForSeconds(time);
    //    _anim.SetBool("Death", false);
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        _anim.SetBool("Death", true);
    //        StartCoroutine(revive(5));
    //    }
    //}

    public void OnJustDied() {
        _anim.SetBool("Death", true);
    }

    public void OnRespawned() {
        _anim.SetBool("Death", false);
    }
}
