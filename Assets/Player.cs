using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float accelerationFactor = 0.1f;
    [SerializeField] float slowDownFactor = 4;

    [SerializeField] float returnToEqFactor = 2;
    [SerializeField] float maxSpeed = 2.5f;

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)){
            AccelerateTime();
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            SlowDownTime();
        }
        else{

            SetTimeBackToNormal();
        }
    }

    void SetTimeBackToNormal(){
        if(Planet.timeScale > 1){
            Planet.timeScale -= returnToEqFactor*Time.deltaTime*Planet.timeScale;
            Planet.timeScale = Mathf.Max(Planet.timeScale, 1);
        }
        else if(Planet.timeScale < 1){
            Planet.timeScale = Mathf.Max(Planet.timeScale, 0.1f);
            Planet.timeScale += returnToEqFactor*Time.deltaTime*Planet.timeScale;
            Planet.timeScale = Mathf.Min(Planet.timeScale, 1);
            
        }
    }


    void AccelerateTime(){
        Planet.timeScale += accelerationFactor*Time.deltaTime*Planet.timeScale; 
        Planet.timeScale = Mathf.Min(Planet.timeScale, maxSpeed);
    }
    
    void SlowDownTime(){
       Planet.timeScale -= slowDownFactor*Time.deltaTime*Planet.timeScale;
    }

}
