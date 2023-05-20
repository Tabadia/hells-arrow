using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float damage = .5f;
    
    private GameObject player;
    private Hearts playerHearts;
    private Parry playerParry;
    // private CapsuleCollider playerCollider;
    private Vector3 prevPos;

    void Start() {
        player = GameObject.FindWithTag("Player");
        playerHearts = player.GetComponent<Hearts>();
        playerParry = player.GetComponent<Parry>();
        // playerCollider = player.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        prevPos = transform.position;
        transform.position += Time.deltaTime * speed * transform.forward;

        RaycastHit[] hits = new RaycastHit[10];
        var hitNum = Physics.RaycastNonAlloc(new Ray(prevPos, (transform.position - prevPos).normalized), hits, (transform.position - prevPos).magnitude);

        for (int i = 0; i < hitNum; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))
            {
                if (!playerParry.isParrying)
                {
                    playerHearts.TakeDamage(damage);
                }
                else{
                    print("parried");
                    blockSFX.Play();
                    Destroy(gameObject);
                }
            }
            if (hits[i].collider.gameObject.CompareTag("Arrow")){
                Destroy(gameObject);
            }
        }
    }
}
