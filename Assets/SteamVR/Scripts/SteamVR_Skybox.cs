//========= Copyright 2015, Valve Corporation, All rights reserved. ===========
//
// Purpose: Sets cubemap to use in the compositor.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

public class SteamVR_Skybox : MonoBehaviour
{
	// Note: Unity's Left and Right Skybox shader variables are switched.
	public Texture front, back, left, right, top, bottom;

	public void SetTextureByIndex(int i, Texture t)
	{
		switch (i)
		{
			case 0:
				front = t;
				break;
			case 1:
				back = t;
				break;
			case 2:
				left = t;
				break;
			case 3:
				right = t;
				break;
			case 4:
				top = t;
				break;
			case 5:
				bottom = t;
				break;
		}
	}

	public Texture GetTextureByIndex(int i)
	{
		switch (i)
		{
			case 0:
				return front;
			case 1:
				return back;
			case 2:
				return left;
			case 3:
				return right;
			case 4:
				return top;
			case 5:
				return bottom;
		}
		return null;
	}

	void OnEnable()
	{
		var vr = SteamVR.instance;
		if (vr != null && vr.compositor != null)
			vr.compositor.SetSkyboxOverride(vr.graphicsAPI,
				front ? front.GetNativeTexturePtr() : System.IntPtr.Zero,
				back ? back.GetNativeTexturePtr() : System.IntPtr.Zero,
				left ? left.GetNativeTexturePtr() : System.IntPtr.Zero,
				right ? right.GetNativeTexturePtr() : System.IntPtr.Zero,
				top ? top.GetNativeTexturePtr() : System.IntPtr.Zero,
				bottom ? bottom.GetNativeTexturePtr() : System.IntPtr.Zero);
	}

	void OnDisable()
	{
		if (SteamVR.active)
		{
			var vr = SteamVR.instance;
			if (vr.compositor != null)
				vr.compositor.ClearSkyboxOverride();
		}
	}
}

