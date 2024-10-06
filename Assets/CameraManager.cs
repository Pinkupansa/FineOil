using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    Transform currentTarget;
    bool isZooming =false;
    bool isShaking = false;

    Vector3 basePos;
    float baseScale;
    void Awake(){
        
        if(instance == null){
            instance = this;
        }
    }
 public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    int numberOfShakes = 60;
    
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;

        basePos = transform.position;
        baseScale = GetComponent<Camera>().orthographicSize;
    }

    void Update(){
        if(currentTarget != null){
            transform.position = Vector3.Lerp(transform.position, new Vector3(currentTarget.position.x, currentTarget.position.y, transform.position.z), 0.1f);
        }
        else{
            transform.position = Vector3.Lerp(transform.position, basePos, 0.1f);
        }
        if(isZooming){
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 3f, 0.001f);
        }
        else{
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, baseScale, 0.001f);
        }
        if(isShaking){
            transform.position = transform.position + Random.insideUnitSphere * shakeMagnitude * Mathf.Sqrt(numberOfShakes/1000f);
            numberOfShakes--;
            if(numberOfShakes <= 0){
                isShaking = false;
            }
        }
    }
    public void ScreenShake(int nbShakes)
    {
        isShaking = true;
        numberOfShakes = nbShakes;
    }

    private void ResetPosition()
    {
        // Reset the camera or object to its original position
        transform.position = originalPosition;
    }

    public void ZoomIn(){
        isZooming = true;
    }

    public void ZoomOut(){
        isZooming = false;
    }
    public void Track(Transform target){
        currentTarget = target;
    }
    public void StopTracking(){
        currentTarget = null;
    }

    
}
