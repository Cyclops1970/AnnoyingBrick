using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour {

    public Toggle powerups;
    public GameObject invincible, lrSwap, udSwap, speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(powerups.isOn)
        {
            invincible.SetActive(true);
            udSwap.SetActive(true);
            lrSwap.SetActive(true);
            speed.SetActive(true);
        }
        else
        {
            invincible.SetActive(false);
            udSwap.SetActive(false);
            lrSwap.SetActive(false);
            speed.SetActive(false);
        }
	}
}
