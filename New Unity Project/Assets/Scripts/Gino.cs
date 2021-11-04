using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gino : MonoBehaviour {
    public static Gino instance;
    public SpawnManager spawnManager;
    public DecorManager decorManager;
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
