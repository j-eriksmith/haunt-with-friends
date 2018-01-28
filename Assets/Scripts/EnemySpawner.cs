using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemy;
    public int enemyCount;

    public override void OnStartServer()
    {
        for (int i = 0; i < enemyCount; ++i)
        {
            Vector2 spawn = new Vector2(transform.position.x, transform.position.y);
            GameObject e = (GameObject)Instantiate(enemy, spawn, Quaternion.Euler(0f, 0f, 0f));
            NetworkServer.Spawn(e);
        }
    }
}
