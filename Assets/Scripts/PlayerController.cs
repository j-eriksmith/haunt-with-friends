using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    private int finishedPlayers = 0;

    private Interactable currentInteractable;
    private GameObject damage;
    private bool hasEars;
    private GameObject mainCamera;

    // Use this for initialization
    void Start() {

    }
    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * Time.deltaTime;

        GetComponent<AudioListener>().enabled = true;
        hasEars = true;
        
        transform.Translate(x, y, 0);
        HandleInputs();
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null && isClient)
        {
            if (currentInteractable is LightSwitchAudio)
                CmdSetLight(currentInteractable.GetInstanceID());
            else if (currentInteractable is Door)
                CmdOpenDoor();
        }
    }

    public override void OnStartLocalPlayer()
    {
        //Local player is initialized to blue color
        //GetComponent<SpriteRenderer>().color = Color.clear;

        GetComponent<SpriteRenderer>().color = Color.blue;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<AudioListener>().enabled = false;
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

    private void CheckForWin(int finishedPlayers)
    {
        Debug.Log(finishedPlayers);
        if (finishedPlayers == 2)
        {
            Debug.Log("you've done it!");
        }
    }

    //Light Switch Logic (this can't live on its own because command calls must originate from a NetworkManager spawned object)
    //-------------------------
    private IEnumerator LightDelay()
    {
        Interactable finalInteractable = currentInteractable;
        yield return new WaitForSeconds(5);
        finalInteractable.GetComponent<Light>().enabled = false;
        finalInteractable.GetComponent<AudioSource>().Stop(); //Todo: play light breaking audio
    }

    [Command]
    private void CmdSetLight(int light)
    {
        RpcOnLightHook(light);
    }

    [ClientRpc]
    private void RpcOnLightHook(int light)
    {
        currentInteractable.GetComponent<CircleCollider2D>().enabled = false;
        currentInteractable.GetComponent<Light>().enabled = true;
        StartCoroutine(LightDelay());
        if (currentInteractable.GetInstanceID() == light) currentInteractable = null;
    }
    //Door commands--------------------------

    [Command]
    private void CmdOpenDoor()
    {
        RpcOpenDoor();
    }

    [ClientRpc]
    private void RpcOpenDoor()
    {
        currentInteractable.PlayInteractSound();
        currentInteractable = null;
    }

    //Exit Door commands--------------------
    public void SendOpenDoorCmd()
    {
        CmdOpenExitDoor();
    }

    [Command] 
    private void CmdOpenExitDoor()
    {
        RpcOpenExitDoor();
    }

    [ClientRpc]
    private void RpcOpenExitDoor()
    {
        currentInteractable.PlayInteractSound();
        currentInteractable = null;
    }

    public void IncrementPlayersDone()
    {
        CmdIncrementPlayersDone();
    }

    [Command]
    private void CmdIncrementPlayersDone()
    {
        RpcIncrementPlayersDone();
    }

    [ClientRpc]
    private void RpcIncrementPlayersDone()
    {
        finishedPlayers++;
        if (finishedPlayers == 2)
        {
            Debug.Log("game is done!");
        }
    }
}
