using UnityEngine;
using System.Collections;
using System.IO;

namespace Game2D
{
	public class Jeu_de_Lecture : MonoBehaviour
	{
		private Texture2D texture;
		private GameObject Joueur ;
		
		// -------- Styles Fichiers
		
		public GUIStyle skinStyleBut ;
		public GUIStyle skinStyle ;
		public GUIStyle skinStyleTexte2 ;
		public GUIStyle skinStyleBox ;
		public GUIStyle skinStyleTexte ;
		public GUIStyle skinStyleTexteReduit ;
		public GUIStyle skinStyleTexteElargi ;
		public GUIStyle skinStyleChamp ;
		public GUIStyle skinStyleChampReduit ;
		public GUIStyle skinStyleTexteCommentaire ;
		public GUIStyle skinStyleButton ;
		public GUIStyle skinStyleMonter ;
		public GUIStyle skinStyleDescendre ;
		public GUIStyle skinStyleMonterLarge ;
		public GUIStyle skinStyleDescendreLarge ;
		
		// -------- Styles Parametres généraux
		
		public GUIStyle skinStyleLabel ;
		public GUIStyle skinStyleLabelReduit ;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;
		
		// Caractéristiques des éléments d'affichage
		
		private float NbColonnes = 10;
		private float NbLignes = 20;
		private float Largeur ;
		private float Hauteur ;
		private float UniteX ;
		private float UniteY ;
		private float EcranL ;
		private float EcranH ;
		private float EcartL;
		private float EcartH;
		private float Larg_But  ;
		private float Haut_But  ;
		private float Larg_Ligne ;
		private float Haut_Ligne ;
		private float Larg_Type  ;
		private float Haut_Type  ;
		private float Larg_Choix  ;
		private float CoordX_Type  ;
		private float CoordY_Type  ;
		private float CoordX_Choix  ;
		private float CoordY_Choix  ;
		private Rect R_Type;
		private Rect R_Choix ;
		public GUIStyle skinStyleExit ;
		private bool quitter;
		private Rect windowRect;
		private float X ;
		private float Y ;

		// Support de choix 
		private float Larg_Support;
		private float Haut_Support;
		private float X_Support;
		private float Y_Support;
		private float TabH;
		private float TabV ;
		private Rect R_Support;
		private Rect R_SupportGo;
		private float Agrandissement ;
		private int NombreDePictogrammes ;
		private string NomDossierTextures;
		private string NomTextures ;
		private string Chemin ;
		private string[] FileLivres ;
		private Texture2D[] Livre ;
		private int NombreFichiers = 0;
		private int NombreLivres ;
		private System.IO.DirectoryInfo[] ListeLivres ;
		private string NomLivreEnCours ;
		private int Page ;
		private int NombrePages ;
		private int EtatTournePageFleche ;
		private int SensTournePageSouris ;
		private int SensTournePageFleche ;
		private float PositionTournePage ;
		private int MaxItem = 10;
		private bool ChoixFichierLectureActif = false;
		private int LectureActive = 0;
		private float Tempo1 ;
		private float MaxTempo ;
		private string OldPages = "first";
		
