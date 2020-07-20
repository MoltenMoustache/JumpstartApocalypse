using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineObject : XRGrabInteractable
{
    [Header("Magazine")]
    public FirearmType ammoType;
    public int ammoCapacity;
    int storedAmmo;
    public int StoredAmmo
    {
        get { return storedAmmo; }
        set { storedAmmo = Mathf.Clamp(value, 0, ammoCapacity); }
    }

    XRBaseInteractor interactor;
    AmmoBag ammoBagInRange = null;
    MeshRenderer renderer = null;
    bool isQueuedToDestroy = false;

    protected override void Awake()
    {
        base.Awake();
        ammoType = FirearmType.NULL;
        onSelectExit.AddListener(OnDeselect);
        onSelectEnter.AddListener(RegisterInteractor);
    }

    public void QueueDestroy()
    {
        if (isSelected)
        {
            interactor.attachTransform.DetachChildren();
            interactor.onSelectExit.Invoke(this);
            Unselect(interactor);
            interactionManager.ClearInteractorSelection(interactor);
        }
        interactionManager.UnregisterInteractableWrapper(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        onSelectExit.RemoveListener(OnDeselect);
        onSelectEnter.RemoveListener(RegisterInteractor);
    }

    public void OnDeselect(XRBaseInteractor a_interactor)
    {
        if (ammoBagInRange != null)
            ammoBagInRange.AddAmmo(this);
    }

    public void IsInAmmoBag(AmmoBag a_ammoBag)
    {
        ammoBagInRange = a_ammoBag;
    }

    void RegisterInteractor(XRBaseInteractor a_interactor)
    {
        interactor = a_interactor;
    }

}
