using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//Makes sure each object with this script attached has the UI and animation elements attached
[RequireComponent(typeof(UIDocument), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Goblin, Skeleton, Boss
    }

    //variable to hold speed of monsters
    [SerializeField]private float m_speed = 2f;

    //Variable for the position that the enemy moves towards - references Tower to access tower script
    private Tower m_target;
    //Holds the initial health of the monster
    [SerializeField] private float m_maxHealth;
    //Holds the changing health (current) of the monster
    [SerializeField] private float m_currentHealth;
    [SerializeField] private float m_dps;

    //Variables for the amount of gold and points that each monster is worth
    public int GoldToAward;
    public int PointsToAward;

    //Holds the position of the health bar
    [SerializeField] private Transform m_healthBarLocation;

    //Holds cooldown for animations
    [SerializeReference] private float m_cooldownDuration;
    //Holds the variable for the timer of the cooldown
    private float m_cooldownTimer;

    //Reference to player
    private Player m_player;

    //Holds the variable for health
    public float Health
    {
        //grabs the current health of the monster
        get => m_currentHealth;
        set
        {
            m_currentHealth = value;

            //Sets state of animation to hurt
            State = EnemyState.Hurt;

            if (m_currentHealth <= 0)
            {
                //Sets state of animation to dead
                State = EnemyState.Dead;

                //Disables the box collider
                GetComponent<BoxCollider2D>().enabled = false;
                //Destroys the game object after 1.5 seconds
                StartCoroutine("Die");


                // Award the player points and gold
                m_player.AddGold(GoldToAward);
                m_player.AddPoints(PointsToAward);
            }

            //Holds the total number of hearts that the monster has
            int currentlyVisibleHearts = m_hearts.Count(x => x.visible);

            //Calculates the percentage of health
            float percentageHealth = m_currentHealth / m_maxHealth * 100f;

            //Holds the next threshold for the next heart to be lost
            int heartThreshold = (currentlyVisibleHearts - 1) * 20;

            if (heartThreshold >= percentageHealth)
            {
                int amountOfHeartsToLose = currentlyVisibleHearts - (RoundUp((int)percentageHealth, 20) / 20);
                for (int i = 0; i < amountOfHeartsToLose; i++)
                {
                    LoseHeart();
                }
            }
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }

   
    public void Reset()
    {
        //m_currentHealth = health;
        gameObject.SetActive(true);
    }


    //Variables for the on-screen display
    private VisualElement m_healthBar;
    private VisualElement[] m_hearts;

    //Reference to the main camera as a variable
    private Camera m_mainCamera;

    //Variable for access of animations
    private Animator m_animator;

    //Different states of animation
    public enum EnemyState
    {
        Walking,
        Idle,
        Dead,
        Attack,
        Hurt
    }

    //Used to access a state from enum and store as variable
    private EnemyState m_state;

    //Boolean variable to tell if a monster has reached the tower or not
    private bool m_atTower = false;

    // We're using a property for the State so that we are able to trigger events when we set the value
    public EnemyState State
    {
        get => m_state;

        set
        {
            ResetAnimationTriggers();
            switch (value)
            {
                case EnemyState.Walking:
                    m_animator.SetTrigger(Constants.MONSTER_WALK);
                    break;
                case EnemyState.Idle:
                    m_animator.SetTrigger(Constants.MONSTER_IDLE);
                    break;
                case EnemyState.Dead:
                    m_animator.SetTrigger(Constants.MONSTER_DEAD);
                    break;
                case EnemyState.Hurt:
                    m_animator.SetTrigger(Constants.MONSTER_HIT);
                    m_cooldownTimer = m_cooldownDuration;
                    break;
                case EnemyState.Attack:
                    m_animator.SetTrigger(Constants.MONSTER_ATTACK);
                    m_cooldownTimer = m_cooldownDuration;
                    break;
            }
            m_state = value;
        }
    }

    // TODO - Wrap every animator.SetTrigger in a new function that calls ResetAnimationTriggers before setting the trigger
    private void ResetAnimationTriggers()
    {
        foreach (AnimatorControllerParameter param in m_animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                m_animator.ResetTrigger(param.name);
            }
        }
    }

    void Start()
    {
        //Animator component for animation manipulation
        m_animator = GetComponent<Animator>();

        //Variable for the main camera
        m_mainCamera = Camera.main;
        //Queries
        m_healthBar = GetComponent<UIDocument>().rootVisualElement.Q("Container");
        m_hearts = m_healthBar.Children().ToArray();

        

        m_currentHealth = m_maxHealth;
        //Starts walking animation
        State = EnemyState.Walking;
        //Direction facing - negative is right side and positive is left side
        Vector2 walkDirection = m_target.transform.position - transform.position;

        //If the monster is facing the wrong way (spawns on the right) then it flips the sprite
        if (walkDirection.x < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        
        SetHealthBarPosition();

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case EnemyState.Walking:
                //Transforms/changes position of the enemy such that it moves towards the given position
                transform.position = Vector2.MoveTowards(transform.position, m_target.transform.position, m_speed * Time.deltaTime);
                break;

            case EnemyState.Idle:
                //Cooldown counts down
                m_cooldownTimer -= Time.deltaTime;

                //Starts attack animation
                if (m_atTower && m_cooldownTimer <= 0f)
                {
                    State = EnemyState.Attack;
                }
                break;
            case EnemyState.Hurt:
                //Timer counts down
                m_cooldownTimer -= Time.deltaTime;

                //Once cooldown is done, starts a different animation depending on where they are
                if (m_cooldownTimer <= 0f)
                {
                    if (m_atTower)
                    {
                        State = EnemyState.Idle;
                    }
                    else
                    {
                        State = EnemyState.Walking;
                    }
                }
                break;
        }

    }

    //Sets a reference to the player to be used in script
    public void SetPlayerRef(Player player)
    {
        m_player = player;
    }

    //When the hurt animation is done, it goes back to idle animation
    public void OnHurtAnimationFinished()
    {
        m_animator.SetTrigger(Constants.MONSTER_IDLE);
    }

    //Takes damage away from tower
    public void OnAttackTower()
    {
        m_target.TakeDamage(m_dps);

        // TODO: Play SFX
    }
    
    //Once attack animation is over, goes back to the idle animation
    public void OnAttackFinished()
    {
        State = EnemyState.Idle;
    }

    //Late update is called after the update function has finished at each frame
    private void LateUpdate()
    {
        //Updates position of health bar with monster moving
        SetHealthBarPosition();
    }

    //Method for setting the target that the enemy moves towards
    public void SetTarget(Tower target)
    {
        //sets the target to move towards as the given target from the spawner script
        m_target = target;

    }
    //Finds the current location of the health bar and updates the new location as the monster moves
    private void SetHealthBarPosition()
    {
        Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(m_healthBar.panel, m_healthBarLocation.position, m_mainCamera);
        m_healthBar.transform.position = new Vector2(newPosition.x - m_healthBar.layout.width / 2, newPosition.y);
    }

    //Takes away a heart from the monster health
    private void LoseHeart()
    {
        // A LINQ expression to get the next visible heart from the healthbar
        VisualElement nextHeart = m_hearts.Where(x => x.visible).LastOrDefault();

        //Sets the next heart as invisible
        nextHeart.style.visibility = Visibility.Hidden;
    }

    //Checks if monster has reached the tower
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TOWER_TAG))
        {
            Debug.Log("AT THE TOWER");
            State = EnemyState.Attack;
            m_atTower = true;
        }
    }


    // A separate class that adds some buttons to the enemy in the inspector window
    [CustomEditor(typeof(Enemy))]
    class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //If the button is pressed in the inspector, damage is dealt to the enemy that was hit
            if (GUILayout.Button("DAMAGE ENEMY"))
            {
                ((Enemy)target).Health -= 20f;
            }
        }
    }

    //Rounds a number to the nearest multiple of given parameters
    private int RoundUp(int numToRound, int multiple)
    {
        // Code taken from https://stackoverflow.com/questions/3407012/rounding-up-to-the-nearest-multiple-of-a-number
        if (multiple == 0)
        {
            return numToRound;
        }

        int remainder = numToRound % multiple;

        if (remainder == 0)
        {
            return numToRound;
        }

        return numToRound + multiple - remainder;
    }
}