		void Start ()
			//---------------
		{
			quitter = false;
			Agrandissement = 1.5f;
			
			FileLivres = new string[50];
			Livre = new Texture2D[50];
			MaxTempo = 0.25f;

			Initialise ();
		}
		/****** Gestion générique pour fin d'un jeu *****/
		void confirme_click (int windowID)
		{
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button("Annuler", skinStyleButtonModal))
            {
				quitter = false;
			}
            if (GUILayout.Button("Quitter", skinStyleButtonModal))
            {
				//réactive notre menu de jeu
				GameObject.Find ("MenuJeux").GetComponent<GestionJeux> ().enabled = true;
				//destruction du jeu instancié
				Destroy (gameObject);
			}
		}
		/************************************************/	
		void Update ()
			//-----------------
		{
			Largeur = Screen.width * 0.95f*GestionJeux.demi ;
			Hauteur = Screen.height * 0.95f*GestionJeux.demi;
			
			UniteX = Largeur / NbColonnes;
			UniteY = Hauteur / NbLignes;
			Larg_Support = UniteX * 8;
			Haut_Support = UniteY * 19;
			X_Support = Screen.width * 0.1f*GestionJeux.demi+Screen.width*GestionJeux.demi;//UniteX * 2;
			Y_Support = Screen.height * 0.1f*GestionJeux.demi+Screen.height*GestionJeux.demi/2;//UniteY * 1;
			TabH = Larg_Support * 0.1f;
			TabV = Haut_Support * 0.1f;
			R_Support = new Rect (X_Support, Y_Support, Larg_Support, Haut_Support);
			R_SupportGo = new Rect (X_Support, Y_Support, Larg_Support/16f, Haut_Support/16f);

			X = Screen.width * 0.1f * GestionJeux.demi + Screen.width * GestionJeux.demi;
			Y = Screen.height * 0.1f*GestionJeux.demi+Screen.height*GestionJeux.demi/2;

            float HauteurModal = Screen.height * 0.3f * GestionJeux.demi;
            float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
            windowRect = new Rect(X + (Larg_Support - LargeurModal) / 2, Y + (Haut_Support - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

            skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f);
            skinStyleButtonModal.fontSize = (int)(Screen.height * 0.04f);

			// Définition des positions en fonction des variations de dimension de l'écran
			EcranL = Screen.width * 0.5f*GestionJeux.demi;
			EcranH = Screen.height * 0.5f*GestionJeux.demi; 
			
			Larg_But = EcranH / 20;
			Haut_But = Larg_But;
			
			EcartL = (Agrandissement - 1) * Larg_But;
			EcartH = EcartL;
			
			Haut_Type = 2 * Haut_But + 3 * EcartH;
			Larg_Type = Larg_But + 2 * EcartL;
			
			CoordX_Type = EcartL;
			CoordY_Type = EcranH - Haut_Type - 2 * EcartH;
			CoordX_Choix = CoordX_Type + Larg_But + 3 * EcartL;
			CoordY_Choix = CoordY_Type;
			
			Larg_Choix = (EcartL + Larg_But) * NombreDePictogrammes;
			
			R_Type = new Rect (CoordX_Type, CoordY_Type, Larg_Type, Haut_Type);
			R_Choix = new Rect (CoordX_Choix, CoordY_Choix, Larg_Choix, Haut_But + 2 * EcartH);
			
			NombreDePictogrammes = 5;

			skinStyleTexte.fixedWidth = Larg_Support * 0.6f;
			skinStyleTexteReduit.fixedWidth = Larg_Support * 0.08f;
			skinStyleTexteElargi.fixedWidth = Larg_Support * 0.6f;
			skinStyleTexteElargi.fixedHeight = Haut_Support * 0.2f;
			skinStyleChamp.fixedWidth = Larg_Support * 0.2f;
			skinStyleChampReduit.fixedWidth = Larg_Support * 0.05f;
			skinStyleTexteCommentaire.fixedWidth = Larg_Support * 0.6f;
			skinStyleTexteCommentaire.fixedHeight = Haut_Support * 0.4f;
			
			skinStyleButton.fixedWidth = Larg_Support * 0.15f;
			skinStyleMonterLarge.fixedWidth = Larg_Support * 0.6f;
			skinStyleDescendreLarge.fixedWidth = Larg_Support * 0.6f;
			Larg_Ligne = Larg_Support * 0.6f;
			Haut_Ligne = Haut_Support * 0.4f;
			
			skinStyleLabel.fixedWidth = UniteX * 2;
			skinStyleLabelReduit.fixedWidth = UniteX * 0.5f;

			skinStyleTexte.fontSize = (int)(Screen.height * 0.04);

		}

		void OnGUI () 
			//----------------
		{

            if (GestionFocus.JeuxEnPause)
            {
                // Choix du livre à lire			
                if (ChoixFichierLectureActif)
                {
                    GUI.Box(R_Support, "", skinStyleBox);
                    if (NomLivreEnCours != "")
                    {
                        LectureLivre(NomLivreEnCours);
                    }
                }

                if (NomLivreEnCours != "")
                {
                    LecturePause();
                }
            }
            else
            {

                Temporisation();// vitesse de défilement des items des fichiers

                // Activation de la temporisation
                Tempo1 += Time.deltaTime;

                //Activation du choix du livre				
                if (LectureActive == 0)
                {
                    LectureActive = 1;
                    ChoixFichierLectureActif = true;
                }


                // Choix du livre à lire			
                if (ChoixFichierLectureActif)
                {
                    GUI.Box(R_Support, "", skinStyleBox);
                    NomLivreEnCours = ChoixLivre();
                    if (NomLivreEnCours != "")
                    {
                        LectureLivre(NomLivreEnCours);
                    }
                }

                if (NomLivreEnCours != "")
                {
                    Lecture();
                }
                //Créer Bouton Exit
                if (GUI.Button(R_SupportGo, "", skinStyleExit))
                    quitter = true;
                if (quitter)
                    GUI.ModalWindow(0, windowRect, confirme_click, "Voulez vous vraiment quitter ?", skinStyleWindowsModal);
            }
		}		
		void Lecture ()
			//--------------------------
		{
			if (Page == 0)
				Page = 1;
			TournePage ();
			AfficheLivre (Page);
			
		}
        void LecturePause()
        //--------------------------
        {
            if (Page == 0)
                Page = 1;
            AfficheLivre(Page);

        }
		void TournePage ()
			//--------------------------
		{
			float Sensibilite = 100.0f;
			float Variation = 0;
			
			if (Input.GetAxis ("Horizontal") > 0) {
				SensTournePageFleche = 1;
				EtatTournePageFleche = 1;
			}
			if (Input.GetAxis ("Horizontal") < 0) {
				SensTournePageFleche = -1;
				EtatTournePageFleche = 1;
			}
			if (Input.GetAxis ("Horizontal") == 0 && EtatTournePageFleche == 1) {
				if (Page == 1)
					Page = 0;
				Page = Page + 2 * SensTournePageFleche;
				EtatTournePageFleche = 0;
			}
			
			
			if (Input.GetMouseButtonDown (0)) {
				PositionTournePage = Input.mousePosition.x;
			}
			if (Input.GetMouseButtonUp (0)) {
				Variation = Input.mousePosition.x - PositionTournePage;
				PositionTournePage = Input.mousePosition.x;
			}
			
			SensTournePageSouris = 0;
			if (Variation < -Sensibilite)
				SensTournePageSouris = 1;
			if (Variation > Sensibilite)
				SensTournePageSouris = -1;
			if (SensTournePageSouris != 0) {
				if (Page == 1)
					Page = 0;
				Page = Page + 2 * SensTournePageSouris;
				SensTournePageSouris = 0;
			}
			
			if (Page > NombrePages)
				Page = NombrePages;
			if (Page < 1)
				Page = 1;
			
		}

		void AfficheLivre (int Page)
			//--------------------------
		{
			Rect R_PageGauche = new Rect (X_Support, Y_Support, Larg_Support / 2.0f, Haut_Support);
			Rect R_PageDroite = new Rect (X_Support + Larg_Support / 2.0f, Y_Support, Larg_Support / 2.0f, Haut_Support);
			Rect R_PageCentre = new Rect (X_Support + Larg_Support / 4.0f, Y_Support, Larg_Support / 2.0f, Haut_Support);
			// page 1 correspond à la couverture
			// page 2 correspond à la première page intérieure
			
			if (Page == 1 || Page == NombrePages)
				GUI.DrawTexture (R_PageCentre, Livre [Page]);
			else {
				if (Page % 2 == 0) {
					GUI.DrawTexture (R_PageGauche, Livre [Page-1]);
					GUI.DrawTexture (R_PageDroite, Livre [Page + 1]);
				} else {
					GUI.DrawTexture (R_PageGauche, Livre [Page - 1]);
					GUI.DrawTexture (R_PageDroite, Livre [Page+1]);
				}
			}
		}

		void LectureLivre (string NomDuLivre) 
			//--------------------------
		{
			string Dossier;
			Dossier = Application.dataPath + "/Livres/" + NomDuLivre;
			WWW www = new WWW (Dossier);
			
			if (System.IO.Directory.Exists (Dossier)) {
				System.IO.DirectoryInfo info = new System.IO.DirectoryInfo (Dossier);
				FileInfo[] fileInfo = info.GetFiles ();
				
				NombrePages = 0;
				foreach (FileInfo file in fileInfo) {
					NombrePages++;
					www = new WWW ("file://" + Dossier + "/" + System.IO.Path.GetFileName ("" + file));
					if (!(System.IO.Path.GetFileName ("" + file) == (OldPages + ".meta"))) {
						Livre [NombrePages] = www.texture;
						Livre [NombrePages].name = System.IO.Path.GetFileName ("" + file);
					}
					OldPages = System.IO.Path.GetFileName ("" + file);
				}
				
			}
		}

		void DirectoryLivres ()
			//-----------------------------
		{
			// Mets le nom des livres dans la variable FileLivres - c'est également le nom des dossiers
			int i=1;
			
			System.IO.DirectoryInfo info = new System.IO.DirectoryInfo (Application.dataPath + "/Livres/");
			DirectoryInfo[] ListeLivres = info.GetDirectories ();
			
			foreach (DirectoryInfo Dir in ListeLivres)
				NombreLivres++;
			foreach (DirectoryInfo Dir in ListeLivres)
				FileLivres [i++] = Dir.Name as string; 
		}

		string ChoixLivre ()
			//-----------------------------
		{
			string Dossier;
			int Debut = 1;
			int Choix = 0;
			int i;
			
			Rect R_ChoixFichier;
			
			Dossier = Application.dataPath + "/Livres/";
			R_ChoixFichier = new Rect (X_Support + Larg_Support / 2 - Larg_Ligne / 2, Y_Support + Haut_Support / 2 - Haut_Ligne / 2, Larg_Ligne, MaxItem * Haut_Ligne);
			
			if (NombreLivres == 0) {
				GUI.Label (R_ChoixFichier, " Il n'y a pas de livre à disposition actuellement", skinStyleTexte2);
				return "";
			}
			
			// Ouvrir interface choix
			
			GUILayout.BeginArea (R_ChoixFichier, "");
			
			GUILayout.BeginVertical ();
			
			GUILayout.Button ("", skinStyleMonterLarge);
			if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition))
			if (Temporisation () && Debut > 1)
				Debut -= 1;
			for (i=1; i<=NombreLivres; i++) {
				if (GUILayout.Button (FileLivres [Debut + i - 1], skinStyleTexte)) {
					Choix = i + Debut - 1;
					ChoixFichierLectureActif = false;
				}									
			}
			GUILayout.Button ("", skinStyleDescendreLarge);
			if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition))
			if (Temporisation () && Debut <= (NombreFichiers - 10))
				Debut += 1; 
			
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
			
			
			if (Choix == 0)
				return "";
			else
				return FileLivres [Choix];
		}

		void Initialise ()
			//---------------------
		{
			// Fonds
			
			Texture2D TextureFond;
			Texture2D TextureActive;
			
			TextureFond = Resources.Load ("Textures/FondBlanc") as Texture2D;
			TextureActive = Resources.Load ("Textures/FondVert") as Texture2D;
			
			skinStyleBox.normal.background = Resources.Load ("Textures/Livre Ouvert") as Texture2D;
			
			skinStyleChamp.normal.background = TextureFond;
			skinStyleTexte.normal.background = TextureFond;
			skinStyleTexte.hover.background = TextureActive;
			skinStyleTexte.active.background = TextureActive;
			skinStyleTexteReduit.normal.background = TextureFond;
			skinStyleTexteReduit.hover.background = TextureActive;
			skinStyleTexteReduit.active.background = TextureActive;
			skinStyleMonter.normal.background = Resources.Load ("Textures/Monter") as Texture2D;
			skinStyleDescendre.normal.background = Resources.Load ("Textures/Descendre") as Texture2D;
			
			skinStyleMonterLarge.normal.background = Resources.Load ("Textures/Monter large") as Texture2D;
			skinStyleDescendreLarge.normal.background = Resources.Load ("Textures/Descendre large") as Texture2D;
			skinStyleTexteCommentaire.normal.background = TextureFond;
			skinStyleTexteCommentaire.hover.background = TextureActive;
			skinStyleTexteCommentaire.active.background = TextureActive;
			skinStyleTexteElargi.normal.background = TextureFond;
			skinStyleTexteElargi.hover.background = TextureActive;
			skinStyleTexteElargi.active.background = TextureActive;
			
			skinStyleButton.normal.background = TextureFond;
			skinStyleButton.hover.background = Resources.Load ("Textures/Bouton") as Texture2D;
			skinStyleButton.active.background = Resources.Load ("Textures/Bouton") as Texture2D;

            skinStyleExit.normal.background = Resources.Load("Textures/Exit_1_met") as Texture2D;
            skinStyleExit.hover.background = Resources.Load("Textures/Exit_2_met") as Texture2D;

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            skinStyleButtonModal.normal.background = (Texture2D)Resources.Load("Textures/FondBlanc");
            skinStyleButtonModal.hover.background = (Texture2D)Resources.Load("Textures/unchecked");
            skinStyleButtonModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/

			skinStyleTexte2.normal.background = Resources.Load ("Textures/Transparent") as Texture2D;

			skinStyleLabel.alignment = TextAnchor.MiddleLeft;
			
			skinStyleLabel.normal.textColor = Color.white;	 
			skinStyleLabelReduit.normal.textColor = Color.white;	
			skinStyle.normal.textColor = Color.white;
			skinStyleTexte2.normal.textColor = Color.black;
			
			
			skinStyle.alignment = TextAnchor.UpperCenter;
			skinStyleTexteReduit.alignment = TextAnchor.MiddleCenter;
			skinStyleChampReduit.alignment = TextAnchor.MiddleCenter;
			
			skinStyleButton.alignment = TextAnchor.MiddleCenter;
			
			DirectoryLivres ();
			NomLivreEnCours = "";
			EtatTournePageFleche = 0;
			Page = 0;
		}

		bool TestDansRectangle (Rect Rectangle)
			//--------------------------------
		{
			
			Vector3 PositionSouris = Input.mousePosition;
			if (
				(PositionSouris.x > (Rectangle.xMin)) 
				&& (PositionSouris.x < (Rectangle.xMax)) 
				&& (PositionSouris.y < (Screen.height - Rectangle.yMin)) 
				&& (PositionSouris.y > (Screen.height - Rectangle.yMax)) 
				)
				return true;
			else
				return false;
			
			
			
		}

		bool Temporisation ()
			//--------------
		{
			if (Tempo1 >= MaxTempo) {
				Tempo1 = 0;
				return true;
			} else
				return false;
		}

		void OnDisable ()
			//----------------
		{  
			LectureActive = 0;
			ChoixFichierLectureActif = false;
		}

		void OnEnable ()
			//----------------
		{     	
			NomLivreEnCours = "";
			LectureActive = 0;
			
		}
		//----------------------
		void F_LectureActive (int Valeur, bool Val2)
			//-----------------------------------------------
		{
			LectureActive = Valeur;
			ChoixFichierLectureActif = Val2;
		}
		
		void F_ChoixLivre (string Livre)
			//-----------------------------------------------
		{
			NomLivreEnCours = Livre;
		}
	}
}