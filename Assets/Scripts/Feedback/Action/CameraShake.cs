using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Camera/CameraShake")]
    public class CameraShake : GameFeedback
    {
        public float intensity;
        public float time;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            CameraMovement.instance.cinemachineVCamInGame.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            yield return new WaitForSeconds(time);
            CameraMovement.instance.cinemachineVCamInGame.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        }

        public override string ToString()
        {
            return $"Shake => intensity :  {intensity}, time : {time} ";
        }

        public override Color Coloration()
        {
            return Color.blue;
        }


    }


}
