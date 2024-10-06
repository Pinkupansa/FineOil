using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    public void Die(){
        GetComponent<Animator>().SetBool("dead", true);
    }

    void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.GetComponent<Rocket>() != null){
            transform.right = coll.contacts[0].normal;
            transform.Rotate(new Vector3(0, 0, 135));
            transform.right = -transform.right;
        }
    }
    
    void OnTriggerEnter2D(Collider2D coll){
        if(GameManager.instance.isEnded) return;
        if(coll.gameObject.GetComponent<Rocket>() != null){
            GameManager.instance.DrumRoll();
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if(GameManager.instance.isEnded) return;
        if(coll.gameObject.GetComponent<Rocket>() != null){
            GameManager.instance.EndDrumRoll();
        }
    }
    
    public void SpawnChunks(){

        Instantiate(chunkPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
