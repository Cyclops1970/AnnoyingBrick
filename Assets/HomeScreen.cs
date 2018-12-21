using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour {

    public Toggle powerups;
    public GameObject invincible, reverse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(powerups.isOn)
        {
            invincible.SetActive(true);
            reverse.SetActive(true);
        }
        else
        {
            invincible.SetActive(false);
            reverse.SetActive(false);
        }
	}
}
