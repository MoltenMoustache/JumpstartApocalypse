using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagSlot : MonoBehaviour
{
    Firearm firearm = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Magazine")
        {
            firearm.TryReload(other.GetComponent<MagazineObject>());
        }
    }

    public void SetWeapon(Firearm a_firearm)
    {
        firearm = a_firearm;
    }
}
