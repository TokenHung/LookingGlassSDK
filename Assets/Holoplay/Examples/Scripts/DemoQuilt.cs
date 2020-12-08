//Copyright 2017-2019 Looking Glass Factory Inc.
//All rights reserved.
//Unauthorized copying or distribution of this file, and the source code contained herein, is strictly prohibited.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LookingGlass.Demos {
	public class DemoQuilt : MonoBehaviour {
		public GameObject UIStart;
		public GameObject UIEnd;
		private bool CanMove = false;
		public LookingGlass.Holoplay holoplay;
		public Texture2D quiltToOverrideWith;
		public enum WhatToShow { SceneOnly, QuiltOnly, SceneOnQuilt};
		public WhatToShow whatToShow = WhatToShow.SceneOnly;
		private WhatToShow lastFrame;
		//private string imagePathToy = "Toy/";
		//private string imagePathCastle = "Castle/";
		private string[] imagePathNow = {"Toys/", "Castle/", "Dragon/", "Flower'/", "Holiday", "Seal and Balls"};
		int imageIndex = 0;
		Texture2D[] textureType;
		Object[] textures;
		string result ="";
		//float timer = 0f;
		private int PathIndex;

		void Start() {
			for (PathIndex = 0; PathIndex < 6; PathIndex++)
			{
				textures = Resources.LoadAll(imagePathNow[PathIndex], typeof(Texture2D));
				textureType = new Texture2D[textures.Length];
				for (int i = 0; i < textures.Length; i++){
					textureType[i] = (Texture2D)textures[i];
				}
			}
			PathIndex = -1;
			holoplay.overrideQuilt = quiltToOverrideWith;
			holoplay.renderOverrideBehind = true;
			holoplay.background = new Color(0, 0, 1, 1);
			holoplay.overrideQuilt = quiltToOverrideWith;
			holoplay.renderOverrideBehind = true;
			holoplay.background = new Color(0, 0, 1, 1);
		}
		public void NextSet()
		{
			PathIndex += 1;
			KeyNext();
			CanMove = true;
		}

		void KeyNext()
		{
			var files = Directory.GetFiles("Assets/Resources/" + imagePathNow[PathIndex] , "Tile_generate__" + imageIndex.ToString() + "__" + "*.jpg" );
			result = Path.GetFileName(files[0]);
			result = result.Substring(0, result.Length - 4);
			print(result.Substring(0, result.Length - 6));
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>( imagePathNow[PathIndex] + result);
			holoplay.overrideQuilt = quiltToOverrideWith;	
		}
		public void Restart()
		{
			imageIndex = 0;
			KeyNext();
			CanMove = true;
		}
		void Update () {
			if(CanMove)
			{	
				if (Input.GetKeyUp(KeyCode.RightArrow))
				{
					if (imageIndex < (textures.Length - 1))
					{
						imageIndex = imageIndex + 1;
					}
					else
					{
						UIEnd.gameObject.SetActive(true);
						CanMove = false;
					}
					print("Right");
				}
				if (Input.GetKeyUp(KeyCode.LeftArrow))
				{
					if (imageIndex > 0)
					{
						imageIndex = imageIndex - 1;
					}
					print("Left");
				}

				if (Input.GetKeyUp(KeyCode.Escape))
				{
					Debug.Log(result);
				}

				if (textureType != null  && textureType.Length > 0 && (Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.LeftArrow)) )
				{
					KeyNext();
				}
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
