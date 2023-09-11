using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CombatScript : MonoBehaviour
{
    
    public TacticalMode tacticalMode;
    private EnemyManager enemyManager;
    private EnemyDetection enemyDetection;
    private MovementInput movementInput;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;

    [Header("Scenemanager Game")]
    public string MainMenu;
    [Header("Stamina Bar")]
    public float stamina;
    public Image staminaImageBar;
    float maxStamina;
    public Slider staminaBar;
    public float dValue;

    [Header("Health")]
    public float health;
    float maxHealth;
    public Slider healthBar;
    public Image healthImageBar;
    public GameObject LosePanel;
    public Animator HitByEnemy;

    [Header("Rage Bar")]
    public float staminaRage;
    public Image barImageFill;
    public Slider staminaBarRage;
    public float dValueRage;
    public GameObject particelEffect1;
    

    [Header("Target")]
    public EnemyScript lockedTarget;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown;

    [Header("States")]
    public bool isAttackingEnemy = false;
    public bool isCountering = false;
    [Header("Public References")]
    [SerializeField] private Transform punchPosition;
    [SerializeField] private ParticleSystemScript punchParticle;
    [SerializeField] private GameObject lastHitCamera;
    [SerializeField] private GameObject RageMode;
    [SerializeField] private GameObject CounterCamera;
    [SerializeField] private Transform lastHitFocusObject;
    [SerializeField] private GameObject WinPanel;

    //Coroutines
    private Coroutine counterCoroutine;
    private Coroutine attackCoroutine;
    private Coroutine damageCoroutine;

    [Space]

    //Events
    public UnityEvent<EnemyScript> OnTrajectory;
    public UnityEvent<EnemyScript> OnHit;
    public UnityEvent<EnemyScript> OnCounterAttack;

    int animationCount = 0;
    string[] attacks;

    void Start()
    {
        // Stamina Bar
        maxStamina = stamina;
        staminaBar.maxValue = maxStamina;


        maxHealth = health;
        healthBar.maxValue = maxHealth;

       
        staminaBarRage.value = 0;

        enemyManager = FindObjectOfType<EnemyManager>();
        animator = GetComponent<Animator>();
        enemyDetection = GetComponentInChildren<EnemyDetection>();
        movementInput = GetComponent<MovementInput>();
        impulseSource = GetComponentInChildren<CinemachineImpulseSource>();

        //lockedTarget = enemyDetection.CurrentTarget();
    }

    private void Update()
    {
        if(isAttackingEnemy == false)
        {
            if (stamina != maxStamina)
            {
                IncreseEnergy();
            }
        }
       
        IncreseEnergyRage();
        staminaBar.value = stamina;
        staminaImageBar.fillAmount = stamina / maxStamina;
        healthBar.value = health;
        healthImageBar.fillAmount = health / maxHealth;
        staminaBarRage.value = staminaRage;
        barImageFill.fillAmount = staminaRage/100;

        if(health == 0)
        {
            LosePanel.SetActive(true);
            CursorManager.Instance.EnableCursorMouse();

        }
        if(staminaRage <= 0)
        {
            staminaRage = 0;
        }

        lockedTarget = enemyDetection.newCurrentTarget;

    }

    private void DecreaseEnergyStamina()
    {
        if (stamina != 0)
            stamina -= dValue;
        if (stamina <= -1)
            stamina = 0;
    }

    private void DecreaseEnergyRage()
    {
        if (staminaRage != 0)
            staminaRage -= dValue;
        if (staminaRage <= -1)
            staminaRage = 0;
    }
    private void IncreseEnergy()
    {

        stamina += dValue * Time.deltaTime / .70f;
        if(stamina >= maxStamina)
        {
            stamina = maxStamina;
        }

    }
    private void IncreseEnergyRage()
    {
        
        staminaRage -= dValueRage * Time.deltaTime / .70f;
        if (staminaRage <= 0)
        {
            particelEffect1.SetActive(false);
            staminaRage = 0;
        }
        else
        {
            StartCoroutine(cooldown());
        }

    }
    IEnumerator cooldown()
    {
        
        yield return new WaitForSeconds(3f);
        particelEffect1.SetActive(true);
        tacticalMode.seblakCyborgOn = false;
        
    }
    public void RageStamina(float rageUp)
    {
        staminaRage += rageUp;
        
    }
    public void StaminaUp(float staminaUp)
    {
        stamina += staminaUp;
    }
    public void HealthUp(int healthUp)
    {
        health += healthUp;
    }
    //This function gets called whenever the player inputs the punch action
    void AttackCheck()
    {
        if (isAttackingEnemy )

            return;
        //Check to see if the detection behavior has an enemy set
        if (enemyDetection.CurrentTarget() == null)
        {
            if (enemyManager.AliveEnemyCount() == 0)
            {
                Attack(null, 0);
                return;
            }
            else
            {
                //lockedTarget = enemyManager.RandomEnemy();
                //lockedTarget = enemyDetection.CurrentTarget();
            }
        }

        //If the player is moving the movement input, use the "directional" detection to determine the enemy
        if (enemyDetection.InputMagnitude() > .2f)
            //lockedTarget = enemyDetection.CurrentTarget();

        //Extra check to see if the locked target was set
        if(lockedTarget == null)
        {
            //lockedTarget = enemyManager.RandomEnemy();
            //lockedTarget = enemyDetection.CurrentTarget();
        }
            


        //AttackTarget
        if(stamina > dValue)
        {
            DecreaseEnergyStamina();
            Attack(lockedTarget, TargetDistance(lockedTarget));
        }
     
         if(stamina < 0)
        {
            stamina = 0;
           
        }
        
    }
    public void Attack(EnemyScript target, float distance)
    {
        //Types of attack animation
        attacks = new string[] { "AirKick", "AirKick2", "AirPunch", "AirKick3" };
        //Attack nothing in case target is null
        if (target == null)
        {
            AttackType("GroundPunch", .2f, null, 0);
            return;
        }

        if (distance < 15)
        {
            DecreaseEnergyStamina();
            animationCount = (int)Mathf.Repeat((float)animationCount + 1, (float)attacks.Length);
            string attackString = isLastHit() ? attacks[Random.Range(0, attacks.Length)] : attacks[animationCount];
            AttackType(attackString, attackCooldown, target, .65f);
        }
        else
        {
            
            lockedTarget = null;
            AttackType("GroundPunch", .2f, null, 0);
        }
        //Change impulse
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = Mathf.Max(3, 1 * distance);
    }
    void AttackType(string attackTrigger, float cooldown, EnemyScript target, float movementDuration)
    {
        animator.SetTrigger(attackTrigger);

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackCoroutine(isLastHit() ? 1.5f : cooldown));

        //Check if last enemy
        if (isLastHit())
            StartCoroutine(FinalBlowCoroutine());

        if (target == null)
            return;

        target.StopMoving();
        MoveTorwardsTarget(target, movementDuration);

        IEnumerator AttackCoroutine(float duration)
        {
            movementInput.acceleration = 0;
            isAttackingEnemy = true;
            movementInput.enabled = false;
            yield return new WaitForSeconds(duration);
            isAttackingEnemy = false;
            yield return new WaitForSeconds(.2f);
            movementInput.enabled = true;
            LerpCharacterAcceleration();
        }
         IEnumerator FinalBlowCoroutine()
        {
            Time.timeScale = .5f;
            lastHitCamera.SetActive(true);
            lastHitFocusObject.position = lockedTarget.transform.position;
            yield return new WaitForSecondsRealtime(2);
            // Game Win
            
            lastHitCamera.SetActive(false);
            Time.timeScale = 1f;
            
            yield return new WaitForSecondsRealtime(5);
            WinPanel.SetActive(true);
            CursorManager.Instance.EnableCursorMouse();

        }
    }

    public void SceneMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void SceneRetri()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MoveTorwardsTarget(EnemyScript target, float duration)
    {
        OnTrajectory.Invoke(target);
        transform.DOLookAt(target.transform.position, .2f);
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    void CounterCheck()
    {
        //Initial check
        if (isCountering || isAttackingEnemy || !enemyManager.AnEnemyIsPreparingAttack())
            return;

        lockedTarget = ClosestCounterEnemy();
        OnCounterAttack.Invoke(lockedTarget);
        StartCoroutine(wow());
        //if (TargetDistance(lockedTarget) > 2)
        //{
        //    Attack(lockedTarget, TargetDistance(lockedTarget));
        //    return;
        //}
       
        float duration = .1f;
        animator.SetTrigger("Dodge");
        transform.DOLookAt(lockedTarget.transform.position, .2f);
        transform.DOMove(transform.position + lockedTarget.transform.forward, duration);

        if (counterCoroutine != null)
            StopCoroutine(counterCoroutine);
        counterCoroutine = StartCoroutine(CounterCoroutine(duration));


        IEnumerator wow()
        {
            Time.timeScale = .5f;
            CounterCamera.SetActive(true);
            lastHitFocusObject.position = lockedTarget.transform.position;
            yield return new WaitForSecondsRealtime(1);
            CounterCamera.SetActive(false);
            Time.timeScale = 1f;
        }
        IEnumerator CounterCoroutine(float duration)
        {
            isCountering = true;
            movementInput.enabled = false;
            yield return new WaitForSeconds(.5f);
            Attack(lockedTarget, TargetDistance(lockedTarget));
            isCountering = false;

        }
    }

    float TargetDistance(EnemyScript target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .50f);
    }

    public void HitEvent()
    {
        if (lockedTarget == null || enemyManager.AliveEnemyCount() == 0)
            return;
        tacticalMode.ModifyATB(25);
        OnHit.Invoke(lockedTarget);
        Score.scoreValue += 1;
        Score.animCombo.Play("ComboAnim", 0, 0);

        //Polish
        punchParticle.PlayParticleAtPosition(punchPosition.position);
    }

    public void DamageEvent()
    {
        if(tacticalMode.seblakCyborgOn == false)
        {
            animator.SetTrigger("Hit");
        }
       
       
        health -= 10;
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(DamageCoroutine());

        IEnumerator DamageCoroutine()
        {
            movementInput.enabled = false;
            yield return new WaitForSeconds(1);
            movementInput.enabled = true;
            LerpCharacterAcceleration();
        }
    }

    EnemyScript ClosestCounterEnemy()
    {
        float minDistance = 100;
        int finalIndex = 0;

        for (int i = 0; i < enemyManager.allEnemies.Length; i++)
        {
            EnemyScript enemy = enemyManager.allEnemies[i].enemyScript;

            if (enemy.IsPreparingAttack())
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, enemy.transform.position);
                    finalIndex = i;
                }
            }
        }

        return enemyManager.allEnemies[finalIndex].enemyScript;

    }

    void LerpCharacterAcceleration()
    {
        movementInput.acceleration = 0;
        DOVirtual.Float(0, 1, .6f, ((acceleration)=> movementInput.acceleration = acceleration));
    }

    bool isLastHit()
    {
        if (lockedTarget == null)
            return false;

        return enemyManager.AliveEnemyCount() == 1 && lockedTarget.health <= 1;
    }

    #region Input

    private void OnCounter()
    {
        CounterCheck();
    }

    private void OnAttack()
    {
        AttackCheck();
    }

    #endregion

}
