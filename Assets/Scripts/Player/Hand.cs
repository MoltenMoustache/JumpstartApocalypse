using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : XRDirectInteractor
{
    [SerializeField] SkinnedMeshRenderer handRenderer;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        onSelectEnter.AddListener(Grab);
        onSelectExit.AddListener(Drop);
    }

    private void OnDestroy()
    {
        onSelectEnter.RemoveListener(Grab);
        onSelectExit.RemoveListener(Drop);
    }

    protected virtual void Grab(XRBaseInteractable a_interactable)
    {
        SetHandVisibility(false);
    }

    protected virtual void Drop(XRBaseInteractable a_interactable)
    {
        SetHandVisibility(true);
    }

    // Disables the hand's renderer when an object is grabbed, this is inducing 'Tomato Presence'.
    void SetHandVisibility(bool a_isVisible)
    {
        handRenderer.enabled = a_isVisible;
    }
}
