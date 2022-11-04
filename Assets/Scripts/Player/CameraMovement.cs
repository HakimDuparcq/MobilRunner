using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public  float yCamFollow;
    public float speedFollow;

    public LayerMask Ground;

    public Transform followCam;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public Vector3 cameraRotationStart;
    public Vector3 cameraRotationGame;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player.instance.gameObject.transform.position + new Vector3(0,2,0), Vector3.down, out hit, Mathf.Infinity, Ground))
        {
            Debug.DrawRay(Player.instance.gameObject.transform.position + new Vector3(0, 2, 0), Vector3.down, Color.red, 2);
            if (hit.transform.GetComponent<PrefabData>())
            {
                if (hit.transform.GetComponent<PrefabData>().montable == Montable.Yes)
                {
                    yCamFollow = hit.point.y;
                    Debug.Log("monte");
                }
                else
                {
                    yCamFollow = Mathf.Lerp(yCamFollow, Player.instance.gameObject.transform.position.y, Time.deltaTime * speedFollow);
                }
            }
        }

        followCam.position = new Vector3(Player.instance.gameObject.transform.position.x / 2f, yCamFollow, Player.instance.gameObject.transform.position.z);
    }


    public IEnumerator ShakeCamera(float intensity, float time)
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

    }

    public void SetRotationCamera(Vector3 offset)
    {
        cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = offset; 
    }



}
