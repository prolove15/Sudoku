using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{

    [SerializeField]
    Paper[] paper_Cps;

    // Start is called before the first frame update
    void Start()
    {
        paper_Cps = FindObjectsOfType<Paper>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            CallingPaperAnimation(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            CallingPaperAnimation(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            CallingPaperAnimation(2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            CallingPaperAnimation(3);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            CallingPaperAnimation(4);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            CallingPaperAnimation(5);
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            CallingPaperAnimation(6);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            CallingPaperAnimation(7);
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            CallingPaperAnimation(8);
        }
    }

    public void CallingPaperAnimation(int paperId)
    {
        paper_Cps[paperId].PlayPaperAnimation();
    }
}
