using System.Collections;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{

    public static int heroCount = 0;
    public static int maxHeroCount = 0;
    private Vector3 _teamRedLoc = new Vector3(0f, 0f, 7f);
    private Vector3 _teamBlueLoc = new Vector3(0f, 0f, -12f);

    [SerializeField]
    private HeroMain heroPlayerPrefab;
    [SerializeField]
    private HeroMain heroAIPrefab;
    
    void Start()
    {
        var heroPlayer = Instantiate(heroPlayerPrefab, _teamRedLoc, Quaternion.identity);
        var heroAI = Instantiate(heroAIPrefab, _teamBlueLoc, Quaternion.identity);
        heroCount = 2;
        maxHeroCount = heroCount;
        Camera.main.GetComponent<CameraFollow>().target = heroPlayer.transform;
    }
    
    void Update()
    {
        if (heroCount < 2)
        {
            var heros = GameObject.FindGameObjectsWithTag("Hero");
            
            //In the future might give some unreasonable execution
            foreach (GameObject hero in heros)
            {
                if (hero.GetComponent<HeroAI>() == null)
                {
                    heroCount++;
                    StartCoroutine(SpawnHeroAI());
                } else if (hero.GetComponent<HeroPlayerMovement>() == null)
                {
                    heroCount++;
                    StartCoroutine(SpawnHeroPlayer());
                }
                else
                {
                    heroCount++;
                    heroCount++;
                    StartCoroutine(SpawnHeroAI());
                    StartCoroutine(SpawnHeroPlayer());
                }
            }
        }
    }

    private IEnumerator SpawnHeroPlayer()
    {
        yield return new WaitForSeconds(3);
        var heroPlayer = Instantiate(heroPlayerPrefab, _teamRedLoc, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().target = heroPlayer.transform;
    }

    private IEnumerator SpawnHeroAI()
    {
        yield return new WaitForSeconds(3);
        var heroAI = Instantiate(heroAIPrefab, _teamBlueLoc, Quaternion.identity);
    }
}
