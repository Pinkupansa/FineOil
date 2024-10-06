using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static float timeScale = 1;
    public static float xScale = 1;
    
    [SerializeField] Transform focus;
    [SerializeField] Transform gravityDisc;
    [SerializeField] float semiMajorAxis, eccentricity, angularMomentum, angularPhase;
    [SerializeField] float gravitationalPull; 
    [SerializeField] float gravitationalPullRadius;

    [SerializeField] GameObject trajectoryRenderer;

    Vector3[] trajectory;

    float currentTheta = 0;

    Vector3 baseScale;
    void Start(){
        baseScale = transform.localScale;
        trajectory = CalculateTrajectory();

        if(trajectoryRenderer != null){
            trajectoryRenderer.GetComponent<LineRenderer>().positionCount = trajectory.Length;
            trajectoryRenderer.GetComponent<LineRenderer>().SetPositions(trajectory);
        }
        gravityDisc.localScale = Vector3.one * gravitationalPullRadius;
    }
    void Pull(Rigidbody2D rb){
        
        Vector2 dir = rb.position - (Vector2)transform.position; 
        if(dir.sqrMagnitude > gravitationalPullRadius*gravitationalPullRadius) return;
        rb.AddForce(-gravitationalPull*dir.normalized/dir.sqrMagnitude);
    }
    void FixedUpdate(){
        foreach(Rocket r in FindObjectsOfType<Rocket>()){
            if(r.IsDying()) continue;
            Pull(r.GetComponent<Rigidbody2D>());
        }
    }

    Vector3[] CalculateTrajectory(){
        if(focus == null) return new Vector3[0];
        float dTheta = 0.1f;
        float theta = 0;
        List<Vector3> trajectory = new List<Vector3>();

        while(theta <= 2*Mathf.PI){
            float r = semiMajorAxis/(1 + eccentricity*Mathf.Cos(theta + angularPhase));
            Vector2 pos = (Vector2)focus.position + r*new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            trajectory.Add(pos);
            theta += dTheta;
        }

        return trajectory.ToArray();
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravitationalPullRadius);

        Gizmos.color = Color.green;

        Vector3[] traj = CalculateTrajectory();
        //draw trajectory
        for (int i = 0; i < traj.Length; i++){
            Gizmos.DrawLine(traj[i], traj[(i+1)%traj.Length]);    
        }
    }

    void Update(){
        float currentDistToFocusSqr = ((Vector2)focus.position - (Vector2)transform.position).sqrMagnitude;
        currentTheta += timeScale*Time.deltaTime/currentDistToFocusSqr * angularMomentum;
        float distToFocus =  semiMajorAxis/(1 + eccentricity*Mathf.Cos(currentTheta + angularPhase));

        Vector2 previousPos = transform.position;
        transform.position = (Vector2)focus.position + distToFocus*new Vector2(Mathf.Cos(currentTheta),Mathf.Sin(currentTheta));
        transform.right = (Vector2)transform.position - previousPos;

        transform.localScale = new Vector3(baseScale.x * xScale, baseScale.y/xScale, baseScale.z);

        gravityDisc.transform.position = transform.position;	
       
    }
    
    public float GetPullRadius(){return gravitationalPullRadius;}
    public float GetPullForce(){return gravitationalPull;}
}
