using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float rocketOrientation = 0f;
    [SerializeField] GameObject departurePrefab;
    GameObject lastRocket; 
    // Start is called before the first frame update
    // Update is called once per frame
    void Start(){
        SpawnRocket();
    }
    void SpawnRocket(){
        GameManager.instance.OnMissileSpawned();
        if (lastRocket != null){
            Destroy(lastRocket);
        }
        lastRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        lastRocket.transform.up = new Vector2(Mathf.Cos(rocketOrientation), Mathf.Sin(rocketOrientation));

        GameObject departure = Instantiate(departurePrefab, transform.position, Quaternion.identity);
        departure.transform.up = new Vector2(Mathf.Cos(rocketOrientation), Mathf.Sin(rocketOrientation));
        departure.transform.position += departure.transform.up * 0.5f;  
        Destroy(departure, 0.5f);
    }

    void OnTriggerExit2D(Collider2D other) {
        if(GameManager.instance.isEnded) return;
        if(other.gameObject == lastRocket){
            SpawnRocket();

        }
    }
    void OnDrawGizmos(){
        //draw the rocket orientation
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector2(Mathf.Cos(rocketOrientation), Mathf.Sin(rocketOrientation)));
    }

    void OnApplicationQuit(){
        Destroy(lastRocket);
    }
}
