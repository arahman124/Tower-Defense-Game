using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Enemy : MonoBehaviour
{
    //variable to hold speed of monsters
    private float m_speed = 2f;

    //Variable for the position that the enemy moves towards
    private Transform m_target;
    [SerializeField] private float m_health;

    [SerializeField] private Transform m_healthBarLocation;

    public float Health
    {
        get => m_health;
        set
        {
            float subValue = m_health - value;
            float mult20 = subValue / 20;

            Debug.Log($"MULT20: {mult20}");

            for (int i = 0; i < (int)mult20; ++i)
            {
                Debug.Log("LOSING A HEART!");
                LoseHeart();
            }

            m_health = value;
        }
    }

    private VisualElement m_healthBar;
    private VisualElement[] m_hearts;

    private Camera m_mainCamera;

    void Start()
    {
        m_mainCamera = Camera.main;
        //Queries
        m_healthBar = GetComponent<UIDocument>().rootVisualElement.Q("Container");
        m_hearts = m_healthBar.Children().ToArray();

        SetHealthBarPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //Transforms/changes position of the enemy such that it moves towards the given position
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        SetHealthBarPosition();
    }

    //Method for setting the target that the enemy moves towards
    public void SetTarget(Transform target)
    {
        //sets the target to move towards as the given target from the spawner script
        m_target = target;

    }

    private void SetHealthBarPosition()
    {
        Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(m_healthBar.panel, m_healthBarLocation.position, m_mainCamera);
        m_healthBar.transform.position = new Vector2(newPosition.x - m_healthBar.layout.width / 2, newPosition.y);
    }

    private void LoseHeart()
    {
        // A LINQ expression to get the next visible heart from the healthbar
        VisualElement nextHeart = m_hearts.Where(x => x.visible).LastOrDefault();

        nextHeart.style.visibility = Visibility.Hidden;
    }


    // A separate class that adds some buttons to the enemy in the inspector window
    [CustomEditor(typeof(Enemy))]
    class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("DAMAGE ENEMY"))
            {
                ((Enemy)target).Health -= 20f;
            }
        }
    }
}
