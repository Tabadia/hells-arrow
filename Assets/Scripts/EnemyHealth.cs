using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = System.Random;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float health = 20f;
    [SerializeField] public float difficulty = 1f; // 1 for basic enemy, 3(?) for bosses
    [SerializeField] private GameObject enemyDrop;
    [SerializeField] private Animator hitEffect;
    [SerializeField] private bool isBoss;
    [SerializeField] private bool isGod;
    
    private GameObject[] portals;
    private Slider healthBar;
    private Random rand;
    private Camera mainCamera;
    private Hearts playerHearts;

    void Start() {
        if (isBoss){
            portals = GameObject.FindGameObjectsWithTag("Portal");
            print(portals[0]);
            if(portals[0] != null){
                print("portal found");
            }
            else{
                print("portal not found");
            }
            portals[0].SetActive(false);
        }
        rand = new Random();
        healthBar = GetComponentInChildren<Slider>();
        health = maxHealth;
        healthBar.value = CalculateHealth();
        mainCamera = Camera.main;
    }

    void Update() {
        playerHearts = GameObject.FindWithTag("Player").GetComponent<Hearts>();
        healthBar.value = CalculateHealth();
        if (health <= 0)
        {
            Physics.Raycast(new Ray(transform.position, Vector3.down), out var hit, 10f, LayerMask.GetMask("Ground"));
            GameObject instantiatedDrop = Instantiate(enemyDrop, hit.point+(new Vector3(0,0.3f,0)), transform.rotation);
            instantiatedDrop.GetComponentInChildren<ExperienceScript>().parentDifficulty = difficulty;
            if(isBoss){
                print("is boss");
                portals[0].SetActive(true);
            }
            if (isGod){
                print("running gamended function");
                playerHearts.GameEnded();
            }
            Destroy(transform.gameObject);
        }
        if (health > maxHealth){
            health = maxHealth;
        }
 
        healthBar.transform.LookAt(healthBar.transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
    }
    
    public void TakeDamage(float bowStrength) {
       
        health -= bowStrength;
        gameObject.transform.GetChild(1).GetComponent<Animator>().Play("hurt", 0);
        hitEffect.Play("hit" + rand.Next(1,3), 0);
    }

    public void TakeDamage(float dmg, bool isFlaming){
        if(isFlaming){
            health -= dmg;
        }
    }

    private float CalculateHealth() {
        return health / maxHealth;
    }

}
