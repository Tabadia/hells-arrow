using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour{

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Projectile"){
            Destroy(other.gameObject);
        }
    }
}
