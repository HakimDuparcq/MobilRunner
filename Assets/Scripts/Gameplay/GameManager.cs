using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hakim;
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

    }

    public void StartGame()
    {
        if (gameState == GameState.GameMenu)
        {
            gameState = GameState.InGame;
            Player.instance.animator.SetTrigger("Run");
            StartCoroutine(CameraMovement.instance.SetCamGameMenuToGame());
            StartCoroutine(UiManager.instance.MenuToGame());
        }
    }

    public void EndGameStateAction(bool isFinishMap)
    {
        MapController.instance.speedMap = 0;
        gameState = GameState.EndGame;
        if (isFinishMap)
        {
            GameEventsManager.PlayEvent("End Finish Game Event", gameObject);
        }
        else
        {
            GameEventsManager.PlayEvent("End Dead Game Event", gameObject);
        }

    }

    public void ResetGame()
    {
        gameState = GameState.GameMenu;
        PatternManager.instance.ResetMap();
        Player.instance.ResetPlayer();
        MapController.instance.speedMap = 3;
        //MapController.instance.Map.transform.position = MapController.instance.startMapPosition;
        CameraMovement.instance.SetMainCamera(CameraMovement.instance.cinemachineVCamGameMenu);
        PlayerStats.instance.actualScore = 0;
        Skin.instance.Reset();
    }

    public void SpawnParticleEvent()
    {
        StartCoroutine(SpawnParticle());
    }

    public IEnumerator SpawnParticle()
    {
        List<GameObject> particles = new List<GameObject>();
        if (Skin.instance.skinNumber>0)
        {
            for (int i = 0; i < Skin.instance.skinNumber; i++)
            {
                particles.Add(Instantiate(Skin.instance.FxGoodCoin.gameObject, Player.instance.gameObject.transform.position, Quaternion.identity));
                StartCoroutine(MoveParticleEnd(particles[particles.Count - 1]));
                yield return new WaitForSeconds(0.5f);

            }
        }
        else
        {
            for (int i = 0; i < -Skin.instance.skinNumber; i++)
            {
                particles.Add(Instantiate(Skin.instance.FxBadCoin.gameObject, Player.instance.gameObject.transform.position, Quaternion.identity));
                StartCoroutine(MoveParticleEnd(particles[particles.Count - 1]));
                yield return new WaitForSeconds(0.5f);

            }
        }

        
    }

    public IEnumerator MoveParticleEnd(GameObject particle)
    {
        particle.GetComponent<Collider>().enabled = false;
        float speed = 10;
        for (int i = 0; i < 100; i++)
        {
            Vector3 endPos = GameObject.FindGameObjectsWithTag("Chest")[0].transform.position;
            particle.transform.position = Vector3.Lerp(particle.transform.position, endPos, Time.deltaTime*speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(particle);

    }

    public IEnumerator ChestAnimation()
    {
        GameObject chest = GameObject.FindGameObjectsWithTag("Chest")[0];
        yield return new WaitForSeconds(1);
        chest.GetComponent<Animator>().SetTrigger("Surprise");

        if (Skin.instance.skinNumber>=2)
        {
            yield return new WaitForSeconds(1);
            chest.GetComponent<Animator>().SetTrigger("Open");
        }

    }

    public IEnumerator PlayDefeatVictoryAnim()
    {
        yield return new WaitForSeconds(1);
        if (Skin.instance.skinNumber >= 2)
        {
            Player.instance.animator.SetTrigger("Victory");
        }
        else
        {
            Player.instance.animator.SetTrigger("Defeat");
        }
        
        
    }

    public IEnumerator PlayAnimation(string name, float time)
    {
        yield return new WaitForSeconds(time);
        Player.instance.animator.SetTrigger(name);
    }

}
