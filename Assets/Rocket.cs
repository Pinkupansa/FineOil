using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    Rigidbody2D rb;
    //int leftRight = 1;
    //[SerializeField] float thrust, angularVelocity;
    [SerializeField] float baseVelocity = 10;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * baseVelocity;
    }
    /*void Thrust(){
        rb.AddForce(thrust*transform.up);
    } 
    
    void Steer(){
        rb.angularVelocity = angularVelocity*leftRight;
    }

    void SwitchDir(){
        leftRight *= -1;
    } */
    void FixedUpdate(){
        /*if(Input.GetKey(KeyCode.UpArrow)){
            Thrust();
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            Steer();
        }
        */
    }
    
    void Update(){
        transform.up = rb.velocity;
        /*
        if(Input.GetKeyUp(KeyCode.DownArrow)){
            SwitchDir();
        }*/
    }
}
