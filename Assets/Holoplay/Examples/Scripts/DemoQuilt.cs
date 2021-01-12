//Copyright 2017-2019 Looking Glass Factory Inc.
//All rights reserved.
//Unauthorized copying or distribution of this file, and the source code contained herein, is strictly prohibited.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LookingGlass.Demos {
	public class DemoQuilt : MonoBehaviour {
		public GameObject Log_Tracker;
		private LogTracker LogTracker;
		public GameObject UIStart;
		public GameObject UIEnd;
		public GameObject User_Study_Finish;
		public GameObject Unfinished_Set;
		private bool ESC_flag = false; // check if ESC press twice in the same pic
		public LookingGlass.Holoplay holoplay;
		public Texture2D quiltToOverrideWith;
		public enum WhatToShow { SceneOnly, QuiltOnly, SceneOnQuilt};
		public WhatToShow whatToShow = WhatToShow.SceneOnly;
		private WhatToShow lastFrame;
		private string[] imagePathNow = {"elephant/", "Castle/", "Dragon/", "Flowers/", "Holiday/", "Seal and Balls/"};
		// current: [quality,parallax]
		private int[,] quality_matrix = new int[15, 2] {{70, 45}, {70, 23}, {70, 15}, {70, 12}, {70, 9}, {50, 45}, {50, 23}, {50, 15}, {50, 12}, {50, 9}, {30, 45}, {30, 23}, {30, 15}, {30, 12}, {30, 9}}; 
		int imageIndex = 0;
		int [] Quality_array = {70, 50, 30};
		int [] Parallax_array = {45, 23, 15, 12, 9};
		Texture2D[] textureType;
		Object[] textures;
		string result ="";
		private int PathIndex;

		int cur_quality = 90;
		int cur_parallax = 45;

		[Header("Set Control")]
		private bool CanMove = false;
		int set_number_count = 0;
		int last_number = 0;
		int ESC_Count = 0;

		void Start() {
			LogTracker = Log_Tracker.GetComponent<LogTracker>();
			for (PathIndex = 0; PathIndex < 6; PathIndex++)
			{
				textures = Resources.LoadAll(imagePathNow[PathIndex], typeof(Texture2D));
				textureType = new Texture2D[textures.Length];
				for (int i = 0; i < textures.Length; i++){
					textureType[i] = (Texture2D)textures[i];
				}
			}
			PathIndex = 0;
			holoplay.overrideQuilt = quiltToOverrideWith;
			holoplay.renderOverrideBehind = true;
			holoplay.background = new Color(0, 0, 1, 1);
			holoplay.overrideQuilt = quiltToOverrideWith;
			holoplay.renderOverrideBehind = true;
			holoplay.background = new Color(0, 0, 1, 1);
		}

		public void Skip_Remaining_Image()
		{
			LogTracker.unfinished_flag = true;
			NextSet();
		}
		public void NextSet()
		{
			ESC_Count = 0;
			PathIndex += 1;
			imageIndex = 0;
			last_number = 0;
			set_number_count = 0;
			//KeyNext();
			CanMove = true;
		}

		void KeyNext_Flicker_quality()
		{
			//ESC_Count = 0;
			var files = Directory.GetFiles("Assets/Resources/" + imagePathNow[PathIndex] + "/" , "Tile_generate__" + imageIndex.ToString() + "__" + "*.jpg" );
			result = Path.GetFileName(files[0]);
			result = result.Substring(0, result.Length - 4);
			//print(result.Length);
			//print(result.Substring(0, result.Length - 6));
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>( imagePathNow[PathIndex] + result);
			holoplay.overrideQuilt = quiltToOverrideWith;	
		}

		void Next_Compare_quality()
		{
			float coin = Random.Range(0.0f, 1.0f);
			if (coin > 0.5f)
			{
				StartCoroutine(Play_Spefic_Stimulus(90, 45, 0f));
				StartCoroutine(ShowBlank(5.0f));
				StartCoroutine(Play_Spefic_Stimulus(30, 45, 6.0f)); // Ref		
			}
			else
			{	
				StartCoroutine(Play_Spefic_Stimulus(30, 45, 0f)); // Ref
				StartCoroutine(ShowBlank(5.0f));
				StartCoroutine(Play_Spefic_Stimulus(90,45, 6.0f));
			}
		}

		void Next_Compare_parallax()
		{

		}

		/*public void Restart()
		{
			LogTracker.Quality_Matrix_csv_string = "";
			LogTracker.matrix_index = 0;
			ESC_Count = 0;
			imageIndex = 0;
			last_number = 0;
			set_number_count = 0;
			KeyNext();
			CanMove = true;
		}*/
		void Update () {
			if (Input.GetKeyUp("r")) // debugging
			{
				Next_Compare_quality();
			}

			if(ESC_Count == 5)
			{
				while(true)
				{
					CanMove = false;
					if (PathIndex == 5)
					{
						User_Study_Finish.gameObject.SetActive(true);
						break;
					}
				UIStart.gameObject.SetActive(true);
				//NextSet();
				break;
				}
			}
			if(CanMove)
			{	
				if (Input.GetKeyUp(KeyCode.RightArrow))
				{
					set_number_count += 1;
					if (imageIndex < (textures.Length - 1))
					{
						imageIndex = imageIndex + 1;
					}
					else
					{
						UIEnd.gameObject.SetActive(true);
						CanMove = false;
					}
					Debug.Log("Right");
				}
				if (Input.GetKeyUp(KeyCode.LeftArrow))
				{	
					while(true)
					{
						if (set_number_count == 0)
						{
							Debug.Log("This is the first picture");
							break;
						}
						else if (last_number == set_number_count)
						{
							Debug.Log("You reach last ESC");
							break;
						}
						set_number_count -= 1;
						if (imageIndex > 0)
						{
							imageIndex = imageIndex - 1;
						}
						Debug.Log("Left");
						break;
					}
				}			



				if (Input.GetKeyUp(KeyCode.Escape))
				{
					while(true)
					{
						if (set_number_count == 0)
						{
							Debug.Log("This is the first picture");
							break;
						}
						if(last_number == set_number_count)
						{
							Debug.Log("This is the last JND you perceive");
							break;
						}
						ESC_Count++;
						Debug.Log("You press ESC, you had press ( " + ESC_Count + " / 5");
						last_number = set_number_count;
						LogTracker.file_name_to_parse = result;
						LogTracker.log_flag = true;
					// Debug.Log(result);
					break;
					}
				}

				if (textureType != null  && textureType.Length > 0 && (Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.LeftArrow)) )
				{
					//KeyNext();
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
		IEnumerator Play_Spefic_Stimulus(int Quality, int Parallax, float second)
		{
			yield return new WaitForSeconds (second);
			var files = Directory.GetFiles("Assets/Resources/UCSD/" + imagePathNow[PathIndex], "Tile_generate__" + "*__" + Quality +  "__" + Parallax + "*.jpg" );
			result = Path.GetFileName(files[0]);
			result = result.Substring(0, result.Length - 4);
			print(result);
			// quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex + 1] + "Tile_generate__" + "*__" + cur_quality +  "__" + cur_parallax);
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex] + result);
			holoplay.overrideQuilt = quiltToOverrideWith;
		}

		IEnumerator ShowBlank(float second)
		{
			yield return new WaitForSeconds (second);	
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("black");
			holoplay.overrideQuilt = quiltToOverrideWith;
		}
	}
}
