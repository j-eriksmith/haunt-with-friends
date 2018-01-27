using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour{

    private const float SPEED = 2.4f;
    private Rigidbody2D rb;
    private bool[] dir = new bool[4];

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //dir = new bool[] { Input.GetKey(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow), 
        //    Input.GetKey(KeyCode.LeftArrow), Input.GetKey(KeyCode.RightArrow) };
        dir[0] = Input.GetKey(KeyCode.UpArrow);
        dir[1] = Input.GetKey(KeyCode.DownArrow);
        dir[2] = Input.GetKey(KeyCode.LeftArrow);
        dir[3] = Input.GetKey(KeyCode.RightArrow);
        rb.velocity = new Vector2((System.Convert.ToInt32(dir[3]) - System.Convert.ToInt32(dir[2])) * SPEED, 
            (System.Convert.ToInt32(dir[0]) - System.Convert.ToInt32(dir[1])) * SPEED);
	}
}
