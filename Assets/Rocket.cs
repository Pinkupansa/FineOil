using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    Rigidbody2D rb;
    //int leftRight = 1;
    //[SerializeField] float thrust, angularVelocity;
    [SerializeField] float baseVelocity = 10;

    [SerializeField] float predictionLength, predictionStep;

    [SerializeField] GameObject trajectoryRendererPrefab;
    [SerializeField] int numberOfTrajectoryRenderers;

    GameObject[] trajectoryRenderers;
    Planet[] planets;
    bool isDying = false;

    public static Rocket instance = null;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * baseVelocity;
        planets = FindObjectsOfType<Planet>(); 
        trajectoryRenderers = new GameObject[numberOfTrajectoryRenderers];
        for(int i = 0; i < trajectoryRenderers.Length; i++){
            
            trajectoryRenderers[i] = Instantiate(trajectoryRendererPrefab, transform);
        }
        instance = this;
    }
    public bool IsDying(){return isDying;}
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
        if(!isDying){
            transform.up = rb.velocity.normalized;
        }
        /*
        if(Input.GetKeyUp(KeyCode.DownArrow)){
            SwitchDir();
        }*/
        RenderTrajectory();
    }


    void RenderTrajectory(){
        Vector2[] trajectory = CalculateTrajectory();
        for (int i = 0; i < trajectoryRenderers.Length; i++){
            float fraction = (float)i / (trajectoryRenderers.Length - 1);
            int index = (int)Mathf.Lerp(0, trajectory.Length - 1, fraction);
            trajectoryRenderers[i].transform.position = trajectory[index];
        }
    }
    Vector2[] CalculateTrajectory(){
        Vector2[] trajectory = new Vector2[(int)(predictionLength/predictionStep)];

        trajectory[0] = transform.position;
        Vector2 currentVel = rb.velocity;
        for(int i = 1; i < trajectory.Length; i++){
            Vector2 resultantGravPull = Vector2.zero;

            foreach(Planet planet in planets){
                Vector2 planetPos = planet.transform.position;
                float distSqr = (planetPos - trajectory[i-1]).sqrMagnitude;
                if(distSqr < planet.GetPullRadius() * planet.GetPullRadius()){
                    resultantGravPull += (planetPos - trajectory[i-1]).normalized / distSqr * planet.GetPullForce();
                }
            }

            trajectory[i] = trajectory[i-1] + currentVel * predictionStep + 0.5f * resultantGravPull * predictionStep * predictionStep;
            currentVel += resultantGravPull * predictionStep;
            
        }

        return trajectory;
            
    }

    void OnDrawGizmos(){
        //if game not playing, dont draw
        if(!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Vector2[] trajectory = CalculateTrajectory();
        for(int i = 0; i < trajectory.Length - 1; i++){
            Gizmos.DrawLine(trajectory[i], trajectory[i+1]);
        }
    }
    void Explode(){
        GetComponent<Animator>().SetBool("dead", true);
        transform.GetChild(0).gameObject.SetActive(false);
        isDying = true;
    }

    public void DestroyGO(){
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.tag == "Planet"){
            //reflect velocity around normal
            Vector2 normal = coll.contacts[0].normal;
            rb.velocity = Vector2.Reflect(rb.velocity, normal)*5f;
            int leftOrRight = Vector2.Angle(normal, rb.velocity) < 180?1:-1;
            Debug.DrawRay(transform.position, Vector2.Reflect(rb.velocity, normal)*10f, Color.red, 1);
            rb.angularVelocity = 1000f*leftOrRight;
            Explode();
            GetComponent<Collider2D>().isTrigger = true;
            //transform.parent = other.transform;


        }
        else if(coll.gameObject.tag == "Target"){
            coll.gameObject.GetComponent<Target>().Die();
            GameManager.instance.OnGameEnd();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Planet"){
            rb.velocity = rb.velocity.normalized*baseVelocity;
        }
    }

}
