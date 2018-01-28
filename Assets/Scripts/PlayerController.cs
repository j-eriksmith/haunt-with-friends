using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private Interactable currentInteractable;
    private GameObject damage;

    // Use this for initialization
    void Start () {
        if (isLocalPlayer)
        {
            GetComponent<AudioListener>().enabled = true;
        }
        //damage = GameObject.Find("enemy");
        //damage.SendMessage("addCharacter", gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.Translate(x, y, 0);
        HandleInputs();
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (isClient) CmdSetLight(enabled);
        }
    }

    public override void OnStartLocalPlayer()
    {
        //Local player is initialized to blue color
        //GetComponent<SpriteRenderer>().color = Color.clear;

        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void setInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    public void removeInteractable(Interactable interactable)
    {
        if (currentInteractable == interactable) //In case they're somehow inside of two interactable zones, don't null out immediately
        {
            interactable = null;
        }
    }


    //Light Switch Logic (this can't live on its own because command calls must originate from a NetworkManager spawned object)
    //-------------------------
    private IEnumerator LightDelay()
    {
        yield return new WaitForSeconds(5);
        currentInteractable.GetComponent<Light>().enabled = false;
        currentInteractable.GetComponent<AudioSource>().Stop(); //Todo: play light breaking audio
        currentInteractable = null;
    }

    [Command]
    private void CmdSetLight(bool e)
    {
        RpcOnLightHook();
    }

    [ClientRpc]
    private void RpcOnLightHook()
    {
        currentInteractable.GetComponent<CircleCollider2D>().enabled = false;
        currentInteractable.GetComponent<Light>().enabled = true;
        StartCoroutine(LightDelay());
    }
    //--------------------------
}
