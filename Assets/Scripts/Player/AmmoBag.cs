using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class AmmoBag : XRBaseInteractable
{
    [Header("Ammo Bag")]
    FirearmAmmo activeAmmo;
    Dictionary<FirearmType, int> ammoCountDictionary = new Dictionary<FirearmType, int>();
    [SerializeField] bool infiniteAmmo = false;
    [SerializeField] TextMeshProUGUI ammoCounter = null;

    protected override void Awake()
    {
        base.Awake();
        onSelectEnter.AddListener(SpawnAmmo);
        AddAmmo(FirearmType.PISTOL, 1000);
    }

    private void OnDestroy()
    {
        onSelectEnter.RemoveListener(SpawnAmmo);
    }

    public void SetActiveAmmoType(FirearmAmmo a_activeAmmo)
    {
        activeAmmo = a_activeAmmo;
    }

    void UpdateAmmoCounter()
    {
        if (activeAmmo != null)
            ammoCounter.text = ammoCountDictionary[activeAmmo.ammoType].ToString();
    }

    public void SpawnAmmo(XRBaseInteractor a_interactor)
    {
        if (infiniteAmmo)
        {
            MagazineObject mag = Instantiate(activeAmmo.magazinePrefab, transform.position, Quaternion.identity).GetComponent<MagazineObject>();
            mag.StoredAmmo = activeAmmo.magazineCapacity;
            interactionManager.ForceSelectObject(a_interactor, mag);
        }

        // If the player has ammo of the active ammo type
        else if (ammoCountDictionary.ContainsKey(activeAmmo.ammoType))
        {
            if (ammoCountDictionary[activeAmmo.ammoType] > 0)
            {
                MagazineObject mag = Instantiate(activeAmmo.magazinePrefab, transform.position, Quaternion.identity).GetComponent<MagazineObject>();

                // If the player has enough ammo for a mag, spawn a mag and decrement the stored ammo.
                if (ammoCountDictionary[activeAmmo.ammoType] >= activeAmmo.magazineCapacity)
                {
                    mag.StoredAmmo = activeAmmo.magazineCapacity;
                    ammoCountDictionary[activeAmmo.ammoType] -= activeAmmo.magazineCapacity;
                }
                // If the player has ammo of the type but not enough for a full mag, spawn a mag containing that ammo.
                else
                {
                    mag.StoredAmmo = ammoCountDictionary[activeAmmo.ammoType];
                    ammoCountDictionary[activeAmmo.ammoType] = 0;
                }

                UpdateAmmoCounter();
                interactionManager.ForceSelectObject(a_interactor, mag);
            }
        }
        else
        {
            // TODO: Out of ammo indicator
            Debug.Log("Out of Ammo!");
        }
    }

    public void AddAmmo(MagazineObject a_mag)
    {
        if (ammoCountDictionary.ContainsKey(a_mag.ammoType))
            ammoCountDictionary[a_mag.ammoType] += a_mag.StoredAmmo;
        else
            ammoCountDictionary.Add(a_mag.ammoType, a_mag.StoredAmmo);

        a_mag.QueueDestroy();
        UpdateAmmoCounter();
    }

    public void AddAmmo(FirearmType a_ammoType, int a_amount)
    {
        if (ammoCountDictionary.ContainsKey(a_ammoType))
            ammoCountDictionary[a_ammoType] += a_amount;
        else
            ammoCountDictionary.Add(a_ammoType, a_amount);

        UpdateAmmoCounter();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MagazineObject>() != null)
        {
            MagazineObject magObj = other.GetComponent<MagazineObject>();
            magObj.IsInAmmoBag(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MagazineObject>() != null)
        {
            MagazineObject magObj = other.GetComponent<MagazineObject>();
            magObj.IsInAmmoBag(null);
        }
    }
}
