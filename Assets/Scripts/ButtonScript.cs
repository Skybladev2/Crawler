using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public void RestartLevel() 
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Quit() 
    {
        Application.Quit();
    }
}
