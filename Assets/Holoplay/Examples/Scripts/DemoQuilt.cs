//Copyright 2017-2019 Looking Glass Factory Inc.
//All rights reserved.
//Unauthorized copying or distribution of this file, and the source code contained herein, is strictly prohibited.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LookingGlass.Demos {
	public class DemoQuilt : MonoBehaviour {
		public LookingGlass.Holoplay holoplay;
		public Texture2D quiltToOverrideWith;
		public enum WhatToShow { SceneOnly, QuiltOnly, SceneOnQuilt};
		public WhatToShow whatToShow = WhatToShow.SceneOnly;
		private WhatToShow lastFrame;
		private string imagePathToy = "Toy/";
		private string imagePathCastle = "Castle/";
		int imageIndex = -1;
		Texture2D[] textureType;
		Object[] textures;
		string result ="";
		//float timer = 0f;

		void Start() {
			textures = Resources.LoadAll(imagePathCastle, typeof(Texture2D));
			textureType = new Texture2D[textures.Length];
			for (int i = 0; i < textures.Length; i++){
            	textureType[i] = (Texture2D)textures[i];
        	}
			holoplay.overrideQuilt = quiltToOverrideWith;
			holoplay.renderOverrideBehind = true;
			holoplay.background = new Color(0, 0, 1, 1);
		}

		void Update () {
			
			if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				imageIndex = imageIndex + 1;
				print("Right");
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				imageIndex = imageIndex - 1;
				print("Left");
			}

			if (Input.GetKeyUp(KeyCode.Escape)) ////
			{
				Debug.Log(result);
			}

			if (textureType != null  && textureType.Length > 0 && (Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.LeftArrow)) )
			{
				var files = Directory.GetFiles("Assets/Resources/Toys/", "Tile_generate__" + imageIndex.ToString() + "__" + "*.jpg" );
				result = Path.GetFileName(files[0]);
				result = result.Substring(0, result.Length - 4);
				//print(files[0]);
				print(result);
				//quiltToOverrideWith = (Texture2D)textures[imageIndex];
				quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>( imagePathCastle + result);
				//quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>( imagePathCastle + "/Tile_generate__" + imageIndex.ToString() + "*");
				holoplay.overrideQuilt = quiltToOverrideWith;	
				// timer = 0f;
				//print("now imageIndex" + imageIndex);
				//print(imagePathCastle + "/Tile_generate__" + imageIndex.ToString() + ".png");
			}

			if (whatToShow != lastFrame) {
				if (whatToShow == WhatToShow.SceneOnly) {
					holoplay.renderOverrideBehind = true;
					holoplay.background = new Color(0, 0, 1, 1);
				}
				if (whatToShow == WhatToShow.QuiltOnly) {
					holoplay.renderOverrideBehind = false;
					holoplay.background = new Color(0, 0, 1, 1);
				}
				if (whatToShow == WhatToShow.SceneOnQuilt) {
					holoplay.renderOverrideBehind = true;
					holoplay.background = new Color(0, 0, 1, 0);
				}
				lastFrame = whatToShow;
			}

		
			// frame player
			/* timer += Time.deltaTime;
			if (textureType != null  && textureType.Length > 0 && timer > 0.3f)
			{
				imageIndex++;
					if (imageIndex >= textureType.Length){
						imageIndex = 1;
					}
				
				//quiltToOverrideWith = (Texture2D)textures[imageIndex];
				quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>( imagePath + "/Tile_generate__" + imageIndex.ToString());
				
				holoplay.overrideQuilt = quiltToOverrideWith;	
				timer = 0f;
				print("now imageIndex" + imageIndex);
				print(imagePath + "/Tile_generate__" + imageIndex.ToString() + ".png");
			}*/

		}
	}
}
