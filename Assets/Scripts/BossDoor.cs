using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField, Range(1, 2)] private int currentDoorType;
    [SerializeField] private GameObject primaryDoor;
    [SerializeField] private GameObject secondaryDoor;

    public bool primaryDoorPassed;
    public bool doorLocked;

    private BossDoor pScript;
    private BossDoor sScript;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        pScript = primaryDoor.GetComponent<BossDoor>();
        sScript = secondaryDoor.GetComponent<BossDoor>();
        spriteRenderer = primaryDoor.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) {return;}
        if (currentDoorType == 1)
        {
            pScript.primaryDoorPassed = true;
            sScript.primaryDoorPassed = true;
        }
        else if (currentDoorType == 2 && pScript.primaryDoorPassed)
        {
            pScript.DisableMovement();
            sScript.DisableMovement();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) {return;}

        if (currentDoorType == 1 && pScript.doorLocked)
        {
            pScript.EnableMovement();
            sScript.EnableMovement();
        }
    }

    public void EnableMovement()
    {
        if (!pScript.doorLocked) {return;}

        spriteRenderer.color = Color.white;
        switch (currentDoorType)
        {
            case 1:
                primaryDoor.GetComponent<BoxCollider>().isTrigger = true;
                pScript.doorLocked = false;
                break;
            case 2:
                secondaryDoor.GetComponent<BoxCollider>().isTrigger = true;
                sScript.doorLocked = false;
                break;
        }
    }

    public void DisableMovement()
    {
        if (doorLocked) {return;}

        spriteRenderer.color = Color.red;
        switch (currentDoorType)
        {
            case 1:
                primaryDoor.GetComponent<BoxCollider>().isTrigger = false;
                pScript.doorLocked = true;
                pScript.primaryDoorPassed = false;
                break;
            case 2:
                secondaryDoor.GetComponent<BoxCollider>().isTrigger = false;
                sScript.doorLocked = true;
                sScript.primaryDoorPassed = false;
                break;
        }
    }
}