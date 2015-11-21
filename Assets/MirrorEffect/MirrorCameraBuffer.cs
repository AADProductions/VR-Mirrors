using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class MirrorCameraBuffer : MonoBehaviour
{

	public Shader MirrorShader;
	public MirrorCamera MirrorCam;
	private Material mirrorMaterial;
	private Dictionary<Camera,CommandBuffer> cameras = new Dictionary<Camera,CommandBuffer> ();

	// Remove command buffers from all cameras we added into
	private void Cleanup ()
	{
		foreach (var cam in cameras) {
			if (cam.Key) {
				cam.Key.RemoveCommandBuffer (CameraEvent.AfterSkybox, cam.Value);
			}
		}
		cameras.Clear ();
		Object.DestroyImmediate (mirrorMaterial);
	}

	public void OnEnable ()
	{
		Cleanup ();
	}
	
	public void OnDisable ()
	{
		Cleanup ();
	}

	// Whenever any camera will render us, add a command buffer to do the work on it
	public void OnWillRenderObject ()
	{
		var act = gameObject.activeInHierarchy && enabled;
		if (!act) {
			Cleanup ();
			return;
		}
		
		var cam = Camera.current;
		if (!cam || cam == MirrorCam.MirrorCam) {
			return;
		}

		if (!mirrorMaterial) {
			mirrorMaterial = new Material (MirrorShader);
			mirrorMaterial.hideFlags = HideFlags.HideAndDontSave;
		}

		//render the mirror camera - this will return our render texture
		RenderTexture tex =  MirrorCam.RenderFromCamera (cam);
		mirrorMaterial.SetTexture ("_MirrorTexture", tex);
		GetComponent <Renderer> ().material = mirrorMaterial;

		CommandBuffer buf = null;
		// Did we already add the command buffer on this camera? Nothing to do then.
		if (cameras.ContainsKey (cam)) {
			return;
		}
		
		buf = new CommandBuffer ();
		buf.name = "Render mirror";
		cameras [cam] = buf;

		int mirrorTextureID = Shader.PropertyToID ("_MirrorTexture");
		buf.Blit (BuiltinRenderTextureType.CurrentActive, mirrorTextureID);
		cam.AddCommandBuffer (CameraEvent.BeforeImageEffects, buf);
	}	
}
