using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MirrorCamera : MonoBehaviour {

	public Camera MirrorCam;
	public Transform CenterAnchor;
	public Camera PlayerCamera;
	public Transform MirrorCameraPosition;
	public Transform ReflectionTransform;
	public Vector3 MatrixScale = new Vector3 (-1f, 1f, 1f);

	public RenderTexture RenderFromCamera (Camera playerCamera) {
		PlayerCamera = playerCamera;
		CenterAnchor = playerCamera.transform;
		if (MirrorCam.targetTexture == null) {
			MirrorCam.targetTexture = RenderTexture.GetTemporary (2160, 1200, 24);
			MirrorCam.targetTexture.Create ();
		}
		/*else if (MirrorCam.targetTexture.width != Screen.width || MirrorCam.targetTexture.height != Screen.height) {
			RenderTexture.ReleaseTemporary (MirrorCam.targetTexture);
			MirrorCam.targetTexture = RenderTexture.GetTemporary (Screen.width, Screen.height, 24);
			MirrorCam.targetTexture.Create ();
		}*/
		MirrorCam.Render ();
		return MirrorCam.targetTexture;
	}

	void OnPreRender () {
		if (MirrorCam.targetTexture == null)
			return;

		GL.invertCulling = true;

		ReflectionTransform.localPosition = Vector3.zero;
		ReflectionTransform.localRotation = Quaternion.identity;
		MirrorCameraPosition.position = CenterAnchor.position;
		MirrorCameraPosition.rotation = CenterAnchor.rotation;
		ReflectionTransform.localEulerAngles = new Vector3 (0f, 180f, 0f);

		Vector3 centerAnchorPosition = MirrorCameraPosition.localPosition;
		centerAnchorPosition.x *= -1;
		Vector3 centerAnchorRotation = -MirrorCameraPosition.localEulerAngles;
		centerAnchorRotation.x *= -1;
		MirrorCameraPosition.localPosition = centerAnchorPosition;
		MirrorCameraPosition.localEulerAngles = centerAnchorRotation;

		MirrorCam.fieldOfView = PlayerCamera.fieldOfView;
		MirrorCam.ResetWorldToCameraMatrix ();
		MirrorCam.ResetProjectionMatrix ();
		MirrorCam.projectionMatrix = PlayerCamera.projectionMatrix * Matrix4x4.Scale (MatrixScale);
	}

	void OnPostRender () {
		GL.invertCulling = false;
	}
}
