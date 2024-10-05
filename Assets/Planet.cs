using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static float timeScale = 1;
    [SerializeField] Transform focus;
    [SerializeField] float semiMajorAxis, eccentricity, angularMomentum, angularPhase;
    [SerializeField] float gravitationalPull; 
    [SerializeField] float gravitationalPullRadius;

    

    float currentTheta = 0;
    void Pull(Rigidbody2D rb){
        
        Vector2 dir = rb.position - (Vector2)transform.position; 
        if(dir.sqrMagnitude > gravitationalPullRadius*gravitationalPullRadius) return;
        rb.AddForce(-gravitationalPull*dir.normalized/dir.sqrMagnitude);
    }
    void FixedUpdate(){
        foreach(Rocket r in FindObjectsOfType<Rocket>()){
            Pull(r.GetComponent<Rigidbody2D>());
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravitationalPullRadius);

        Gizmos.color = Color.green;
        //draw trajectory
        float theta = 0;
        float dTheta = 0.1f;
        while(theta <= 2*Mathf.PI){
            float r = semiMajorAxis/(1 + eccentricity*Mathf.Cos(theta + angularPhase));
            Vector2 pos = (Vector2)focus.position + r*new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            Gizmos.DrawWireSphere(pos, 0.1f);
            theta += dTheta;
        }
    }

    void Update(){
       float currentDistToFocusSqr = ((Vector2)focus.position - (Vector2)transform.position).sqrMagnitude;
       currentTheta += timeScale*Time.deltaTime/currentDistToFocusSqr * angularMomentum;
       float distToFocus =  semiMajorAxis/(1 + eccentricity*Mathf.Cos(currentTheta + angularPhase));

       transform.position = (Vector2)focus.position + distToFocus*new Vector2(Mathf.Cos(currentTheta),Mathf.Sin(currentTheta));
    }

}
