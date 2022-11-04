using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    public static Skin instance;

    public int bonus;
    public int bonusMax;

    public int shield;
    public int shieldMax;

    public GameObject startSkin;
    public GameObject deadSkin;
    public GameObject actualSkin;
    public GameObject previousSkin;
    public GameObject[] goodSkins;
    public GameObject[] badSkins;

    public int skinNumber;

    public ParticleSystem FxTourbillonBleu;
    public ParticleSystem FxHeadStars;
    public Vector3 TourbillonBleuOffset;

    public Transform head;

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

    public void UpdateStats()
    {
        previousSkin = actualSkin;
        startSkin.SetActive(false);
        for (int i = 0; i < goodSkins.Length; i++)
        {
            goodSkins[i].SetActive(false);
        }
        for (int i = 0; i < badSkins.Length; i++)
        {
            badSkins[i].SetActive(false);
        }

        skinNumber = (int)((float)bonus / bonusMax  * goodSkins.Length);
        skinNumber = Mathf.Clamp(skinNumber, -badSkins.Length, goodSkins.Length);
        if (skinNumber==0)
        {
            startSkin.SetActive(true);
            actualSkin = startSkin;
        }
        else if (skinNumber>0)
        {
            goodSkins[skinNumber-1].SetActive(true);
            actualSkin = goodSkins[skinNumber - 1];
        }
        else if (skinNumber<0)
        {
            badSkins[-skinNumber-1].SetActive(true);
            actualSkin = badSkins[-skinNumber - 1];
        }


        if (actualSkin != previousSkin)
        {
            StartCoroutine(PlayParticleSkinChange());
        }
    }

    public IEnumerator PlayParticleHeadStars()
    {
        //Debug.Log("PlayParticleHeadStars");
        ParticleSystem FxHeadStarss = Instantiate(FxHeadStars, gameObject.transform);
        //FxSkinChange.gameObject.transform.localPosition = TourbillonBleuOffset;
        FxHeadStarss.Play();
        int counter = 0;
        while (counter<3)
        {
            FxHeadStarss.gameObject.transform.position = head.position;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(FxHeadStarss.gameObject);
    }


    public IEnumerator PlayParticleSkinChange() // En faire un Feedback
    {
        ParticleSystem FxSkinChange = Instantiate(FxTourbillonBleu, gameObject.transform);
        FxSkinChange.gameObject.transform.localPosition = TourbillonBleuOffset;
        FxSkinChange.Play();
        yield return new WaitForSeconds(3f);
        var em = FxSkinChange.emission;
        em.enabled = false;

        foreach (Transform child in FxSkinChange.gameObject.transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            var emm = ps.emission;
            emm.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        Destroy(FxSkinChange.gameObject);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PrefabData>())
        {
            if (other.GetComponent<PrefabData>().obstacleType== ObstacleType.GoodCoin)
            {
                bonus += 25;
            }
            else if (other.GetComponent<PrefabData>().obstacleType == ObstacleType.BadCoin)
            {
                bonus += -25;
            }
            
            UpdateStats();
            Destroy(other.gameObject);
            AudioManager.instance.Play("Coin");
        }
    }

}
