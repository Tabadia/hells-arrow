using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShrineManager : MonoBehaviour {
    [SerializeField] private Button[] upOptions;
    [SerializeField] private TextMeshProUGUI[] optionText;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private GameObject shrineText;
    [SerializeField] private GameObject upMenu;
    [SerializeField] private GameObject sphere;
    [SerializeField] public float upgradePoints;
    [SerializeField] private GameObject upgrade;
    [SerializeField] private Animator upgradeAnimator;
    [SerializeField] private TMP_Text soulText;
    [SerializeField] private Animator soulAnim;

    private GameObject[] shrines;
    private string chosenUpgrade = "";
    private GameObject closest;
    private GameObject player;
    private MovementController movementScript;
    private Detection detection;
    private float pastPoints = 0f;
    
    public string[,] upgrades = {{"Exploding", "0"}, {"Multishot", "0"}, {"Piercing", "0"}, {"Flaming", "0"}, {"Arrow Speed", "0"}, {"Movement Speed", "0"}, {"Damage", "0"}, {"Decay Tolerance", "0"}}; 
    // {"Upgrade Name", "Upgrade Level"}
    public bool inMenu;

    // ShootingScript shootingScript;
    // Shrines shrineScript;

    void Start() {
        shrines = GameObject.FindGameObjectsWithTag("Shrine");
        // shootingScript = GetComponent<ShootingScript>();
        // shrineScript = GetComponent<Shrines>();
        player = GameObject.FindGameObjectWithTag("Player");
        movementScript = player.GetComponent<MovementController>();
        detection = sphere.GetComponent<Detection>();
        pastPoints = upgradePoints;
    }

    void Update() {
        if (upgradePoints != pastPoints){
            soulAnim.Play("acquire");
        }
        pastPoints = upgradePoints;
        soulText.text = upgradePoints.ToString("N0");
        float distance = Mathf.Infinity;
        // Vector3 position = transform.position;
        foreach (GameObject s in shrines) {
            float curDistance = s.GetComponent<Shrines>().distance;
            if (curDistance < distance) {
                closest = s;
                distance = curDistance;
            }
        }
        if (distance < maxDistance)
        {
            var closestScript = closest.GetComponent<Shrines>();
            if (Input.GetKeyDown(KeyCode.E)) {
                if (upgradePoints >= 1f){
                    Time.timeScale = 0;
                    AudioListener.pause = true;
                    for (int i = 0; i < upgrades.GetLength(0); i++) {
                        for (int j = 0; j < closestScript.upgrades.GetLength(0); j++) {
                            if (upgrades[i, 0] == closestScript.upgrades[j, 0]) {
                                closestScript.upgrades[j, 1] = upgrades[i, 1];
                            }
                        }
                    }
                    for (int i = 0; i < upOptions.Length; i++) {
                        optionText[i].text = closestScript.upgrades[i,0] + " " + (int.Parse(closestScript.upgrades[i,1]) + 1);
                    }
                    inMenu = true;
                    upMenu.SetActive(true);
                }
                else {
                    StartCoroutine(ChangeText());
                } 
            }
            shrineText.SetActive(!inMenu);
        }
        else {
            shrineText.SetActive(false);
        }
    }

    public void UpgradeClicked() {
        upgradeAnimator.Play("upgrade");
        chosenUpgrade = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        if (upgradePoints >= 1f) {
            for (int i = 0; i < upgrades.GetLength(0); i++) {
                if ((upgrades[i, 0] + " " + (int.Parse(upgrades[i, 1]) + 1)) == chosenUpgrade) {
                    upgrades[i, 1] = (int.Parse(upgrades[i, 1]) + 1).ToString(); 
                    if (upgrades[i, 0] == "Movement Speed"){
                        movementScript.moveSpeed += 1f;
                        movementScript.maxSpeed += 1f;
                    }
                    if (upgrades[i, 0] == "Decay Tolerance"){
                        detection.damageInterval += 1f;
                    }
                }
            }

            inMenu = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
            upMenu.SetActive(false);
            upgradePoints -= 1f;
        }
    }

    IEnumerator ChangeText(){
        shrineText.GetComponent<TMP_Text>().text = "You need upgrade points!";
        yield return new WaitForSeconds(2f);
        shrineText.GetComponent<TMP_Text>().text = "Press E to interact with shrine";
    }
}
