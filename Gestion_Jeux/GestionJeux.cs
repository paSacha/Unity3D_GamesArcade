/************************ IMPORTANT ******************************************
 * Pour ajouter un jeu accessible depuis le menu il faut respecter 3 règles:
 * 
 *  -Le GameObject contenant le jeu doit impérativement se nommer par le Titre
 *      qui se trouve dans le fichier XML
 * 
 *  -Dans le même principe l'aperçu du jeu doit aussi porter le nom du Titre
 * 
 *  -Enfin il faut placer le GameObject dans le dossier Resources\GameObjects
 *      et l'aperçu dans le dossier Resources\Textures
 * 
 ******************************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic ;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
//using BUS;

namespace Game2D
{
	public class GestionJeux : MonoBehaviour
	{

		private string CHEMIN ;
		private float X0ss;
		private float Y0ss;
		private float Wss;
		private float Hss;

		public static float demi = 0.5f;
		private GUISkin skin_Menu;

		private int NumJeuxEnCours = 0;
		private Tab_Jeux TabJeux = null;
        private GameObject NewJeu;

        public Camera camJeux;
        private Camera camJoueur;
		
		//Init des textures des aperçus des jeux 
		private int NombreJeux;
		private Texture[] ApercuJeux ;
		private string[] TitresJeux;
		private string[] DescriptionsJeux;
		private GameObject[] GameObjectsJeux;
		/**
		 * 
		 *  
		 **/ 	
		void Start () {
			CHEMIN =  Application.dataPath + "/FichiersXML/Jeux.xml";
			NumJeuxEnCours = 0;
			//ouverture ou création du fichier XML*******
			if (File.Exists (CHEMIN))
				TabJeux = Tab_Jeux.Charger (CHEMIN);
			else
				TabJeux = new Tab_Jeux ();
			//*******************************************
			skin_Menu = Resources.Load ("Skins/SkinMenuJeux") as GUISkin;
            camJoueur = GameObject.Find("Joueur").GetComponent<Camera>();

			InitialisationJeux ();
		}
		/**
		 * 
		 *  
		 **/ 
		void InitialisationJeux() {
			int i = 0;
			//récupération du nombre de jeux
			foreach (Jeu game in TabJeux) {
				NombreJeux++;
			}
			TitresJeux = new string[NombreJeux];
			DescriptionsJeux = new string[NombreJeux];
			ApercuJeux = new Texture[NombreJeux];
			GameObjectsJeux = new GameObject[NombreJeux];
			//récupération des paramètres qui définissent un jeu
			foreach (Jeu game in TabJeux) {
				TitresJeux[i] = game.Titre; //récupération du titre
				DescriptionsJeux[i] = game.Description; //récupération de la description
				ApercuJeux[i] = (Texture2D) (Resources.Load ("Textures/"+TitresJeux[i])); //attribution de l'apercu en fonction de son titre
				GameObjectsJeux[i] = (GameObject) (Resources.Load ("GameObjects/"+ TitresJeux[i])); //attribution du Jeu en fonction de son titre
				i++;
			}
		}
		/**
		 * 
		 *  
		 **/ 
		void Update () {
			//paramètres de l'area en fonction de la taille de l'écran
			parametre (Screen.width * 0.1f*demi+Screen.width*demi, Screen.height * 0.1f*demi+Screen.height*demi/2 , Screen.width * 0.8f*demi, Screen.height * 0.8f*demi);
		}
		/**
		 * 
		 *  
		 **/ 
		void OnGUI() {
			GUI.skin = skin_Menu;

			//paramètre pour l'affichage du titre du jeu
			skin_Menu.box.wordWrap = false;
			skin_Menu.box.fontSize = (int)(Screen.height * 0.03*demi );
			skin_Menu.button.fontSize = (int)(Screen.height * 0.04*demi);

			GUILayout.BeginArea (new Rect (X0ss, Y0ss, Wss, Hss),GUI.skin.customStyles[0]);
			GUILayout.BeginHorizontal ();

			/**/	GUILayout.BeginVertical (GUILayout.Width (Screen.width * 0.15f*demi));
			/****/		GUILayout.FlexibleSpace();
			/**/	GUILayout.EndVertical ();

			/********* Zone pour le titre, la description et un aperçu du jeu **************/
			/**/	GUILayout.BeginVertical (GUILayout.Width (Screen.width * 0.35f*demi));
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		GUILayout.Box (TitresJeux[NumJeuxEnCours]); //gestion des titres
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.2f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		skin_Menu.box.wordWrap = true; //permet de faire tenir un grand texte
			/******/		skin_Menu.box.fontSize = (int)(Screen.height * 0.025*demi); //changement de taille de police pour la description
			/******/		GUILayout.Box (DescriptionsJeux[NumJeuxEnCours]); //gestion des descriptions
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.2f*demi));
			/******/		GUILayout.FlexibleSpace(); //gestion des aperçus
			/******/		GUILayout.Box (ApercuJeux[NumJeuxEnCours],GUILayout.Width (Screen.width * 0.35f*demi),GUILayout.Height (Screen.height * 0.25f*demi));
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/**/	GUILayout.EndVertical ();
			/*******************************************************************************/
			
            if (GestionFocus.JeuxEnPause)
            {
            /********* Zone pour les flèches et les boutons joueur et quitter inactive *****/
			/**/	GUILayout.BeginVertical (GUILayout.Width (Screen.width * 0.25f*demi));
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.35f*demi));
			/******/		GUILayout.FlexibleSpace(); 
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		GUILayout.Button ("",GUI.skin.customStyles[3],GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		GUILayout.Button ("Jouer");
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		GUILayout.Button ("",GUI.skin.customStyles[4],GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		GUILayout.Button ("Quitter");
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/**/	GUILayout.EndVertical ();
			/*******************************************************************************/
            }
            else
            { 
			/********* Zone pour les flèches et les boutons joueur et quitter **************/
			/**/	GUILayout.BeginVertical (GUILayout.Width (Screen.width * 0.25f*demi));
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.35f*demi));
			/******/		GUILayout.FlexibleSpace(); 
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		if (GUILayout.Button ("",GUI.skin.customStyles[3],GUILayout.Height (Screen.height * 0.1f*demi))) {
			/******/			if (NumJeuxEnCours <= 0) //permet d'aller au dernier jeu
			/******/				NumJeuxEnCours = NombreJeux-1;
			/******/			else
			/******/				NumJeuxEnCours--;
			/******/			}
			/******/		GUILayout.FlexibleSpace();
			/******/		if (GUILayout.Button ("Jouer")) {
			/******/			LancementJeu (NumJeuxEnCours);
			/******/			gameObject.GetComponent<GestionJeux>().enabled = false;
			/******/		}
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/****/		GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.1f*demi));
			/******/		GUILayout.FlexibleSpace();
			/******/		if (GUILayout.Button ("",GUI.skin.customStyles[4],GUILayout.Height (Screen.height * 0.1f*demi))) {
			/******/			if (NumJeuxEnCours >= NombreJeux-1) //permet de faire un nouveau tour des jeux
			/******/				NumJeuxEnCours = 0;
			/******/			else
			/******/				NumJeuxEnCours++;
			/******/			}
			/******/		GUILayout.FlexibleSpace();
			/******/		if (GUILayout.Button ("Quitter")) 
            /******/            QuitterMenu();
			/******/		GUILayout.FlexibleSpace();
			/****/		GUILayout.EndHorizontal ();
			/**/	GUILayout.EndVertical ();
			/*******************************************************************************/
            }
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();
		}
		/**
		 * 
		 *  
		 **/ 
		void parametre (float a,float b,float c, float d){
			X0ss = a;
			Y0ss = b;
			Wss = c;
			Hss = d;
		}
        /** 
		 * 
		 *  
		 **/
        void QuitterMenu()
        {
            GameObject.Find("Focus").GetComponent<GestionFocus>().enabled = false;
            camJeux.camera.enabled = false;
            camJoueur.camera.targetTexture = null;
            camJoueur.camera.enabled = true;
            /** Réactivation des scripts du Joueur *****/
            GameObject.Find("Joueur").GetComponent<MouseLook>().enabled = true;
            GameObject.Find("Joueur").GetComponent<CharacterController>().enabled = true;
            GameObject.Find("Joueur").GetComponent<FPSInputController>().enabled = true;
            /*******************************************/
            gameObject.GetComponent<GestionJeux>().enabled = false;
        }
		/**
		 * 
		 *  
		 **/
		void LancementJeu (int a) {
			//GameObject.Find ("CameraMenuJeu").SetActive = false;
			//GameObject.Find ("BS_Camera").camera.enabled = true;
            NewJeu = GameObject.Instantiate(GameObjectsJeux[a], new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            NewJeu.transform.parent = gameObject.transform;
            NewJeu.transform.localPosition = (new Vector3(6.11f, -3.95f, -2.85f));
            //GameObject.Find (GameObjectsJeux[a].ToString()).transform.localScale = new Vector3 (Screen.width * 0.1f * demi + Screen.width * demi, Screen.height * 0.1f * demi + Screen.height * demi / 2, 0);
		}
	}
}