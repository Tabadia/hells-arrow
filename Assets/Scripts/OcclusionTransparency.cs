using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OcclusionTransparency : MonoBehaviour
{
    public Transform cam;
    public Material transparentMaterial;
    
    public List<GameObject> currentHitObjects = new();
    public GameObject[] pastHitObjects = new GameObject[0];
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, -(transform.position - cam.position));
    
        foreach (RaycastHit obj in hitObjects)
        {
            if (!currentHitObjects.Contains(obj.transform.gameObject)) 
                currentHitObjects.Add(obj.transform.gameObject);
        }
    
        pastHitObjects = currentHitObjects.ToArray();
        //print(currentHitObjects.Count);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -(transform.position - cam.position));
    }
}
