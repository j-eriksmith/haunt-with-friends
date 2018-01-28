using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    [SyncVar]
    public int finishedPlayers = 0;

    private Interactable currentInteractable;
    private GameObject damage;
    private bool hasEars;
    private GameObject mainCamera;

    //Copy paste from TestController
    public bool moving = false;
    private const float SPEED = 2.4f;
    private Rigidbody2D rb;
    private bool[] dir;
    private AudioClip clipWood, clipCarpet, clipStone;
    private AudioSource audioSrc;
    private float stepWait, stopStepWait;
    private const float WALL_HIT_STEP_DELAY = 0.05f; // How long player has to be walking for footsteps to begin
    private float moveTime = 0f;
    private float lastStep = 0f;
    private bool canStep = true, canStopStep = false;

    // Use this for initialization
    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        clipWood = Resources.Load<AudioClip>("Audio/wooden_footsteps");
        clipStone = Resources.Load<AudioClip>("Audio/concrete_footsteps_loop");
        clipCarpet = Resources.Load<AudioClip>("Audio/carpet_footsteps_loop");
        stepWait = clipWood.length / 9;
        stopStepWait = stepWait - 0.1f;
    }

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer) return;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Must be executed before pos/velocity adjust
        lastStep += Time.deltaTime;
        if (lastStep > stepWait)
        {
            canStep = true;
        }
        if (lastStep > stopStepWait)
        {
            canStopStep = true;
        }
        if (moving)
        {
            moveTime += Time.deltaTime;
        }
        else
        {
            moveTime = 0f;
        }
        if (rb.velocity.magnitude > 0.01f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        GetComponent<AudioListener>().enabled = true;
        hasEars = true;

        rb.velocity = new Vector2(x, y);
        HandleInputs();

        // Movement code
        // print(rb.velocity.magnitude);

        //print(moving);
//        dir = new bool[] { Input.GetKey(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow),
//            Input.GetKey(KeyCode.LeftArrow), Input.GetKey(KeyCode.RightArrow) };
//        rb.velocity = new Vector2((System.Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow))) * SPEED,
//            (System.Convert.ToInt32(Input.GetKey(KeyCode.UpArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.DownArrow))) * SPEED);


        // Footsteps code
        if (moving && moveTime >= WALL_HIT_STEP_DELAY)
        {
            if (!audioSrc.isPlaying && canStep)
            {
                print("start");
                audioSrc.PlayOneShot(clipWood);
                StartCoroutine("wood_step_sound_guard");
            }
        }
        else
        {
            if (canStopStep)
            {
                //print("stop");
                audioSrc.Stop();
                StopCoroutine("wood_step_sound_guard");
            }
        }
    }

    //Footstep code
    IEnumerator wood_step_sound_guard() // Terminates a tiny amount early so that it can be called again without conflict
    {
        float time = 0.05f;
        int stepcount = 0;
        while (time < clipWood.length - 0.05f)
        {
            time += Time.deltaTime;
            if (stepcount <= (int)(time / stepWait))
            {
                stepcount++;
                lastStep = 0f;
                canStep = false;
                canStopStep = false;
                //print("step");
            }
            yield return null;
        }
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
        finishedPlayers++;
        Debug.Log(finishedPlayers);
        RpcIncrementPlayersDone();
    }

    [ClientRpc]
    private void RpcIncrementPlayersDone()
    {
        if (finishedPlayers == 2)
        {
            Debug.Log("game is done!");
        }
    }
}
