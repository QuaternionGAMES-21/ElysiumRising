using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotationNormalizer : MonoBehaviour
{
    private GameObject mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Camera");
        }
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, mainCamera.transform.up);
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
