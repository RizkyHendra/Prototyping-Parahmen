using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyScript[] enemies;
    public EnemyStruct[] allEnemies;
    private List<int> enemyIndexes;

    [Header("Main AI Loop - Settings")]
    private Coroutine AI_Loop_Coroutine;

    public int aliveEnemyCount;

    bool isPlay1, isPlay2, isPlay3;

    public GameObject Interaction1, Interaction2, Interaction3;
    void Start()
    {
        enemies = GetComponentsInChildren<EnemyScript>();

        allEnemies = new EnemyStruct[enemies.Length];

        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].enemyScript = enemies[i];
            allEnemies[i].enemyAvailability = true;
        }

        StartAI();

        isPlay1 = false;
        isPlay2 = false;
        isPlay3 = false;
    }

    private void Update()
    {
        if(aliveEnemyCount == 3)
        {
            StartCoroutine("InteractionOne");
        }
        if (aliveEnemyCount == 2)
        {
            StartCoroutine("InteractionTwo");
        }
        if (aliveEnemyCount == 1)
        {
            StartCoroutine("InteractionThree");
        }
    }


    public IEnumerator InteractionOne()
    {
        Interaction1.SetActive(true);
        if(!isPlay1 )
        {
            SoundManager.Instance.PlaySFX("SFX - Narrative Arnold (1)");
            isPlay1 = true;
        }
        
        yield return new WaitForSeconds(2.2f);
        Destroy(Interaction1);
    }
    public IEnumerator InteractionTwo()
    {
        
        Interaction2.SetActive(true);
        //Interaction1.SetActive(false);

        if (!isPlay2)
        {
            SoundManager.Instance.PlaySFX("SFX - Narrative Arnold (2)");
            isPlay2 = true;
        }
        yield return new WaitForSeconds(2.2f);
        Destroy(Interaction2);
    }
    public IEnumerator InteractionThree()
    {
        //Interaction2.SetActive(false);
        Interaction3.SetActive(true);

        if (!isPlay3)
        {
            SoundManager.Instance.PlaySFX("SFX - Narrative Arnold (3)");
            isPlay3 = true;
        }
        yield return new WaitForSeconds(2.2f);
        Destroy(Interaction3);
    }
  
    public void StartAI()
    {
        AI_Loop_Coroutine = StartCoroutine(AI_Loop(null));
    }

    IEnumerator AI_Loop(EnemyScript enemy)
    {
        if (AliveEnemyCount() == 0)
        {
            StopCoroutine(AI_Loop(null));
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(.5f,1.5f));

        EnemyScript attackingEnemy = RandomEnemyExcludingOne(enemy);

        if (attackingEnemy == null)
            attackingEnemy = RandomEnemy();

        if (attackingEnemy == null)
            yield break;
            
        yield return new WaitUntil(()=>attackingEnemy.IsRetreating() == false);
        yield return new WaitUntil(() => attackingEnemy.IsLockedTarget() == false);
        yield return new WaitUntil(() => attackingEnemy.IsStunned() == false);

        attackingEnemy.SetAttack();

        yield return new WaitUntil(() => attackingEnemy.IsPreparingAttack() == false);

        attackingEnemy.SetRetreat();

        yield return new WaitForSeconds(Random.Range(0,.5f));

        if (AliveEnemyCount() > 0)
            AI_Loop_Coroutine = StartCoroutine(AI_Loop(attackingEnemy));
    }

    public EnemyScript RandomEnemy()
    {
        enemyIndexes = new List<int>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability)
                enemyIndexes.Add(i);
        }

        if (enemyIndexes.Count == 0)
        {
            return null;
        }
            

        EnemyScript randomEnemy;
        int randomIndex = Random.Range(0, enemyIndexes.Count);
        randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

        return randomEnemy;
    }

    public EnemyScript RandomEnemyExcludingOne(EnemyScript exclude)
    {
        enemyIndexes = new List<int>();

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability && allEnemies[i].enemyScript != exclude)
                enemyIndexes.Add(i);
        }

        if (enemyIndexes.Count == 0)
            return null;

        EnemyScript randomEnemy;
        int randomIndex = Random.Range(0, enemyIndexes.Count);
        randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

        return randomEnemy;
    }

    public int AvailableEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyAvailability)
                count++;
        }
        return count;
    }

    public bool AnEnemyIsPreparingAttack()
    {
        foreach (EnemyStruct enemyStruct in allEnemies)
        {
            if (enemyStruct.enemyScript.IsPreparingAttack())
            {
                return true;
            }
        }
        return false;
    }


    public int AliveEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyScript.isActiveAndEnabled)
                count++;
        }
        aliveEnemyCount = count;
        return count;
    }

    public void SetEnemyAvailiability (EnemyScript enemy, bool state)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].enemyScript == enemy)
                allEnemies[i].enemyAvailability = state;
        }

        if (FindObjectOfType<EnemyDetection>().CurrentTarget() == enemy)
            FindObjectOfType<EnemyDetection>().SetCurrentTarget(null);
    }


}

[System.Serializable]
public struct EnemyStruct
{
    public EnemyScript enemyScript;
    public bool enemyAvailability;
}
