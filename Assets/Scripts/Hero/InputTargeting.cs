using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargeting : MonoBehaviour
{
    private GameObject selectedHero;

    public bool heroPlayer;

    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        selectedHero = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                var hitTargetable = hit.collider.GetComponent<Targetable>();
                if (hitTargetable != null)
                {
                    if (hitTargetable.enemyType == Targetable.EnemyType.Minion)
                    {
                        if (hit.collider.GetComponent<Minion>().team == selectedHero.GetComponent<HeroMain>().GetTeam())
                            return;
                        selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                    }
                    
                    if (hitTargetable.enemyType == Targetable.EnemyType.Hero)
                    {
                        if (hit.collider.GetComponent<HeroMain>().GetTeam() == selectedHero.GetComponent<HeroMain>().GetTeam())
                            return;
                        selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                    }
                }
                else if (hitTargetable == null)
                {
                    selectedHero.GetComponent<HeroCombat>().targetedEnemy = null;
                }
            }
        }
    }
}
