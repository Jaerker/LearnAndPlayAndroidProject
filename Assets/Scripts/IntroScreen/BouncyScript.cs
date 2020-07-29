using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyScript : MonoBehaviour
{
    public float setDistance;
    public float setTime;
    public LeanTweenType easeType;
    // Start is called before the first frame update
    void OnEnable()
    {
        LeanTween.moveLocalY(gameObject, setDistance, setTime).setLoopPingPong().setEase(easeType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
