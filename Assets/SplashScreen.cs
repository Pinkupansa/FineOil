using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    void Start(){
        StartCoroutine(WaitBeforeLoadingMenu());
    }
    IEnumerator WaitBeforeLoadingMenu(){
        yield return new WaitForSeconds(3f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
