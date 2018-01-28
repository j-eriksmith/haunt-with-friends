using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour{

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
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        clipWood = Resources.Load<AudioClip>("Audio/wooden_footsteps");
        clipStone = Resources.Load<AudioClip>("Audio/concrete_footsteps_loop");
        clipCarpet = Resources.Load<AudioClip>("Audio/carpet_footsteps_loop");
        stepWait = clipWood.length / 9;
        stopStepWait = stepWait - 0.1f;
	}

    IEnumerator wood_step_sound_guard() // Terminates a tiny amount early so that it can be called again without conflict
    {
        float time = 0.05f;
        int stepcount = 0;
        while(time < clipWood.length-0.05f)
        {
            time += Time.deltaTime;
            if(stepcount <= (int)(time/stepWait))
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

	// Update is called once per frame
	void Update () {

        // Movement code
        // print(rb.velocity.magnitude);
        lastStep += Time.deltaTime;
        if(lastStep > stepWait)
        {
            canStep = true;
        }
        if(lastStep > stopStepWait)
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
        //print(moving);
        dir = new bool[] { Input.GetKey(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow), 
            Input.GetKey(KeyCode.LeftArrow), Input.GetKey(KeyCode.RightArrow) };
        rb.velocity = new Vector2((System.Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow))) * SPEED, 
            (System.Convert.ToInt32(Input.GetKey(KeyCode.UpArrow)) - System.Convert.ToInt32(Input.GetKey(KeyCode.DownArrow))) * SPEED);
        

        // Footsteps code
        if(moving && moveTime >= WALL_HIT_STEP_DELAY)
        {
            if (!audioSrc.isPlaying && canStep)
            {
                //print("start");
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
}
