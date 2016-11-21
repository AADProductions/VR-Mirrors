using UnityEngine;
using System.Collections;

public class MirrorCamera : MonoBehaviour {
	
	public Transform CameraRig;
	public Camera PlayerCamera;
	public Transform MirrorCameraParent;
	public Transform ReflectionTransform;
	public int TextureSize = 1024;
	public Camera VrEye;
	public GameObject VrRig;
	public bool RenderAsMirror = false;
	Camera cameraForPortal;
	RenderTexture leftEyeRenderTexture;
	RenderTexture rightEyeRenderTexture;
	Vector3 mirrorMatrixScale = new Vector3 (-1f, 1f, 1f);
	Vector3 reflectionRotation = new Vector3 (0f, 180f, 0f);
	Vector3 eyeOffset;

	protected void Awake() {
		cameraForPortal = GetComponent<Camera>();
		cameraForPortal.enabled = false;

		if (RenderAsMirror) {
			//create render textures based on the screen size so reflections look realistic
			//TODO create / release temporary render textures
			leftEyeRenderTexture = new RenderTexture ((int)SteamVR.instance.sceneWidth, (int)SteamVR.instance.sceneHeight, 24);
			rightEyeRenderTexture = new RenderTexture ((int)SteamVR.instance.sceneWidth, (int)SteamVR.instance.sceneHeight, 24);
        } else {
			leftEyeRenderTexture = new RenderTexture (TextureSize, TextureSize, 24);
			rightEyeRenderTexture = new RenderTexture (TextureSize, TextureSize, 24);
		}

		int aa = QualitySettings.antiAliasing == 0 ? 1 : QualitySettings.antiAliasing;
		leftEyeRenderTexture.antiAliasing = aa;
		rightEyeRenderTexture.antiAliasing = aa;
	}

	protected Matrix4x4 HMDMatrix4x4ToMatrix4x4(Valve.VR.HmdMatrix44_t input) {
		var m = Matrix4x4.identity;

		m[0, 0] = input.m0;
		m[0, 1] = input.m1;
		m[0, 2] = input.m2;
		m[0, 3] = input.m3;

		m[1, 0] = input.m4;
		m[1, 1] = input.m5;
		m[1, 2] = input.m6;
		m[1, 3] = input.m7;

		m[2, 0] = input.m8;
		m[2, 1] = input.m9;
		m[2, 2] = input.m10;
		m[2, 3] = input.m11;

		m[3, 0] = input.m12;
		m[3, 1] = input.m13;
		m[3, 2] = input.m14;
		m[3, 3] = input.m15;

		return m;
	}

	public void RenderIntoMaterial(Material material) {
		if (Camera.current == VrEye) {
			if (RenderAsMirror) {
				ReflectionTransform.localPosition = Vector3.zero;
				ReflectionTransform.localRotation = Quaternion.identity;
				MirrorCameraParent.position = CameraRig.position;
				MirrorCameraParent.rotation = CameraRig.rotation;
				ReflectionTransform.localEulerAngles = reflectionRotation;

				Vector3 centerAnchorPosition = MirrorCameraParent.localPosition;
				centerAnchorPosition.x *= -1;
				Vector3 centerAnchorRotation = -MirrorCameraParent.localEulerAngles;
				centerAnchorRotation.x *= -1;
				MirrorCameraParent.localPosition = centerAnchorPosition;
				MirrorCameraParent.localEulerAngles = centerAnchorRotation;
			}

			transform.localRotation = VrEye.transform.localRotation;

			if (RenderAsMirror) {
				GL.invertCulling = true;
			}
			// left eye
			eyeOffset = SteamVR.instance.eyes [0].pos;
			eyeOffset.z = 0.0f;
			Vector3 worldSpaceEyeOffset = VrEye.transform.TransformVector(eyeOffset);
			transform.localPosition = VrEye.transform.localPosition + VrRig.transform.InverseTransformDirection(worldSpaceEyeOffset);

			if (RenderAsMirror)
			{
				cameraForPortal.projectionMatrix = HMDMatrix4x4ToMatrix4x4 (SteamVR.instance.hmd.GetProjectionMatrix (Valve.VR.EVREye.Eye_Left, VrEye.nearClipPlane, VrEye.farClipPlane, Valve.VR.EGraphicsAPIConvention.API_DirectX)) * Matrix4x4.Scale (mirrorMatrixScale);
			} else {
				cameraForPortal.projectionMatrix = HMDMatrix4x4ToMatrix4x4 (SteamVR.instance.hmd.GetProjectionMatrix (Valve.VR.EVREye.Eye_Left, VrEye.nearClipPlane, VrEye.farClipPlane, Valve.VR.EGraphicsAPIConvention.API_DirectX));
			}

			cameraForPortal.targetTexture = leftEyeRenderTexture;
			cameraForPortal.Render();
			material.SetTexture("_LeftEyeTexture", leftEyeRenderTexture);

			// right eye
			eyeOffset = SteamVR.instance.eyes [1].pos;
			eyeOffset.z = 0.0f;
			worldSpaceEyeOffset = VrEye.transform.TransformVector(eyeOffset);
			transform.localPosition = VrEye.transform.localPosition + VrRig.transform.InverseTransformDirection(worldSpaceEyeOffset);

			if (RenderAsMirror)
			{
				cameraForPortal.projectionMatrix = HMDMatrix4x4ToMatrix4x4 (SteamVR.instance.hmd.GetProjectionMatrix (Valve.VR.EVREye.Eye_Right, VrEye.nearClipPlane, VrEye.farClipPlane, Valve.VR.EGraphicsAPIConvention.API_DirectX)) * Matrix4x4.Scale (mirrorMatrixScale);
			} else {
				cameraForPortal.projectionMatrix = HMDMatrix4x4ToMatrix4x4 (SteamVR.instance.hmd.GetProjectionMatrix (Valve.VR.EVREye.Eye_Right, VrEye.nearClipPlane, VrEye.farClipPlane, Valve.VR.EGraphicsAPIConvention.API_DirectX));
			}

			cameraForPortal.targetTexture = rightEyeRenderTexture;
			cameraForPortal.Render();
			material.SetTexture("_RightEyeTexture", rightEyeRenderTexture);

			if (RenderAsMirror) {
				GL.invertCulling = false;
			}

		}
	}

}
