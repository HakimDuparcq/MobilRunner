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

    public CinemachineVirtualCamera cinemachineVCamInGame;
    public CinemachineVirtualCamera cinemachineVCamGameMenu;
    public CinemachineVirtualCamera cinemachineVCamGameMenuTransition;


    public Vector3 cameraRotationStart;
    public Vector3 cameraRotationGame;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetMainCamera(cinemachineVCamGameMenu);
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
                    yCamFollow = hit.point.y + 1;
                }
                else if(hit.transform.GetComponent<PrefabData>().montable == Montable.Ground && Player.instance.gameObject.transform.position.y<2)
                {
                    yCamFollow = Mathf.Lerp(yCamFollow, 1, Time.deltaTime * speedFollow);
                }
                else if (hit.transform.GetComponent<PrefabData>().montable == Montable.No)
                {
                    //dont move cam
                }
            }
        }

        followCam.position = new Vector3(Player.instance.gameObject.transform.position.x / 2f, yCamFollow, Player.instance.gameObject.transform.position.z);
    }


    public IEnumerator ShakeCamera(float intensity, float time)
    {
        cinemachineVCamInGame.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        cinemachineVCamInGame.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
    

    public void SetMainCamera(CinemachineVirtualCamera activeCam)
    {
        cinemachineVCamInGame.Priority = 0;
        cinemachineVCamGameMenu.Priority = 0;
        cinemachineVCamGameMenuTransition.Priority = 0;
        activeCam.Priority = 10;
    }

    public IEnumerator SetCamGameMenuToGame()
    {
        SetMainCamera(cinemachineVCamGameMenuTransition);
        yield return new WaitForSeconds(1);
        SetMainCamera(cinemachineVCamInGame);

    }


}
