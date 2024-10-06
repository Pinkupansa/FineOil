using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Chunk : MonoBehaviour
{
    [SerializeField] Transform epicenter;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce((transform.position - epicenter.position)*10, ForceMode2D.Impulse);
    }
}
