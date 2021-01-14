//Copyright 2017-2019 Looking Glass Factory Inc.
//All rights reserved.
//Unauthorized copying or distribution of this file, and the source code contained herein, is strictly prohibited.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace LookingGlass.Demos {
	public class DemoQuilt : MonoBehaviour {
		float [] Is_Answer_Correct = new float [3];
		float coin = 0f;
		int log_iterator = 0;
		public float timer;
		public float [] time_turn = new float [3];
		public float time_start;
		public GameObject User_Study_Mode_Canvas;
		public TextMeshProUGUI User_Study_Mode_Text;
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
		public GameObject Ask_Quality_Sequence_Button;
		private string[] imagePathNow = {"elephant/", "pink_stuffed_animal/", "stuffed_gorilla/", "train/", "trees/"};
		// current: [quality,parallax]
		private int[,] quality_matrix = new int[15, 2] {{70, 45}, {70, 23}, {70, 15}, {70, 12}, {70, 9}, {50, 45}, {50, 23}, {50, 15}, {50, 12}, {50, 9}, {30, 45}, {30, 23}, {30, 15}, {30, 12}, {30, 9}}; 
		int imageIndex = 0;
		int [] Quality_array = {95, 70, 45, 20};
		int Quality_array_iterator = 0;
		int [] Parallax_array = {45, 23, 15, 12}; // {45, 23, 15, 12, 9};
		int Parallax_array_iterator = 0;
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
		Vector3 User_Study_Done = new Vector3(0, 0, 0); // quality; parallax; set
		[Header("Set Control")]
		public int User_Study_Mode = 0; // 0: init; 1: Simul 2. Flicker
		void Start() {
			LogTracker = Log_Tracker.GetComponent<LogTracker>();
			for (PathIndex = 0; PathIndex < imagePathNow.Length; PathIndex++)
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
			print(User_Study_Mode);
			if (Quality_array_iterator == Quality_array.Length - 1)
			{
				Quality_array_iterator = 0;
				User_Study_Done.x = 1;
			}
			if (User_Study_Done.x == 1)
			{
				
				if (User_Study_Done.y != 1)
				{
					User_Study_Mode = 2;
					User_Study_Mode_Text.text = "Parallax";
					StartCoroutine(SetObjectActive_GameObject_After(User_Study_Mode_Canvas, 0));
					return;
					// Next_Compare_parallax();
				}
				else if (User_Study_Done.z == 0 && PathIndex != (imagePathNow.Length - 1))
				{
					PathIndex += 1;
					Quality_array_iterator = 0;
					Parallax_array_iterator = 0;
					User_Study_Done.x = 0;
					User_Study_Done.y = 0;
					StartCoroutine(SetObjectActive_GameObject_After(User_Study_Mode_Canvas, 0));
					return;
					// Next set
				}
				else
				{
					User_Study_Done.z = 1;
					StartCoroutine(SetObjectActive_GameObject_After(UIEnd.gameObject, 0));
					return;
					// Done
				}
			}

			coin = Random.Range(0.0f, 1.0f);
			float buffer_duration = 1.0f;
			float scene_duration = 10.0f;
			float accumulate = 0f;
			if (coin > 0.5f)
			{
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Next_SequenceMode(accumulate));
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Ref_SequenceMode(accumulate)); // Ref
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
			}
			else
			{	
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Ref_SequenceMode(accumulate)); // Ref	
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Next_SequenceMode(accumulate));
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
			}
			accumulate += buffer_duration;
			StartCoroutine(SetObjectActive_GameObject_After(Ask_Quality_Sequence_Button, accumulate));
			time_start = timer + accumulate;//- accumulate;
		}

		void Next_Compare_parallax()
		{
			print(User_Study_Mode);
			if (Parallax_array_iterator == Parallax_array.Length - 1)
			{
				Parallax_array_iterator = 0;
				User_Study_Done.y = 1;
			}
			if (User_Study_Done.y == 1)
			{
				if (User_Study_Done.x != 1)
				{
					User_Study_Mode = 1;
					User_Study_Mode_Text.text = "Quality";
					StartCoroutine(SetObjectActive_GameObject_After(User_Study_Mode_Canvas, 0));
					return;
					// Next_Compare_quality();
				}
				else if (User_Study_Done.z == 0 && PathIndex != (imagePathNow.Length - 1))
				{
					PathIndex += 1;
					Quality_array_iterator = 0;
					Parallax_array_iterator = 0;
					User_Study_Done.x = 0;
					User_Study_Done.y = 0;
					StartCoroutine(SetObjectActive_GameObject_After(User_Study_Mode_Canvas, 0));
					return;
					// Next set
				}
				else
				{
					User_Study_Done.z = 1;
					StartCoroutine(SetObjectActive_GameObject_After(UIEnd.gameObject, 0));
					return;
				}
			}

			coin = Random.Range(0.0f, 1.0f);
			float buffer_duration = 1.0f;
			float scene_duration = 10.0f;
			float accumulate = 0f;
			if (coin > 0.5f)
			{
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Next_SequenceMode(accumulate));
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Ref_SequenceMode(accumulate)); // Ref
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
			}
			else
			{	
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Ref_SequenceMode(accumulate)); // Ref	
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
				accumulate += buffer_duration;
				StartCoroutine(Play_Next_SequenceMode(accumulate));
				accumulate += scene_duration;
				StartCoroutine(ShowBlank(accumulate));
			}
			accumulate += buffer_duration;
			StartCoroutine(SetObjectActive_GameObject_After(Ask_Quality_Sequence_Button, accumulate));
			time_start = timer + accumulate ;//- accumulate;
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
			timer += Time.deltaTime;
			// frame player
			/* 
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
		IEnumerator Play_Next_SequenceMode(float second)
		{
			yield return new WaitForSeconds (second);
			if (User_Study_Mode == 1)
			{
				Quality_array_iterator += 1;
				print(Quality_array[Quality_array_iterator]);
				//print(Parallax_array[0]);
				var files = Directory.GetFiles("Assets/Resources/UCSD/" + imagePathNow[PathIndex], "Tile_generate__" + "*__" + Quality_array[Quality_array_iterator] +  "__" + Parallax_array[0] + "*.jpg" );
				result = Path.GetFileName(files[0]);
				result = result.Substring(0, result.Length - 4);
				print(result);
				// quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex + 1] + "Tile_generate__" + "*__" + cur_quality +  "__" + cur_parallax);
				quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex] + result);
				holoplay.overrideQuilt = quiltToOverrideWith;
			}
			else if (User_Study_Mode == 2)
			{
				Parallax_array_iterator += 1;
				print(Parallax_array[Parallax_array_iterator]);
				//print(Parallax_array[0]);
				var files = Directory.GetFiles("Assets/Resources/UCSD/" + imagePathNow[PathIndex], "Tile_generate__" + "*__" + Quality_array[0] +  "__" + Parallax_array[Parallax_array_iterator] + "*.jpg" );
				result = Path.GetFileName(files[0]);
				result = result.Substring(0, result.Length - 4);
				print(result);
				// quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex + 1] + "Tile_generate__" + "*__" + cur_quality +  "__" + cur_parallax);
				quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex] + result);
				holoplay.overrideQuilt = quiltToOverrideWith;
			}
		}
		IEnumerator Play_Next_Parallax(float second) // back up
		{
			yield return new WaitForSeconds (second);
			Parallax_array_iterator += 1;
			print(Parallax_array[Parallax_array_iterator]);
			//print(Parallax_array[0]);
			var files = Directory.GetFiles("Assets/Resources/UCSD/" + imagePathNow[PathIndex], "Tile_generate__" + "*__" + Quality_array[0] +  "__" + Parallax_array[Parallax_array_iterator] + "*.jpg" );
			result = Path.GetFileName(files[0]);
			result = result.Substring(0, result.Length - 4);
			print(result);
			// quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex + 1] + "Tile_generate__" + "*__" + cur_quality +  "__" + cur_parallax);
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex] + result);
			holoplay.overrideQuilt = quiltToOverrideWith;
		}

		IEnumerator Play_Ref_SequenceMode(float second)
		{
			print("playsource");
			yield return new WaitForSeconds (second);
			var files = Directory.GetFiles("Assets/Resources/UCSD/" + imagePathNow[PathIndex], "Tile_generate__" + "*__" + "95" +  "__" + "45" + "*.jpg" );
			result = Path.GetFileName(files[0]);
			result = result.Substring(0, result.Length - 4);
			print(result);
			// quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex + 1] + "Tile_generate__" + "*__" + cur_quality +  "__" + cur_parallax);
			quiltToOverrideWith = (Texture2D)Resources.Load<Texture2D>("UCSD/" + imagePathNow[PathIndex] + result);
			holoplay.overrideQuilt = quiltToOverrideWith;
		}
		IEnumerator SetObjectActive_GameObject_After(GameObject something, float second)
		{
			yield return new WaitForSeconds (second);
			something.gameObject.SetActive(true);
			// print("ff");
		}

		public void Continue_Current_Test()
		{
			if (User_Study_Mode == 1)
			{
				Next_Compare_quality();
			}
			else
			{
				Next_Compare_parallax();
			}
		}

		public void Get_User_Study_Mode()
		{
			User_Study_Mode = LogTracker.User_Study_Mode;
			StartCoroutine(Set_User_Study_String());
		}

		public void Go_Next_Study_Mode()
		{
			if(User_Study_Mode == 1)
			{
				Next_Compare_quality();
			}
			else
			{
				Next_Compare_parallax();
			}
		}

		IEnumerator Set_User_Study_String()
		{
			yield return new WaitForSeconds (1.0f);
			if (User_Study_Mode == 1)
			{
				User_Study_Mode_Text.text = "Quality";
			}
			else
			{
				User_Study_Mode_Text.text = "Parallax";
			}
		}

		public void Is_Second_Correct()
		{
			time_turn[log_iterator] = timer - time_start;
			if (coin > 0.5)
			{
				Is_Answer_Correct[log_iterator] = 1;
			}
			else
			{
				Is_Answer_Correct[log_iterator] = 0;
			}

			log_iterator += 1;
			if ((Parallax_array_iterator == Parallax_array.Length -1) || (Quality_array_iterator == Quality_array.Length -1))
			{
				LogTracker.Quality_flag = User_Study_Mode;
				LogTracker.log_flag = true;
				string answer_to_parse = "";
				for (int i = 0; i < 3; i++)
				{
					answer_to_parse = answer_to_parse + Is_Answer_Correct[i].ToString() + "," + time_turn[i].ToString();
					if (i != 2)
					{
						answer_to_parse += ",";
					}
				}
				LogTracker.string_to_parse = answer_to_parse;
				log_iterator = 0;
			}
			
		}
		public void Is_First_Correct()
		{
			time_turn[log_iterator] = timer - time_start;
			if (coin > 0.5)
			{
				Is_Answer_Correct[log_iterator] = 0;
			}
			else
			{
				Is_Answer_Correct[log_iterator] = 1;
			}
			
			log_iterator += 1;
			if ((Parallax_array_iterator == Parallax_array.Length -1) || (Quality_array_iterator == Quality_array.Length -1))
			{
				LogTracker.Quality_flag = User_Study_Mode;
				LogTracker.log_flag = true;
				string answer_to_parse = "";
				for (int i = 0; i < 3; i++)
				{
					answer_to_parse = answer_to_parse + Is_Answer_Correct[i].ToString() + "," + time_turn[i].ToString();
					if (i != 2)
					{
						answer_to_parse += ",";
					}
				}
				LogTracker.string_to_parse = answer_to_parse;
				log_iterator = 0;
			}
		}

	}
}
