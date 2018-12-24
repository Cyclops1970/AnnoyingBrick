using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour {

    public GameObject helpPanel;

	// Update is called once per frame
	void Update () {
	
        if(Input.GetMouseButtonDown(0))
        {
            helpPanel.SetActive(false);
        }
	}
}
