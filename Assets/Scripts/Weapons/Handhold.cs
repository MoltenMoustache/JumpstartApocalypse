using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Handhold : XRBaseInteractable
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        onSelectEnter.AddListener(Grab);
        onSelectExit.AddListener(Drop);
    }

    protected virtual void OnDestroy()
    {
        onSelectEnter.RemoveListener(Grab);
        onSelectExit.RemoveListener(Drop);
    }

    protected virtual void Grab(XRBaseInteractor a_interactor)
    {

    }

    protected virtual void Drop(XRBaseInteractor a_interactor)
    {

    }
}
