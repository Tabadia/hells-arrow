using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float health = 20f;
    [SerializeField] public float difficulty = 1f; // 1 for basic enemy, 3(?) for bosses
    [SerializeField] private GameObject enemyDrop;
    [SerializeField] private Animator hitEffect;
    [SerializeField] private bool isBoss;
    
    private GameObject portal;
    private Slider healthBar;
    private Random rand;

    void Start() {
        if (isBoss){
            portal = GameObject.FindGameObjectWithTag("Portal");
        }
        rand = new Random();
        healthBar = GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    void Update() {
        healthBar.value = CalculateHealth();
        if (health <= 0){
            GameObject instantiatedDrop = Instantiate(enemyDrop, transform.position+(new Vector3(0,0.5f,0)), transform.rotation);
            instantiatedDrop.GetComponentInChildren<ExperienceScript>().parentDifficulty = difficulty;
            if(isBoss){
                portal.SetActive(true);
            }
            Destroy(gameObject);
        }
        if (health > maxHealth){
            health = maxHealth;
        }

        Camera camera = Camera.main;
 
        healthBar.transform.LookAt(healthBar.transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }
    
    public void takeDamage(float bowStrength) {
        health -= bowStrength;
        gameObject.transform.GetChild(1).GetComponent<Animator>().Play("hurt", 0);
        hitEffect.Play("hit" + rand.Next(1,3), 0);
    }

    public void takeDamage(float dmg, bool isFlaming){
        if(isFlaming){
            health -= dmg;
        }
    }

    private float CalculateHealth() {
        return health / maxHealth;
    }
}
