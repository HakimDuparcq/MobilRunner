using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakim
{
    [Hakim.GameFeedback("Camera/ChangeMainCamera")]
    public class ChangeMainCamera : GameFeedback
    {
        public GameState virtualCam;
        public override IEnumerator Execute(GameEvent gameEvent, GameObject gameObject)
        {
            if (virtualCam==GameState.GameMenu || virtualCam == GameState.Menu)
            {
                CameraMovement.instance.SetMainCamera(CameraMovement.instance.cinemachineVCamGameMenu);
            }
            else if (virtualCam == GameState.InGame )
            {
                CameraMovement.instance.SetMainCamera(CameraMovement.instance.cinemachineVCamInGame);
            }
            else if (virtualCam == GameState.EndGame)
            {
                CameraMovement.instance.SetMainCamera(CameraMovement.instance.cinemachineVCamEndGame);
            }
            yield break;
        }

        public override string ToString()
        {
            return $"Camera :  { virtualCam}";
        }

        public override Color Coloration()
        {
            return Color.black;
        }
    }

}
