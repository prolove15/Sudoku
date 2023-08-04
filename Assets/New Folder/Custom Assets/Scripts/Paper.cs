using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{

    [SerializeField]
    Animator paperAnim_Cp;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayPaperAnimation();
        }
    }
    
    public void StartFlying()
    {
        // gameObject.AddComponent<SmoothMove>();
        // gameObject.AddComponent<SmoothRotation>();
    }

    public void PlayPaperAnimation()
    {
        paperAnim_Cp.Play("Base Layer.Paper Holder 2", 0, 0f);
    }

}
