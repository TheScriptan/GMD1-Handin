using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField] private Minion minionPrefab;

    private Queue<Minion> _spawnQueue = new Queue<Minion>();
    private IEnumerator _spawningCoroutine;

    private GameObject[] _heros;
    private bool _canInteract = false;

    [SerializeField]
    private string _team;

    // Start is called before the first frame update
    void Start()
    {
        _heros = GameObject.FindGameObjectsWithTag("Hero");
        _spawningCoroutine = SpawnMinions();
        StartCoroutine(_spawningCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        IsHeroNear();
    }

    public void Initialize(string team)
    {
        _team = team;
    }

    public string GetTeam() => _team;

    private void IsHeroNear()
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
            var heroTeam = hero.GetComponent<HeroMain>().teams.ToString();
            var dist = Vector3.Distance(transform.position, hero.transform.position);
            
            if (heroTeam != _team)
                continue;
            
            if (hero.GetComponent<HeroAI>() != null)
            {
                if (!(dist < 5)) 
                    continue;
                
                if (!GameManager.Instance.IsEnoughGold(_team, 25)) 
                    continue;
                
                QueueMinion(this, EventArgs.Empty);
                continue;
            }

            if (dist < 5 && !_canInteract)
            {
                _canInteract = true;
                InputManager.Instance.OnXPressed += QueueMinion;
                UITooltip.Instance.ShowTooltip("Press X to queue a Minion for 25 gold.");
            }
            else if (dist > 7 && _canInteract)
            {
                _canInteract = false;
                InputManager.Instance.OnXPressed -= QueueMinion;
                UITooltip.Instance.HideTooltip();
            }
        }
    }

    private void QueueMinion(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsEnoughGold(_team, 25))
        {
            UITooltip.Instance.ShowTooltip("Not enough gold");
            return;
        }

        GameManager.Instance.UpdateGold(_team, -25);
        var pos = gameObject.transform.GetChild(0).position;
        var minion = Instantiate(minionPrefab, pos, Quaternion.identity);
        minion.Initialize(_team);
        minion.gameObject.SetActive(false);
        _spawnQueue.Enqueue(minion);
    }

    private IEnumerator SpawnMinions()
    {
        while (true)
        {
            if (_spawnQueue.Count > 0)
            {
                var minion = _spawnQueue.Dequeue();
                minion.gameObject.SetActive(true);
                yield return new WaitForSeconds(3);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawningCoroutine);
    }
}