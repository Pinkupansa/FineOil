using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject baseMenu;
    [SerializeField] GameObject levelMenu;

    void Start(){
        baseMenu.SetActive(true);
        levelMenu.SetActive(false);
    }
    public void Play(){
        SceneManager.LoadScene("Level 1");
    }
    public void Levels(){
        levelMenu.SetActive(true);
        baseMenu.SetActive(false);
    }
    public void BackToBaseMenu(){
        levelMenu.SetActive(false);
        baseMenu.SetActive(true);
    }
    public void Exit(){
        Application.Quit();
    }
    public void LoadLevel(int i){
        SceneManager.LoadScene("Level " + i.ToString());
    }
}
