using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AmmoBag ammoBag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Locomotion
    #endregion Locomotion

    #region Holstering
    #endregion Holstering

    #region Firearms
    public void SetActiveFirearm(Firearm a_firearm)
    {
        ammoBag.SetActiveAmmoType(a_firearm.ammoStats);
    }
    #endregion Firearms
}
