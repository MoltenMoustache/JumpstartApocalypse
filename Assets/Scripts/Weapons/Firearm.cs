using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class Firearm : XRGrabInteractable
{
    [Header("Firing")]
    [SerializeField] Transform projectileSpawnPos;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 10.0f;
    [SerializeField] float bulletDamage = 10.0f;

    [Header("Ammo")]
    public FirearmAmmo ammoStats;
    bool hasMagazine = true;
    [SerializeField] int currentAmmo = 0;
    [SerializeField] bool canChamber = true;
    [SerializeField] TextMeshProUGUI ammoCounter = null;

    public XRController controller;
    readonly Vector3 gripRotation = new Vector3(-90, 90, 0);

    protected override void Awake()
    {
        base.Awake();
        onSelectEnter.AddListener(SetRotation);
        onSelectEnter.AddListener(Pickup);
        onSelectExit.AddListener(Drop);
        onActivate.AddListener(Fire);
    }

    private void Start()
    {
        SetupGrip();
    }

    private void OnDestroy()
    {
        onSelectEnter.RemoveListener(SetRotation);
        onSelectEnter.RemoveListener(Pickup);
        onSelectExit.RemoveListener(Drop);
        onActivate.RemoveListener(Fire);
    }


    private void Update()
    {
        bool isPressed = false;
        controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);
        if (isPressed)
            EjectMagazine();
    }

    void SetupGrip()
    {
        // TODO: Rename function
        Transform gripTransform = transform.Find("Grip");
        gripTransform.GetComponent<WeaponGrip>().SetWeapon(this);

        Transform magSlotTransform = transform.Find("MagSlot");
        magSlotTransform.GetComponent<MagSlot>().SetWeapon(this);

    }

    public void Fire(XRBaseInteractor a_interactor)
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateAmmoCounter();

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(projectileSpawnPos.forward * projectileForce, ForceMode.VelocityChange);
        }
        else
        {
            // TODO: CLICK
        }
    }

    public void EjectMagazine()
    {
        if (!hasMagazine)
            return;

        // TODO: Alter gun appearance to reflect unloaded mag


        // Spawns a magazine and sets the mags stored ammo and capacity, if the gun can chamber a round then the stored ammo does not count the chambered round
        // TODO: Eject magazine from eject transform, add slight force based on ejection direction
        MagazineObject magazine = Instantiate(ammoStats.magazinePrefab, transform.position, Quaternion.identity).GetComponent<MagazineObject>();
        magazine.ammoCapacity = ammoStats.magazineCapacity;
        magazine.StoredAmmo = canChamber ? currentAmmo-- : currentAmmo;
        magazine.ammoType = ammoStats.ammoType;
        hasMagazine = false;

        // Empties gun, if a round is chambered then the current ammo contains only that chambered round.
        if (currentAmmo > 0)
            currentAmmo = canChamber ? 1 : 0;
        else
            currentAmmo = 0;

        UpdateAmmoCounter();
    }

    public bool TryReload(MagazineObject a_mag)
    {
        if (hasMagazine)
            return false;

        currentAmmo += a_mag.StoredAmmo;
        hasMagazine = true;
        UpdateAmmoCounter();
        a_mag.QueueDestroy();

        // TODO: Alter gun appearance to reflect loaded mag

        return true;
    }

    void UpdateAmmoCounter()
    {
        ammoCounter.text = currentAmmo + "/" + ammoStats.magazineCapacity;
    }

    public void Pickup(XRBaseInteractor a_interactor)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetActiveFirearm(this);
        ammoCounter.enabled = true;
        UpdateAmmoCounter();
    }

    public void Drop(XRBaseInteractor a_interactor)
    {
        ammoCounter.enabled = false;
    }

    void SetRotation(XRBaseInteractor a_interactor)
    {
        Quaternion rotation = Quaternion.Euler(gripRotation);
        a_interactor.attachTransform.localRotation = rotation;
    }

    public void Grip(XRBaseInteractor a_interactor)
    {
        OnSelectEnter(a_interactor);
        Debug.Log("Firearm.Grip()");
    }

    public void ClearGrip(XRBaseInteractor a_interactor)
    {
        OnSelectExit(a_interactor);
    }
}

public enum FirearmType
{
    RIFLE,
    SHOTGUN,
    PISTOL,
    HEAVY,
    NULL,
}

[System.Serializable]
public class FirearmAmmo
{
    public FirearmType ammoType;
    public int magazineCapacity;
    public GameObject magazinePrefab;
}
