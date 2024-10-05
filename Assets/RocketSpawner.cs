using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float rocketOrientation = 0f;

    GameObject lastRocket; 
    // Start is called before the first frame update
    // Update is called once per frame
    void Start(){
        SpawnRocket();
    }
    void SpawnRocket(){
        if (lastRocket != null){
            Destroy(lastRocket);
        }
        lastRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        lastRocket.transform.up = new Vector2(Mathf.Cos(rocketOrientation), Mathf.Sin(rocketOrientation));

    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == lastRocket){
            SpawnRocket();
        }
    }
    void OnDrawGizmos(){
        //draw the rocket orientation
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector2(Mathf.Cos(rocketOrientation), Mathf.Sin(rocketOrientation)));
    }
}
