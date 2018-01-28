using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightSwitchAudio : Interactable {
    private bool enabled;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //this.GetComponent<NetworkIdentity>().AssignClientAuthority(collision.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.setInteractable(this);
            used = true;
        }
    }

    public override void interact()
    {
        enabled = true;
        if (isClient) CmdSetLight(enabled);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(LightDelay());
    }

    private IEnumerator LightDelay()
    {
        yield return new WaitForSeconds(5);
        enabled = false;
        CmdSetLight(enabled);
        GetComponent<AudioSource>().Stop(); //Todo: play light breaking audio
    }

    [Command]
    private void CmdSetLight(bool e)
    {
        RpcOnLightHook(e);
    }

    [ClientRpc]
    private void RpcOnLightHook(bool light)
    {
        GetComponent<Light>().enabled = light;
    }
}
