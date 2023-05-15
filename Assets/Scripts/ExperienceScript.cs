
using UnityEngine;

public class ExperienceScript : MonoBehaviour
{
    public float parentDifficulty = 1f;
    private ShrineManager shrines;
    private PauseManager pauseManager;
    private GameObject UI;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float baseScorePoints = 15f;
    [SerializeField] private float baseShrinePoints = 0.2f; // 5 easy kills to get an upgrade

    void Start()
    {
        shrines = GameObject.FindWithTag("ShrineManager").GetComponent<ShrineManager>();
        UI = GameObject.Find("UI");
        pauseManager = UI.GetComponentInChildren<PauseManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shrines.upgradePoints += baseShrinePoints * parentDifficulty;
            pauseManager.playerScore += baseScorePoints * parentDifficulty;
            Destroy(parentObject);
        }
    }
}
