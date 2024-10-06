using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject endMenu;
    [SerializeField] GameObject missileText;

    int missileCount = 0; 
    
    public bool isEnded {get; private set;}
    // Start is called before the first frame update
    void Awake()
    {
       instance = this; 
    }
    void Start(){
        endMenu.SetActive(false);
    }
    // Update is called once per frame
    public void OnGameEnd()
    {
        isEnded = true;
        StartCoroutine(WaitBeforeShowingMenu());
        CameraManager.instance.ZoomIn();
        CameraManager.instance.Track(Rocket.instance.transform);
        CameraManager.instance.ScreenShake(1000);
    }
    public void OnMissileSpawned()
    {
        missileCount ++; 
        missileText.GetComponent<TMP_Text>().text = "Missile " + missileCount.ToString();
    }

    public void Replay(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void Next(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator WaitBeforeShowingMenu(){
        yield return new WaitForSeconds(3f);
        endMenu.SetActive(true);

    }

    public void DrumRoll(){
        Time.timeScale = 0.5f;
        CameraManager.instance.Track(Rocket.instance.transform);
        CameraManager.instance.ZoomIn();

    }
    public void EndDrumRoll(){
        Time.timeScale = 1f;
        CameraManager.instance.StopTracking();
        CameraManager.instance.ZoomOut();
    }
}
