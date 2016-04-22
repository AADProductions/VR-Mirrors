using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Mirror : MonoBehaviour {

	public MirrorCamera portalCamera;
    private Material _portalMaterial;

    private void Awake() {
        _portalMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnWillRenderObject() {
        portalCamera.RenderIntoMaterial(_portalMaterial);
    }

}
