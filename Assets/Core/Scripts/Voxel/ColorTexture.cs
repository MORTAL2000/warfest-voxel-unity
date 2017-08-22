﻿using UnityEngine;
using System.Collections.Generic;

public class ColorTexture : MonoBehaviour {

	Texture2D texture;

	public Texture2D Texture { get { return texture; } }

	Dictionary<Color32, Vector2> colorMap;

	[SerializeField] int size = 0;

	public int Size { get { return size; } }

	public void AddColors(Color32[] colors) {
		texture = new Texture2D(size, size);
		colorMap = new Dictionary<Color32, Vector2>();

		for (int i = 0; i < colors.Length; i++) {
			int x = i % size;
			int y = i / size;
			Color32 color = colors[i];

			texture.SetPixel(x, y, color);
			colorMap.Add(color, new Vector2(x, y));
		}

		texture.filterMode = FilterMode.Point;
		texture.Apply();
	}

	// TODO: cache uvs
	public Vector2[] GetColorUVs(Color32 color) {
		Vector2 pos = colorMap[color];
		Vector2[] UVs = new Vector2[4];

		const float offset = 0.2f;

		UVs[0] = new Vector2(
			(pos.x + offset)  / (float)size,
			(pos.y + offset) / (float)size
		);
		UVs[1] = new Vector2(
			(pos.x + offset) / (float)size,
			(pos.y + 1 - offset) / (float)size
		);
		UVs[2] = new Vector2(
			(pos.x + 1 - offset) / (float)size,
			(pos.y + 1 - offset) / (float)size
		);
		UVs[3] = new Vector2(
			(pos.x + 1 - offset) / (float)size,
			(pos.y + offset) / (float)size
		);

		Debug.LogFormat(
			"color: {0}, pos: {1}, uv: ({2},{3}) ({4},{5}) ({6},{7}) ({8},{9})",
			color,
			pos,
			UVs[0].x, UVs[0].y,
			UVs[1].x, UVs[1].y,
			UVs[2].x, UVs[2].y,
			UVs[3].x, UVs[3].y
		);

		return UVs;
	}

	public void SaveTextureToFile() {
		string filename = Application.dataPath + "/StreamingAssets/texture.png";

		System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
		Debug.LogFormat("Texture save in {0}", filename);
	}

}
