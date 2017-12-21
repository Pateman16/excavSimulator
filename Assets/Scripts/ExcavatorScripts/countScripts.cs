using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class countScripts : MonoBehaviour {
    public Text counter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        counter.text = objects.ToString();
        Debug.Log(objects);
        if(objects == 50)
        {
            SceneManager.LoadSceneAsync(2);
        }
	}

    private int objects = 0;
    void OnTriggerEnter(Collider other)
    {
        objects++;

    }
    void OnTriggerExit(Collider other)
    {
        objects--;
    }

}
