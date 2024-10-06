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

    float buttonPressedTime = 0;

    [SerializeField] AnimationCurve accScaling, decScaling; 

    [SerializeField]float timeAccScaling;

    float startingVel;

    bool actedOnce = false;
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
            buttonPressedTime = 0;
            actedOnce = true;
            startingVel = Planet.timeScale;
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            AccelerateTime();
            buttonPressedTime += Time.deltaTime * timeAccScaling;
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            AccelerateTimeBackwards();
            buttonPressedTime += Time.deltaTime * timeAccScaling;
        }
        else if(actedOnce){

            SetTimeBackToNormal();

            buttonPressedTime -= Time.deltaTime * timeAccScaling;
        }

        buttonPressedTime = Mathf.Clamp(buttonPressedTime, 0, 1);
    }

    void SetTimeBackToNormal(){
        Planet.timeScale -= slowDownFactor*Time.deltaTime*Planet.timeScale;
        Planet.xScale = 1 + (decScaling.Evaluate(buttonPressedTime) - 1)*startingVel;
    }


    void AccelerateTime(){
        Planet.timeScale = Mathf.Max(Planet.timeScale, 1);
        Planet.timeScale += accelerationFactor*Time.deltaTime*Planet.timeScale; 
        Planet.timeScale = Mathf.Min(Planet.timeScale, maxSpeed);

        Planet.xScale = accScaling.Evaluate(buttonPressedTime);
    }
    
    void AccelerateTimeBackwards(){
        Planet.timeScale = Mathf.Min(Planet.timeScale, -1);
        Planet.timeScale += accelerationFactor*Time.deltaTime*Planet.timeScale; 
        Planet.timeScale = Mathf.Max(Planet.timeScale, -maxSpeed);

        Planet.xScale = accScaling.Evaluate(buttonPressedTime); 
    }

}
