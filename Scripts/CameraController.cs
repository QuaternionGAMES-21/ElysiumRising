using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        GameObject c = GameObject.Find("Camera");
        CinemachineVirtualCamera vc = c.GetComponent<CinemachineVirtualCamera>();
        vc.Follow = transform;
        vc.LookAt = transform;
    }
}