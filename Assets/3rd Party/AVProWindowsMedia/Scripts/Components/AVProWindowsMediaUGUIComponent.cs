#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5
#define UNITY_FEATURE_UGUI
#endif

using System.Collections.Generic;
using UnityEngine.Serialization;
#if UNITY_FEATURE_UGUI
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------------------------
// Copyright 2012-2016 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[AddComponentMenu("AVPro Windows Media/uGUI Movie")]
public class AVProWindowsMediaUGUIComponent : UnityEngine.UI.MaskableGraphic
{
	[SerializeField]
	public AVProWindowsMediaMovie m_movie;

	[SerializeField]
	public Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	public bool _setNativeSize = false;

	[SerializeField]
	public bool _keepAspectRatio = true;

	[SerializeField]
	public bool _noDefaultDisplay = true;

	[SerializeField]
	public Texture _defaultTexture;

	private int _lastWidth;
	private int _lastHeight;

	protected AVProWindowsMediaUGUIComponent()
	{ }


	/// <summary>
	/// Returns the texture used to draw this Graphic.
	/// </summary>
	public override Texture mainTexture
	{
		get
		{
			Texture result = Texture2D.whiteTexture;
			if (HasValidTexture())
			{
				result = m_movie.OutputTexture;
			}
			else
			{
				if (_noDefaultDisplay)
				{
					result = null;
				}
				else if (_defaultTexture != null)
				{
					result = _defaultTexture;
				}
			}
			return result;
		}
	}

	public bool HasValidTexture()
	{
		return (m_movie != null && m_movie.OutputTexture != null);
	}

	void Update()
	{
		if (mainTexture == null)
		{
			return;
		}

		if (_setNativeSize)
		{
			SetNativeSize();
		}
		if (HasValidTexture())
		{
			if (mainTexture.width != _lastWidth ||
			    mainTexture.height != _lastHeight)
			{
				_lastWidth = mainTexture.width;
				_lastHeight = mainTexture.height;
				SetVerticesDirty();
			}
		}
		SetMaterialDirty();
	}

	/// <summary>
	/// Texture to be used.
	/// </summary>
	public AVProWindowsMediaMovie texture
	{
		get
		{
			return m_movie;
		}
		set
		{
			if (m_movie == value)
				return;

			m_movie = value;
			//SetVerticesDirty();
			SetMaterialDirty();
		}
	}

	/// <summary>
	/// UV rectangle used by the texture.
	/// </summary>
	public Rect uvRect
	{
		get
		{
			return m_UVRect;
		}
		set
		{
			if (m_UVRect == value)
				return;
			m_UVRect = value;
			SetVerticesDirty();
		}
	}

	/// <summary>
	/// Adjust the scale of the Graphic to make it pixel-perfect.
	/// </summary>

	[ContextMenu("Set Native Size")]
	public override void SetNativeSize()
	{
		Texture tex = mainTexture;
		if (tex != null)
		{
			int w = Mathf.RoundToInt(tex.width * uvRect.width);
			int h = Mathf.RoundToInt(tex.height * uvRect.height);
			rectTransform.anchorMax = rectTransform.anchorMin;
			rectTransform.sizeDelta = new Vector2(w, h);
		}
	}

	/// <summary>
	/// Update all renderer data.
	/// </summary>
#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();

		List<UIVertex> aVerts = new List<UIVertex>();
		_OnFillVBO(aVerts);

		List<int> aIndicies = new List<int>(new int[] { 0, 1, 2, 2, 3, 0 });
		vh.AddUIVertexStream(aVerts, aIndicies);
	}
	[System.Obsolete("This method is not called from Unity 5.2 and above")]
#endif
	protected override void OnFillVBO(List<UIVertex> vbo)
	{
		_OnFillVBO(vbo);
	}

	protected void _OnFillVBO(List<UIVertex> vbo)
	{
		bool flipY = false;
		if (HasValidTexture())
		{
			flipY = m_movie.MovieInstance.RequiresFlipY;
		}

		Vector4 v = GetDrawingDimensions(_keepAspectRatio);

		vbo.Clear();

		var vert = UIVertex.simpleVert;

		vert.position = new Vector2(v.x, v.y);
		vert.uv0 = new Vector2(m_UVRect.xMin, m_UVRect.yMin);
		if (flipY)
		{
			vert.uv0 = new Vector2(m_UVRect.xMin, 1.0f - m_UVRect.yMin);
		}
		vert.color = color;
		vbo.Add(vert);

		vert.position = new Vector2(v.x, v.w);
		vert.uv0 = new Vector2(m_UVRect.xMin, m_UVRect.yMax);
		if (flipY)
		{
			vert.uv0 = new Vector2(m_UVRect.xMin, 1.0f - m_UVRect.yMax);
		}
		vert.color = color;
		vbo.Add(vert);

		vert.position = new Vector2(v.z, v.w);
		vert.uv0 = new Vector2(m_UVRect.xMax, m_UVRect.yMax);
		if (flipY)
		{
			vert.uv0 = new Vector2(m_UVRect.xMax, 1.0f - m_UVRect.yMax);
		}
		vert.color = color;
		vbo.Add(vert);

		vert.position = new Vector2(v.z, v.y);
		vert.uv0 = new Vector2(m_UVRect.xMax, m_UVRect.yMin);
		if (flipY)
		{
			vert.uv0 = new Vector2(m_UVRect.xMax, 1.0f - m_UVRect.yMin);
		}
		vert.color = color;
		vbo.Add(vert);
	}

	//Added this method from Image.cs to do the keep aspect ratio
	private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
	{
		Vector4 returnSize = Vector4.zero;

		if (mainTexture)
		{
			var padding = Vector4.zero;
			var textureSize = new Vector2(mainTexture.width, mainTexture.height);

			Rect r = GetPixelAdjustedRect();

			int spriteW = Mathf.RoundToInt(textureSize.x);
			int spriteH = Mathf.RoundToInt(textureSize.y);

			var size = new Vector4(padding.x / spriteW,
									padding.y / spriteH,
									(spriteW - padding.z) / spriteW,
									(spriteH - padding.w) / spriteH);

			if (shouldPreserveAspect && textureSize.sqrMagnitude > 0.0f)
			{
				var spriteRatio = textureSize.x / textureSize.y;
				var rectRatio = r.width / r.height;

				if (spriteRatio > rectRatio)
				{
					var oldHeight = r.height;
					r.height = r.width * (1.0f / spriteRatio);
					r.y += (oldHeight - r.height) * rectTransform.pivot.y;
				}
				else
				{
					var oldWidth = r.width;
					r.width = r.height * spriteRatio;
					r.x += (oldWidth - r.width) * rectTransform.pivot.x;
				}
			}

			returnSize = new Vector4(r.x + r.width * size.x,
									  r.y + r.height * size.y,
									  r.x + r.width * size.z,
									  r.y + r.height * size.w);

		}

		return returnSize;
	}
}

#endif