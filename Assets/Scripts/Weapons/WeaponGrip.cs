using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponGrip : Handhold
{
    Firearm weapon = null;

    protected override void Awake()
    {
        base.Awake();
        onActivate.AddListener(Activate);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onActivate.RemoveListener(Activate);
    }

    protected override void Grab(XRBaseInteractor a_interactor)
    {
        weapon.Grip(a_interactor);
        Debug.Log("WeaponGrip.Grab()");
    }

    protected override void Drop(XRBaseInteractor a_interactor)
    {
        weapon.ClearGrip(a_interactor);
    }

    protected void Activate(XRBaseInteractor a_interactor)
    {
        weapon.Fire(a_interactor);
    }

    protected void Deactivate(XRBaseInteractor a_interactor)
    {

    }

    public void SetWeapon(Firearm a_weapon)
    {
        weapon = a_weapon;
    }
}
