using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System;

public class GlobalHandler : MonoBehaviour
{


    //Om spelet startats för första gången eller inte
    public static bool gameRunning = false;

    

    // Start is called before the first frame update
    /*
    * Alla kombinationer för olika ljud att kunna slänga in i metoder: 
    * AudioSource.PlayClipAtPoint(GlobalHandler., GlobalHandler.soundSource);
    */

 

 
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
