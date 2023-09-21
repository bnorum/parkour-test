using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove {
    public float speed = 5.0f;
    void Start() {


    }
    void Update(){
        float Horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;


    }


}