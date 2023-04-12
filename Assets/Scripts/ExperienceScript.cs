
using System;
using UnityEngine;

public class ExperienceScript : MonoBehaviour
{
    public float parentDifficulty = 1f;
    private ShrineManager shrines;
    [SerializeField] private float baseExpPoints = 15f;
    [SerializeField] private float baseShrinePoints = 0.2f; // 5 easy kills to get an upgrade
    private GameObject player;
    private CapsuleCollider playerCollider;
    private Mesh mesh;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        shrines = GameObject.FindWithTag("ShrineManager").GetComponent<ShrineManager>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= mesh.bounds.size.x/2f + playerCollider.radius)
        {
            Debug.Log("EXP PICKUP");
            shrines.upgradePoints += baseShrinePoints*parentDifficulty;
            // NotImplemented experienceCounter += baseExpPoints * parentDifficulty
            Destroy(gameObject);
        }
    }
}
