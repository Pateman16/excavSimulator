using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync(3);
    }
}
