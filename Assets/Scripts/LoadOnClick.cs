using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public GameObject loadingImage;
    public GameObject menuGrej;

    public void LoadScene(int level)
    {
        loadingImage.SetActive(true);
        menuGrej.SetActive(false);
        SceneManager.LoadSceneAsync(level);
    }
}
