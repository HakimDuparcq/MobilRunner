using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Menu, GameMenu, InGame, EndGame}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Player.instance.Tap && gameState==GameState.GameMenu)
        {
            gameState = GameState.InGame;
            Player.instance.animator.SetTrigger("Run");
            CameraMovement.instance.SetMainCamera(CameraMovement.instance.cinemachineVCamInGame);
        }
    }
}
