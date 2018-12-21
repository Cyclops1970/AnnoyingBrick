using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    //public Sprite stateImage;
    public Sprite paused;
    public Sprite notPaused;

    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Image[] i = this.GetComponentsInChildren<Image>();
            i[1].sprite = notPaused;
        }
        else
        {
            Time.timeScale = 1;
            Image[] i = this.GetComponentsInChildren<Image>();
            i[1].sprite = paused;
        }
    }
}
