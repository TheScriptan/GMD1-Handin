using System;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] public float buildingRotationY = 0;
    [SerializeField] private Barracks barracksPrefab;
    private GameObject[] _heros;
    private bool _canBuild;
    private string _buildingTeam = "";

    // Start is called before the first frame update
    void Start()
    {
        _heros = GameObject.FindGameObjectsWithTag("Hero");
    }

    // Update is called once per frame
    void Update()
    {
        if (_heros.Length != HeroSpawner.maxHeroCount)
        {
            _heros = GameObject.FindGameObjectsWithTag("Hero");
        }
        
        if (HeroSpawner.heroCount != HeroSpawner.maxHeroCount)
        {
            _heros = GameObject.FindGameObjectsWithTag("Hero");
        }

        foreach (GameObject hero in _heros)
        {
            var dist = Vector3.Distance(transform.position, hero.transform.position);
            //AI CODE
            if (hero.GetComponent<HeroAI>() != null)
            {
                if (!(dist < 5)) 
                    continue;
                
                _buildingTeam = hero.GetComponent<HeroMain>().teams.ToString();
                
                if (!GameManager.Instance.IsEnoughGold(_buildingTeam, 50)) 
                    continue;
                BuildBarracks(this, EventArgs.Empty);
                continue;
            }
            
            //PLAYER CODE (such bad structure :(()
            dist = Vector3.Distance(transform.position, hero.transform.position);
            if (dist < 5 && !_canBuild)
            {
                _canBuild = true;
                _buildingTeam = hero.GetComponent<HeroMain>().teams.ToString();
                InputManager.Instance.OnXPressed += BuildBarracks;
                UITooltip.Instance.ShowTooltip("Press X to build Barracks for 150 gold.");
            }
            else if (dist > 7 && _canBuild)
            {
                _canBuild = false;
                InputManager.Instance.OnXPressed -= BuildBarracks;
                UITooltip.Instance.HideTooltip();
                _buildingTeam = "";
            }
        }
    }

    private void BuildBarracks(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsEnoughGold(_buildingTeam, 50))
        {
            //Prompt an error to the player, that he is missing X amount of gold!
            UITooltip.Instance.ShowTooltip($"Not enough gold.");
            return;
        }

        GameManager.Instance.UpdateGold(_buildingTeam, -50);
        UITooltip.Instance.HideTooltip();

        var barracks = Instantiate(barracksPrefab, transform.position, Quaternion.identity);
        barracks.transform.parent = transform.parent;
        var rotation = barracks.transform.eulerAngles;
        rotation.y = buildingRotationY;
        barracks.transform.eulerAngles = rotation;
        barracks.tag = "Barracks";
        barracks.Initialize(_buildingTeam);

        InputManager.Instance.OnXPressed -= BuildBarracks;

        Destroy(gameObject);
    }
}