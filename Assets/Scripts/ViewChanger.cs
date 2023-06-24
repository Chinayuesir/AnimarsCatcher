using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewChanger : MonoBehaviour
{
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;

    private bool mIsThirdView;

    private void Start()
    {
        EnableThirdView();
    }

    public void EnableFirstView()
    {
        firstPersonCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);
    }

    public void EnableThirdView()
    {
        firstPersonCamera.SetActive(false);
        thirdPersonCamera.SetActive(true);
    }

    private void Update()
    {
        // Test
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mIsThirdView = !mIsThirdView;
            if (mIsThirdView)
                EnableThirdView();
            else
                EnableFirstView();
        }
    }
}
