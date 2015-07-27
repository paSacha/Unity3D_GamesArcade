using UnityEngine;
using System.Collections;

namespace Game2D
{
	public class Jeu_des_Lettres : MonoBehaviour
	{
		private int Resultat ;
		private int Niveau ;
	
		// caractéristiques interface
	
		public GUIStyle skinStyleSupportJeu ;
		public GUIStyle skinStyleBouton ;
		public GUIStyle skinStyleBoutonNeutre ;
		public GUIStyle skinStyleScore ;
		public GUIStyle skinStyleExit ;
		public GUIStyle skinStyleZoneMots ;
		public GUIStyle skinStyleTexte ;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;
		private Color guiColor ;
		private float Largeur ;
		private float Hauteur  ;
		private float Profondeur ;
		private float EcartL ;
		private float EcartH ;
		private float X ;
		private float Y ;
		private Rect R_SupportJeu;
		private Rect R_SupportGo ;
		private Rect R_SupportExit;
		private Rect R_SupportResultat ;
		private Rect R_ZoneMots ;
		private float BordTapis;
		private float Larg_Bouton;
		private float Haut_Bouton ;
		private float Larg_BoutonGo ;
		private Texture2D TextureBouton ;
		private Vector3 PositionCentreJeu ;
		private Vector3 PositionSouris  ;
		private bool SourisActive;
		private int ScoreJoueur ;
		private bool ActivePoints ;
		private float  TemporisationMax ;
		private float  Temporisation ;
		private bool quitter;
		private Rect windowRect;
		// variables du jeu
	
	
		private string[,] GrilleJeu ;
		private int NumJeu;
		private int NbGrillesNiveau1 ;
		private int NbGrillesNiveau2 ;
		private int NbGrillesNiveau3 ;
		private int NbGrillesNiveau4 ;
		private int[] NbLignesGrilleNiveau1;
		private int[] NbLignesGrilleNiveau2;
		private int[] NbLignesGrilleNiveau3;
		private int[] NbLignesGrilleNiveau4;
		private int[] NbColonnesGrilleNiveau1 ;
		private int[] NbColonnesGrilleNiveau2 ;
		private int[] NbColonnesGrilleNiveau3 ;
		private int[] NbColonnesGrilleNiveau4 ;
		private int[] NbMotsNiveau1 ;
		private int[] NbMotsNiveau2 ;
		private int[] NbMotsNiveau3 ;
		private int[] NbMotsNiveau4 ;
		private string[,] ListeDesMotsNiveau1 ;
		private string[,] ListeDesMotsNiveau2 ;
		private string[,] ListeDesMotsNiveau3 ;
		private string[,] ListeDesMotsNiveau4 ;
		private string[,] GrillesFinaleNiveau1 ;
		private string[,] GrillesFinaleNiveau2;
		private string[,] GrillesFinaleNiveau3 ;
		private string[,] GrillesFinaleNiveau4 ;
		private int NbGrilles ;
		private int[] NbLignesGrille;
		private int[] NbColonnesGrille ;
		private int[] NbMots ;
		private string[,] ListeDesMots ;
		private string[,] GrillesFinale;
		private string DebugList ;
		private string[] VecteurGrille ;
		private Vector2 BoutonActif ;
	
		void Start ()
		//---------------
		{
			quitter = false;
			Niveau = 2;
			windowRect = new Rect (Screen.width*GestionJeux.demi * 0.5f - 350 / 2, Screen.height*GestionJeux.demi * 0.5f - 150 / 2, 350, 150); //fenetre pour la fin du jeu

			ScoreJoueur = 0;
		
			GrilleJeu = new string[21, 21];
		
			NbGrillesNiveau1 = 8;
			NbGrillesNiveau2 = 8;
			NbGrillesNiveau3 = 8;
			NbGrillesNiveau4 = 8;
		
			NbLignesGrilleNiveau1 = new int[NbGrillesNiveau1 + 1];
			NbColonnesGrilleNiveau1 = new int[NbGrillesNiveau1 + 1];
			NbMotsNiveau1 = new int[10];
			ListeDesMotsNiveau1 = new string[11, 10];// Nombre de jeux +1,Nombre de jeux différents pour le niveau
			GrillesFinaleNiveau1 = new string[11, 15];// [nombre de jeux, Nombre de mots de la matrice]
		
			NbLignesGrilleNiveau2 = new int[NbGrillesNiveau2 + 1];
			NbColonnesGrilleNiveau2 = new int[NbGrillesNiveau2 + 1];
			NbMotsNiveau2 = new int[10];
			ListeDesMotsNiveau2 = new string[11, 10];// Nombre de jeux +1,Nombre de jeux différents pour le niveau
			GrillesFinaleNiveau2 = new string[11, 15];// [nombre de jeux, Nombre de mots de la matrice]
		
			NbLignesGrilleNiveau3 = new int[NbGrillesNiveau3 + 1];
			NbColonnesGrilleNiveau3 = new int[NbGrillesNiveau3 + 1];
			NbMotsNiveau3 = new int[10];
			ListeDesMotsNiveau3 = new string[11, 10];// Nombre de jeux +1,Nombre de jeux différents pour le niveau
			GrillesFinaleNiveau3 = new string[11, 15];// [nombre de jeux, Nombre de mots de la matrice]
		
			NbLignesGrilleNiveau4 = new int[NbGrillesNiveau4 + 1];
			NbColonnesGrilleNiveau4 = new int[NbGrillesNiveau4 + 1];
			NbMotsNiveau4 = new int[10];
			ListeDesMotsNiveau4 = new string[11, 10];// Nombre de jeux +1,Nombre de jeux différents pour le niveau
			GrillesFinaleNiveau4 = new string[11, 15];// [nombre de jeux, Nombre de mots de la matrice]
		
			Initialisation ();
			GestionSize ();
		}

		void Update () 
		//---------------
		{	
			GestionSize ();
		}
		void GestionSize() 
		//---------------------------------
		{
			Largeur = Screen.width*GestionJeux.demi * 0.78f;
			Hauteur = Screen.height*GestionJeux.demi * 0.90f;
			EcartL = 0.01f * Largeur;
			EcartH = 0.01f * Hauteur;
			
			X = Screen.width * 0.1f * GestionJeux.demi + Screen.width * GestionJeux.demi;
			Y = Screen.height * 0.1f*GestionJeux.demi+Screen.height*GestionJeux.demi/2;
			
			Larg_BoutonGo = Largeur / 10;

            float HauteurModal = Screen.height * 0.3f * GestionJeux.demi;
            float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
            windowRect = new Rect(X + (Largeur - LargeurModal) / 2, Y + (Hauteur - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

			R_SupportJeu = new Rect (X, Y, Largeur, Hauteur);
			R_ZoneMots = new Rect (X + Largeur * 3 / 4 + EcartL, Y + EcartH, Largeur / 4 - 3 * EcartL, Hauteur - 2 * EcartH);
			R_SupportResultat = new Rect (X + Largeur + EcartL, Y, Larg_BoutonGo, Larg_BoutonGo);

			//taille police d'écriture
			skinStyleBouton.fontSize = (int)(Haut_Bouton * 2 / 3);
			skinStyleTexte.fontSize = (int)((Largeur / 4 - 2 * EcartL) / NbColonnesGrille [NumJeu]);
			skinStyleScore.fontSize = (int)(Larg_BoutonGo * 2 / 3);
			skinStyleZoneMots.fontSize = (int)(Haut_Bouton * 0.8f);
            skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f);
            skinStyleButtonModal.fontSize = (int)(Screen.height * 0.04f);

			BordTapis = Largeur / 60;
			PositionCentreJeu.x = X + Largeur * 3 / 4 / 2;
			PositionCentreJeu.y = Y + Hauteur / 2;
			CalculDimentionBoutons ();
		}
		void OnGUI ()  
		//-----------------
		{
		
			PositionSouris = Input.mousePosition;
		
			// Créer zone de jeux
			GUI.Box (R_SupportJeu, "", skinStyleSupportJeu);
		
			//Créer Zone résultat
			GUI.Label (R_SupportResultat, "" + ScoreJoueur, skinStyleScore);

            if (GestionFocus.JeuxEnPause)
                AfficheJeuPause();
            else
                AfficheJeu();
		}
		void AfficheJeu ()
		//------------------
		{
			int Ligne;
			int Colonne;
			float PositionX;
			float PositionY;
			Rect R_Case;
			string Tempo;
		
		
			// Afficher Grille

            PositionX = PositionCentreJeu.x - (NbColonnesGrille [NumJeu] / 2.0f) * Larg_Bouton;
            PositionY = PositionCentreJeu.y - (NbLignesGrille[NumJeu] / 2.0f) * Haut_Bouton;
			for (Ligne=1; Ligne<=NbLignesGrille[NumJeu]; Ligne++)
				for (Colonne=1; Colonne<=NbColonnesGrille[NumJeu]; Colonne++) {
					R_Case = new Rect (PositionX + (Colonne - 1) * Larg_Bouton, PositionY + (Ligne - 1) * Haut_Bouton, Larg_Bouton, Haut_Bouton);
					if (GrilleJeu [Ligne, Colonne] == " ")
						GUI.Box (R_Case, GrilleJeu [Ligne, Colonne], skinStyleBoutonNeutre);// affiche bouton Neutre
			else
						GUI.Box (R_Case, GrilleJeu [Ligne, Colonne], skinStyleBouton);// affiche bouton
					if (TestDansRectangle (R_Case) && Input.GetMouseButtonDown (0) && GrilleJeu [Ligne, Colonne] != " ") {
						BoutonActif.x = Ligne;
						BoutonActif.y = Colonne;
						SourisActive = true;
					}
					if (SourisActive)
						GUI.Box (new Rect (PositionSouris.x - Larg_Bouton / 2, Screen.height - PositionSouris.y - Haut_Bouton / 2, Larg_Bouton, Haut_Bouton), GrilleJeu [(int)(BoutonActif.x), (int)(BoutonActif.y)], skinStyleBouton);	
					if (Input.GetMouseButtonUp (0) && SourisActive && TestDansRectangle (R_Case)) {
						// Déplacer bouton
						SourisActive = false;
						if (GrilleJeu [Ligne, Colonne] != " ") {
							Tempo = GrilleJeu [Ligne, Colonne];
							GrilleJeu [Ligne, Colonne] = GrilleJeu [(int)(BoutonActif.x), (int)(BoutonActif.y)];
							GrilleJeu [(int)(BoutonActif.x), (int)(BoutonActif.y)] = Tempo;
						}
					}
				}
		
		
			// Créer zone d'affichage des mots
			R_ZoneMots = new Rect (X + Largeur * 3 / 4 + EcartL, PositionY, Largeur / 4 - 3 * EcartL, Haut_Bouton * NbLignesGrille [NumJeu]);
			GUI.Box (R_ZoneMots, "", skinStyleZoneMots);
			GUILayout.BeginArea (R_ZoneMots, "");
			GUILayout.BeginVertical ();
			GUILayout.Label ("");
			for (Ligne=1; Ligne<=NbMots[NumJeu]; Ligne++)
				GUILayout.Label (" " + ListeDesMots [NumJeu, Ligne], skinStyleTexte);
		
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		
			if (TestGagne () == 0 && ActivePoints) {
				ScoreJoueur += Niveau + 2;
				ActivePoints = false;
			}
			if (ScoreJoueur > 0)
                GUI.ModalWindow(0, windowRect, Gestion_gagne, "Bravo vous avez gagné !", skinStyleWindowsModal);

			//Créer Bouton Go
			R_SupportExit = new Rect (X + Largeur + EcartL, Y + Hauteur - Larg_BoutonGo, Larg_BoutonGo, Larg_BoutonGo);
			if (GUI.Button (R_SupportExit, "", skinStyleExit)) {
				quitter = true;
			}
			if (quitter)
                GUI.ModalWindow(0, windowRect, confirme_click, "Voulez vous vraiment quitter?", skinStyleWindowsModal);
		
		}
        void AfficheJeuPause()
        //------------------
        {
            int Ligne;
            int Colonne;
            float PositionX;
            float PositionY;
            Rect R_Case;

            // Afficher Grille

            PositionX = PositionCentreJeu.x - (NbColonnesGrille[NumJeu] / 2.0f) * Larg_Bouton;
            PositionY = PositionCentreJeu.y - (NbLignesGrille[NumJeu] / 2.0f) * Haut_Bouton;
            for (Ligne = 1; Ligne <= NbLignesGrille[NumJeu]; Ligne++)
                for (Colonne = 1; Colonne <= NbColonnesGrille[NumJeu]; Colonne++)
                {
                    R_Case = new Rect(PositionX + (Colonne - 1) * Larg_Bouton, PositionY + (Ligne - 1) * Haut_Bouton, Larg_Bouton, Haut_Bouton);
                    if (GrilleJeu[Ligne, Colonne] == " ")
                        GUI.Box(R_Case, GrilleJeu[Ligne, Colonne], skinStyleBoutonNeutre);// affiche bouton Neutre
                    else
                        GUI.Box(R_Case, GrilleJeu[Ligne, Colonne], skinStyleBouton);// affiche bouton
                }


            // Créer zone d'affichage des mots
            R_ZoneMots = new Rect(X + Largeur * 3 / 4 + EcartL, PositionY, Largeur / 4 - 3 * EcartL, Haut_Bouton * NbLignesGrille[NumJeu]);
            GUI.Box(R_ZoneMots, "", skinStyleZoneMots);
            GUILayout.BeginArea(R_ZoneMots, "");
            GUILayout.BeginVertical();
            GUILayout.Label("");
            for (Ligne = 1; Ligne <= NbMots[NumJeu]; Ligne++)
                GUILayout.Label(" " + ListeDesMots[NumJeu, Ligne], skinStyleTexte);

            GUILayout.EndVertical();
            GUILayout.EndArea();


            //Créer Bouton Go
            R_SupportExit = new Rect(X + Largeur + EcartL, Y + Hauteur - Larg_BoutonGo, Larg_BoutonGo, Larg_BoutonGo);
            GUI.Button(R_SupportExit, "", skinStyleExit);
        }
		void Gestion_gagne(int ID) 
		{
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button("Rejouer", skinStyleButtonModal))
            {
				Start ();
			}
            if (GUILayout.Button("Quitter", skinStyleButtonModal))
            {
				//réactive notre menu de jeu
				GameObject.Find ("MenuJeux").GetComponent<GestionJeux> ().enabled = true;
				//destruction du jeu instancié
				Destroy (gameObject);
			}
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

		void Initialisation ()
		//--------------------------------
		{	
			switch (Niveau) {
			case  1:
				NbLignesGrille = NbLignesGrilleNiveau1;
				NbColonnesGrille = NbColonnesGrilleNiveau1;
				NbMots = NbMotsNiveau1;
				ListeDesMots = ListeDesMotsNiveau1;
				GrillesFinale = GrillesFinaleNiveau1;
				InitialisationDesMotsNiveau1 ();
				NbGrilles = NbGrillesNiveau1;
				break;
			case  2:
				NbLignesGrille = NbLignesGrilleNiveau2;
				NbColonnesGrille = NbColonnesGrilleNiveau2;
				NbMots = NbMotsNiveau2;
				ListeDesMots = ListeDesMotsNiveau2;
				GrillesFinale = GrillesFinaleNiveau2;
				InitialisationDesMotsNiveau2 ();
				NbGrilles = NbGrillesNiveau2;
				break;
			case  3:
				NbLignesGrille = NbLignesGrilleNiveau3;
				NbColonnesGrille = NbColonnesGrilleNiveau3;
				NbMots = NbMotsNiveau3;
				ListeDesMots = ListeDesMotsNiveau3;
				GrillesFinale = GrillesFinaleNiveau3;
				InitialisationDesMotsNiveau3 ();
				NbGrilles = NbGrillesNiveau3;
				break;
			case  4:
				NbLignesGrille = NbLignesGrilleNiveau4;
				NbColonnesGrille = NbColonnesGrilleNiveau4;
				NbMots = NbMotsNiveau4;
				ListeDesMots = ListeDesMotsNiveau4;
				GrillesFinale = GrillesFinaleNiveau4;
				InitialisationDesMotsNiveau4 ();
				NbGrilles = NbGrillesNiveau4;
				break;
			default:
				break;
			}
		
			NumJeu = (int)(Random.Range (100.0f, (NbGrilles + 1) * 100.0f - 1) / 100.0f);
			InitialisationGrilleJeu ();	
			CalculDimentionBoutons ();
		
			skinStyleSupportJeu.normal.background = Resources.Load ("Textures/TapisBillard") as Texture2D;
			skinStyleScore.normal.background = Resources.Load ("Textures/FondKaki") as Texture2D;

			skinStyleScore.alignment = TextAnchor.MiddleCenter;
			skinStyleScore.normal.textColor = Color.black;
			skinStyleScore.fontStyle = FontStyle.Italic;
			skinStyleZoneMots.normal.background = Resources.Load ("Textures/Bouton2") as Texture2D;
			skinStyleBouton.normal.background = Resources.Load ("Textures/Bouton1") as Texture2D;
			skinStyleBouton.hover.background = Resources.Load ("Textures/Bouton1 Actif") as Texture2D;
			skinStyleBouton.alignment = TextAnchor.MiddleCenter;
			skinStyleTexte.normal.textColor = Color.white;
			skinStyleBoutonNeutre.normal.background = Resources.Load ("Textures/Bouton 2") as Texture2D;
			skinStyleBoutonNeutre.alignment = TextAnchor.MiddleCenter;
			skinStyleZoneMots.alignment = TextAnchor.MiddleCenter;

            skinStyleExit.normal.background = Resources.Load("Textures/Exit_1") as Texture2D;
            skinStyleExit.hover.background = Resources.Load("Textures/Exit_2") as Texture2D;

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            skinStyleButtonModal.normal.background = (Texture2D)Resources.Load("Textures/FondBlanc");
            skinStyleButtonModal.hover.background = (Texture2D)Resources.Load("Textures/unchecked");
            skinStyleButtonModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/

			TextureBouton = Resources.Load ("Textures/Bleu") as Texture2D;
		

		
		
			BoutonActif.x = 0;
			BoutonActif.y = 0;
			SourisActive = false;
			ActivePoints = true;
			}

		void InitialisationDesMotsNiveau1 ()
		//-----------------------------
		{
			int Index = 0;
		
		
			// Grille 1
			Index++;
			NbLignesGrilleNiveau1 [Index] = 6;
			NbColonnesGrilleNiveau1 [Index] = 5;
			NbMotsNiveau1 [Index] = 3;
		
		
			ListeDesMotsNiveau1 [Index, 1] = "VERT";
			ListeDesMotsNiveau1 [Index, 2] = "ROUGE";
			ListeDesMotsNiveau1 [Index, 3] = "GRIS";
		
		
			GrillesFinaleNiveau1 [Index, 1] = " V    ";
			GrillesFinaleNiveau1 [Index, 2] = " E    ";
			GrillesFinaleNiveau1 [Index, 3] = " ROUGE";
			GrillesFinaleNiveau1 [Index, 4] = " T  R ";
			GrillesFinaleNiveau1 [Index, 5] = "    I ";
			GrillesFinaleNiveau1 [Index, 6] = "    S ";
		
			// Grille 2
			Index++;
			NbLignesGrilleNiveau1 [Index] = 4;
			NbColonnesGrilleNiveau1 [Index] = 5;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "CARRE";
			ListeDesMotsNiveau1 [Index, 2] = "CUBE";
			ListeDesMotsNiveau1 [Index, 3] = "ROND";
		
			GrillesFinaleNiveau1 [Index, 1] = " CARRE";
			GrillesFinaleNiveau1 [Index, 2] = " U  O ";
			GrillesFinaleNiveau1 [Index, 3] = " B  N ";
			GrillesFinaleNiveau1 [Index, 4] = " E  D ";
		
			// Grille 3
			Index++;
			NbLignesGrilleNiveau1 [Index] = 8;
			NbColonnesGrilleNiveau1 [Index] = 8;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "CAMION";
			ListeDesMotsNiveau1 [Index, 2] = "GRUE";
			ListeDesMotsNiveau1 [Index, 3] = "TRACTEUR";
		
			GrillesFinaleNiveau1 [Index, 1] = "    T    ";
			GrillesFinaleNiveau1 [Index, 2] = "    R    ";
			GrillesFinaleNiveau1 [Index, 3] = "   CAMION";
			GrillesFinaleNiveau1 [Index, 4] = "    C    ";
			GrillesFinaleNiveau1 [Index, 5] = "    T    ";
			GrillesFinaleNiveau1 [Index, 6] = " GRUE    ";
			GrillesFinaleNiveau1 [Index, 7] = "    U    ";
			GrillesFinaleNiveau1 [Index, 8] = "    R    ";
		
			// Grille 4
			Index++;
			NbLignesGrilleNiveau1 [Index] = 7;
			NbColonnesGrilleNiveau1 [Index] = 7;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "RENAULT";
			ListeDesMotsNiveau1 [Index, 2] = "PEUGEOT";
			ListeDesMotsNiveau1 [Index, 3] = "AUDI";
		
			GrillesFinaleNiveau1 [Index, 1] = "  P     ";
			GrillesFinaleNiveau1 [Index, 2] = " RENAULT";
			GrillesFinaleNiveau1 [Index, 3] = "  U U   ";
			GrillesFinaleNiveau1 [Index, 4] = "  G D   ";
			GrillesFinaleNiveau1 [Index, 5] = "  E I   ";
			GrillesFinaleNiveau1 [Index, 6] = "  O     ";
			GrillesFinaleNiveau1 [Index, 7] = "  T     ";
		
			// Grille 5
			Index++;
			NbLignesGrilleNiveau1 [Index] = 8;
			NbColonnesGrilleNiveau1 [Index] = 8;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "CAMION";
			ListeDesMotsNiveau1 [Index, 2] = "GRUE";
			ListeDesMotsNiveau1 [Index, 3] = "TRACTEUR";
		
			GrillesFinaleNiveau1 [Index, 1] = "    T    ";
			GrillesFinaleNiveau1 [Index, 2] = "    R    ";
			GrillesFinaleNiveau1 [Index, 3] = "   CAMION";
			GrillesFinaleNiveau1 [Index, 4] = "    C    ";
			GrillesFinaleNiveau1 [Index, 5] = "    T    ";
			GrillesFinaleNiveau1 [Index, 6] = " GRUE    ";
			GrillesFinaleNiveau1 [Index, 7] = "    U    ";
			GrillesFinaleNiveau1 [Index, 8] = "    R    ";
		
		
			// Grille 6
			Index++;
			NbLignesGrilleNiveau1 [Index] = 6;
			NbColonnesGrilleNiveau1 [Index] = 10;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "PAQUERETTE";
			ListeDesMotsNiveau1 [Index, 2] = "MUGUET";
			ListeDesMotsNiveau1 [Index, 3] = "ROSE";
		
			GrillesFinaleNiveau1 [Index, 1] = "    M      ";
			GrillesFinaleNiveau1 [Index, 2] = " PAQUERETTE";
			GrillesFinaleNiveau1 [Index, 3] = "    G      ";
			GrillesFinaleNiveau1 [Index, 4] = "    U      ";
			GrillesFinaleNiveau1 [Index, 5] = " ROSE      ";
			GrillesFinaleNiveau1 [Index, 6] = "    T      ";
		
		
			// Grille 7
			Index++;
			NbLignesGrilleNiveau1 [Index] = 6;
			NbColonnesGrilleNiveau1 [Index] = 10;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "CHAISE";
			ListeDesMotsNiveau1 [Index, 2] = "TABLE";
			ListeDesMotsNiveau1 [Index, 3] = "ARMOIRE";
		
			GrillesFinaleNiveau1 [Index, 1] = "       C   ";
			GrillesFinaleNiveau1 [Index, 2] = "       H   ";
			GrillesFinaleNiveau1 [Index, 3] = "      TABLE";
			GrillesFinaleNiveau1 [Index, 4] = "       I   ";
			GrillesFinaleNiveau1 [Index, 5] = "       S   ";
			GrillesFinaleNiveau1 [Index, 6] = " ARMOIRE   ";
		
			// Grille 8
			Index++;
			NbLignesGrilleNiveau1 [Index] = 8;
			NbColonnesGrilleNiveau1 [Index] = 7;
			NbMotsNiveau1 [Index] = 3;
		
			ListeDesMotsNiveau1 [Index, 1] = "EPICEA";
			ListeDesMotsNiveau1 [Index, 2] = "PIN";
			ListeDesMotsNiveau1 [Index, 3] = "CERISIER";
		
			GrillesFinaleNiveau1 [Index, 1] = "  C     ";
			GrillesFinaleNiveau1 [Index, 2] = "  EPICEA";
			GrillesFinaleNiveau1 [Index, 3] = "  R     ";
			GrillesFinaleNiveau1 [Index, 4] = "  I     ";
			GrillesFinaleNiveau1 [Index, 5] = "  S     ";
			GrillesFinaleNiveau1 [Index, 6] = " PIN    ";
			GrillesFinaleNiveau1 [Index, 7] = "  E     ";
			GrillesFinaleNiveau1 [Index, 8] = "  R     ";
		
		}

		void InitialisationDesMotsNiveau2 ()
		//-----------------------------
		{
			int Index = 0;
		
			// Grille 1
			Index++;
			NbLignesGrilleNiveau2 [Index] = 6;
			NbColonnesGrilleNiveau2 [Index] = 6;
			NbMotsNiveau2 [Index] = 4;
		
		
			ListeDesMotsNiveau2 [Index, 1] = "CANARD";
			ListeDesMotsNiveau2 [Index, 2] = "CHEVAL";
			ListeDesMotsNiveau2 [Index, 3] = "VACHE";
			ListeDesMotsNiveau2 [Index, 4] = "COQ";
		
		
			GrillesFinaleNiveau2 [Index, 1] = " CHEVAL";
			GrillesFinaleNiveau2 [Index, 2] = " A  A  ";
			GrillesFinaleNiveau2 [Index, 3] = " N  COQ";
			GrillesFinaleNiveau2 [Index, 4] = " A  H  ";
			GrillesFinaleNiveau2 [Index, 5] = " R  E  ";
			GrillesFinaleNiveau2 [Index, 6] = " D     ";
		
			// Grille 2
			Index++;
			NbLignesGrilleNiveau2 [Index] = 7;
			NbColonnesGrilleNiveau2 [Index] = 8;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "LILLE";
			ListeDesMotsNiveau2 [Index, 2] = "NICE";
			ListeDesMotsNiveau2 [Index, 3] = "LOURDE";
			ListeDesMotsNiveau2 [Index, 4] = "BORDEAUX";
		
			GrillesFinaleNiveau2 [Index, 1] = "     N   ";
			GrillesFinaleNiveau2 [Index, 2] = "    LILLE";
			GrillesFinaleNiveau2 [Index, 3] = "     C O ";
			GrillesFinaleNiveau2 [Index, 4] = " BORDEAUX";
			GrillesFinaleNiveau2 [Index, 5] = "       R ";
			GrillesFinaleNiveau2 [Index, 6] = "       D ";
			GrillesFinaleNiveau2 [Index, 7] = "       E ";
		
			// Grille 3
			Index++;
			NbLignesGrilleNiveau2 [Index] = 7;
			NbColonnesGrilleNiveau2 [Index] = 7;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "MARS";
			ListeDesMotsNiveau2 [Index, 2] = "JUILLET";
			ListeDesMotsNiveau2 [Index, 3] = "JUIN";
			ListeDesMotsNiveau2 [Index, 4] = "AVRIL";
		
			GrillesFinaleNiveau2 [Index, 1] = "   MARS ";
			GrillesFinaleNiveau2 [Index, 2] = "    V   ";
			GrillesFinaleNiveau2 [Index, 3] = "    R   ";
			GrillesFinaleNiveau2 [Index, 4] = "  J I   ";
			GrillesFinaleNiveau2 [Index, 5] = " JUILLET";
			GrillesFinaleNiveau2 [Index, 6] = "  I     ";
			GrillesFinaleNiveau2 [Index, 7] = "  N     ";
		
		
			// Grille 4
			Index++;
			NbLignesGrilleNiveau2 [Index] = 7;
			NbColonnesGrilleNiveau2 [Index] = 8;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "NORVEGE";
			ListeDesMotsNiveau2 [Index, 2] = "ESPAGNE";
			ListeDesMotsNiveau2 [Index, 3] = "POLOGNE";
			ListeDesMotsNiveau2 [Index, 4] = "FRANCE";
		
			GrillesFinaleNiveau2 [Index, 1] = "  NORVEGE";
			GrillesFinaleNiveau2 [Index, 2] = "      S  ";
			GrillesFinaleNiveau2 [Index, 3] = "      P  ";
			GrillesFinaleNiveau2 [Index, 4] = "      A  ";
			GrillesFinaleNiveau2 [Index, 5] = "  POLOGNE";
			GrillesFinaleNiveau2 [Index, 6] = "      N  ";
			GrillesFinaleNiveau2 [Index, 7] = " FRANCE  ";
		
			// Grille 5
			Index++;
			NbLignesGrilleNiveau2 [Index] = 8;
			NbColonnesGrilleNiveau2 [Index] = 11;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "NAPOLEON";
			ListeDesMotsNiveau2 [Index, 2] = "VOLTAIRE";
			ListeDesMotsNiveau2 [Index, 3] = "RICHELIEU";
			ListeDesMotsNiveau2 [Index, 4] = "COLOCHE";
		
			GrillesFinaleNiveau2 [Index, 1] = "    V  C    ";
			GrillesFinaleNiveau2 [Index, 2] = " NAPOLEON   ";
			GrillesFinaleNiveau2 [Index, 3] = "    L  L    ";
			GrillesFinaleNiveau2 [Index, 4] = "    T  U    ";
			GrillesFinaleNiveau2 [Index, 5] = "    A  C    ";
			GrillesFinaleNiveau2 [Index, 6] = "   RICHELIEU";
			GrillesFinaleNiveau2 [Index, 7] = "    R  E    ";
			GrillesFinaleNiveau2 [Index, 8] = "    E       ";
		
		
			// Grille 6
			Index++;
			NbLignesGrilleNiveau2 [Index] = 12;
			NbColonnesGrilleNiveau2 [Index] = 10;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "PATISSIER";
			ListeDesMotsNiveau2 [Index, 2] = "ARCHITECTE";
			ListeDesMotsNiveau2 [Index, 3] = "INSTITUTEUR";
			ListeDesMotsNiveau2 [Index, 4] = "MEDECIN";
		
			GrillesFinaleNiveau2 [Index, 1] = " P     M   ";
			GrillesFinaleNiveau2 [Index, 2] = " ARCHITECTE";
			GrillesFinaleNiveau2 [Index, 3] = " T   N D   ";
			GrillesFinaleNiveau2 [Index, 4] = " I   S E   ";
			GrillesFinaleNiveau2 [Index, 5] = " S   T C   ";
			GrillesFinaleNiveau2 [Index, 6] = " S   I I   ";
			GrillesFinaleNiveau2 [Index, 7] = " I   T N   ";
			GrillesFinaleNiveau2 [Index, 8] = " E   U     ";
			GrillesFinaleNiveau2 [Index, 9] = " R   T     ";
			GrillesFinaleNiveau2 [Index, 10] = "     E     ";
			GrillesFinaleNiveau2 [Index, 11] = "     U     ";
			GrillesFinaleNiveau2 [Index, 12] = "     R     ";
		
		
			// Grille 7
			Index++;
			NbLignesGrilleNiveau2 [Index] = 11;
			NbColonnesGrilleNiveau2 [Index] = 8;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "TROUSSE";
			ListeDesMotsNiveau2 [Index, 2] = "CALCULER";
			ListeDesMotsNiveau2 [Index, 3] = "LECTURE";
			ListeDesMotsNiveau2 [Index, 4] = "VOCABULAIRE";
		
			GrillesFinaleNiveau2 [Index, 1] = "   V   L ";
			GrillesFinaleNiveau2 [Index, 2] = " TROUSSE ";
			GrillesFinaleNiveau2 [Index, 3] = "   C   C ";
			GrillesFinaleNiveau2 [Index, 4] = "   A   T ";
			GrillesFinaleNiveau2 [Index, 5] = "   B   U ";
			GrillesFinaleNiveau2 [Index, 6] = "   U   R ";
			GrillesFinaleNiveau2 [Index, 7] = " CALCULER";
			GrillesFinaleNiveau2 [Index, 8] = "   A     ";
			GrillesFinaleNiveau2 [Index, 9] = "   I     ";
			GrillesFinaleNiveau2 [Index, 10] = "   R     ";
			GrillesFinaleNiveau2 [Index, 11] = "   E     ";
		
			// Grille 8
			Index++;
			NbLignesGrilleNiveau2 [Index] = 10;
			NbColonnesGrilleNiveau2 [Index] = 9;
			NbMotsNiveau2 [Index] = 4;
		
			ListeDesMotsNiveau2 [Index, 1] = "ZEBRE";
			ListeDesMotsNiveau2 [Index, 2] = "GAZELLE";
			ListeDesMotsNiveau2 [Index, 3] = "DROMADAIRE";
			ListeDesMotsNiveau2 [Index, 4] = "ELEPHANT";
		
			GrillesFinaleNiveau2 [Index, 1] = "      D   ";
			GrillesFinaleNiveau2 [Index, 2] = "  G   R   ";
			GrillesFinaleNiveau2 [Index, 3] = "  A   O   ";
			GrillesFinaleNiveau2 [Index, 4] = "  Z   M   ";
			GrillesFinaleNiveau2 [Index, 5] = " ELEPHANT ";
			GrillesFinaleNiveau2 [Index, 6] = "  L   D   ";
			GrillesFinaleNiveau2 [Index, 7] = "  L   A   ";
			GrillesFinaleNiveau2 [Index, 8] = "  E   I   ";
			GrillesFinaleNiveau2 [Index, 9] = "      R   ";
			GrillesFinaleNiveau2 [Index, 10] = "     ZEBRE";
		
		}

		void InitialisationDesMotsNiveau3 ()
		//-----------------------------
		{
			int Index = 0;
		
			// Grille 1
			Index++;
			NbLignesGrilleNiveau3 [Index] = 11;
			NbColonnesGrilleNiveau3 [Index] = 13;
			NbMotsNiveau3 [Index] = 5;
		
		
			ListeDesMotsNiveau3 [Index, 1] = "DIRECTION";
			ListeDesMotsNiveau3 [Index, 2] = "RETARD";
			ListeDesMotsNiveau3 [Index, 3] = "DESTINATION";
			ListeDesMotsNiveau3 [Index, 4] = "ARRET";
			ListeDesMotsNiveau3 [Index, 5] = "TICKET";
		
		
			GrillesFinaleNiveau3 [Index, 1] = "     DIRECTION";
			GrillesFinaleNiveau3 [Index, 2] = "     E E      ";
			GrillesFinaleNiveau3 [Index, 3] = "     S T      ";
			GrillesFinaleNiveau3 [Index, 4] = " ARRET A      ";
			GrillesFinaleNiveau3 [Index, 5] = "     I R      ";
			GrillesFinaleNiveau3 [Index, 6] = "     N D      ";
			GrillesFinaleNiveau3 [Index, 7] = "     A        ";
			GrillesFinaleNiveau3 [Index, 8] = "     TICKET   ";
			GrillesFinaleNiveau3 [Index, 9] = "     I        ";
			GrillesFinaleNiveau3 [Index, 10] = "     O        ";
			GrillesFinaleNiveau3 [Index, 11] = "     N        ";
		
			// Grille 2
			Index++;
			NbLignesGrilleNiveau3 [Index] = 11;
			NbColonnesGrilleNiveau3 [Index] = 8;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "BANANE";
			ListeDesMotsNiveau3 [Index, 2] = "ANANAS";
			ListeDesMotsNiveau3 [Index, 3] = "MANGUE";
			ListeDesMotsNiveau3 [Index, 4] = "POIRE";
			ListeDesMotsNiveau3 [Index, 5] = "RAISIN";
		
		
			GrillesFinaleNiveau3 [Index, 1] = "       B ";
			GrillesFinaleNiveau3 [Index, 2] = "       A ";
			GrillesFinaleNiveau3 [Index, 3] = "     M N ";
			GrillesFinaleNiveau3 [Index, 4] = "   ANANAS";
			GrillesFinaleNiveau3 [Index, 5] = "     N N ";
			GrillesFinaleNiveau3 [Index, 6] = "   R G E ";
			GrillesFinaleNiveau3 [Index, 7] = "   A U   ";
			GrillesFinaleNiveau3 [Index, 8] = " POIRE   ";
			GrillesFinaleNiveau3 [Index, 9] = "   S     ";
			GrillesFinaleNiveau3 [Index, 10] = "   I     ";
			GrillesFinaleNiveau3 [Index, 11] = "   N     ";
		
			// Grille 3
			Index++;
			NbLignesGrilleNiveau3 [Index] = 10;
			NbColonnesGrilleNiveau3 [Index] = 8;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "DAUPHIN";
			ListeDesMotsNiveau3 [Index, 2] = "DINOSAURE";
			ListeDesMotsNiveau3 [Index, 3] = "FAUTEUIL";
			ListeDesMotsNiveau3 [Index, 4] = "CHAUSSURE";
			ListeDesMotsNiveau3 [Index, 5] = "AUTO";
		
			GrillesFinaleNiveau3 [Index, 1] = "    A C  ";
			GrillesFinaleNiveau3 [Index, 2] = "  DAUPHIN";
			GrillesFinaleNiveau3 [Index, 3] = "  I T A  ";
			GrillesFinaleNiveau3 [Index, 4] = "  N O U  ";
			GrillesFinaleNiveau3 [Index, 5] = "  O   S  ";
			GrillesFinaleNiveau3 [Index, 6] = "  S   S  ";
			GrillesFinaleNiveau3 [Index, 7] = " FAUTEUIL";
			GrillesFinaleNiveau3 [Index, 8] = "  U   R  ";
			GrillesFinaleNiveau3 [Index, 9] = "  R   E  ";
			GrillesFinaleNiveau3 [Index, 10] = "  E      ";
		
			// Grille 4
			Index++;
			NbLignesGrilleNiveau3 [Index] = 7;
			NbColonnesGrilleNiveau3 [Index] = 9;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "LIVRE";
			ListeDesMotsNiveau3 [Index, 2] = "VELO";
			ListeDesMotsNiveau3 [Index, 3] = "CHOCOLAT";
			ListeDesMotsNiveau3 [Index, 4] = "LAPIN";
			ListeDesMotsNiveau3 [Index, 5] = "PILE";
		
			GrillesFinaleNiveau3 [Index, 1] = "   LIVRE  ";
			GrillesFinaleNiveau3 [Index, 2] = "     E    ";
			GrillesFinaleNiveau3 [Index, 3] = "     L    ";
			GrillesFinaleNiveau3 [Index, 4] = " CHOCOLAT ";
			GrillesFinaleNiveau3 [Index, 5] = "      A   ";
			GrillesFinaleNiveau3 [Index, 6] = "      PILE";
			GrillesFinaleNiveau3 [Index, 7] = "      I   ";
			GrillesFinaleNiveau3 [Index, 8] = "      N   ";
		
			// Grille 5
			Index++;
			NbLignesGrilleNiveau3 [Index] = 11;
			NbColonnesGrilleNiveau3 [Index] = 9;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "POMME";
			ListeDesMotsNiveau3 [Index, 2] = "MONTAGNE";
			ListeDesMotsNiveau3 [Index, 3] = "TOMATE";
			ListeDesMotsNiveau3 [Index, 4] = "FANTOME";
			ListeDesMotsNiveau3 [Index, 5] = "GOMME";
		
		
			GrillesFinaleNiveau3 [Index, 1] = "         F";
			GrillesFinaleNiveau3 [Index, 2] = "         A";
			GrillesFinaleNiveau3 [Index, 3] = "         N";
			GrillesFinaleNiveau3 [Index, 4] = " POMME   T";
			GrillesFinaleNiveau3 [Index, 5] = "    O    O";
			GrillesFinaleNiveau3 [Index, 6] = "    N    M";
			GrillesFinaleNiveau3 [Index, 7] = "    TOMATE";
			GrillesFinaleNiveau3 [Index, 8] = "    A     ";
			GrillesFinaleNiveau3 [Index, 9] = "    G     ";
			GrillesFinaleNiveau3 [Index, 10] = "    N     ";
			GrillesFinaleNiveau3 [Index, 11] = "    E     ";
		
		
			// Grille 6
			Index++;
			NbLignesGrilleNiveau3 [Index] = 13;
			NbColonnesGrilleNiveau3 [Index] = 10;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "CIGOGNE";
			ListeDesMotsNiveau3 [Index, 2] = "AGNEAU";
			ListeDesMotsNiveau3 [Index, 3] = "CHAMPIGNON";
			ListeDesMotsNiveau3 [Index, 4] = "CHAMPAGNE";
			ListeDesMotsNiveau3 [Index, 5] = "CHIGNON";
		
			GrillesFinaleNiveau3 [Index, 1] = "       C   ";
			GrillesFinaleNiveau3 [Index, 2] = "       I   ";
			GrillesFinaleNiveau3 [Index, 3] = " CHAMPIGNON";
			GrillesFinaleNiveau3 [Index, 4] = "       O   ";
			GrillesFinaleNiveau3 [Index, 5] = "    CHIGNON";
			GrillesFinaleNiveau3 [Index, 6] = "    H  N   ";
			GrillesFinaleNiveau3 [Index, 7] = "    AGNEAU ";
			GrillesFinaleNiveau3 [Index, 8] = "    M      ";
			GrillesFinaleNiveau3 [Index, 9] = "    P      ";
			GrillesFinaleNiveau3 [Index, 10] = "    A      ";
			GrillesFinaleNiveau3 [Index, 11] = "    G      ";
			GrillesFinaleNiveau3 [Index, 12] = "    N      ";
			GrillesFinaleNiveau3 [Index, 13] = "    E      ";
		
		
		
			// Grille 7
			Index++;
			NbLignesGrilleNiveau3 [Index] = 8;
			NbColonnesGrilleNiveau3 [Index] = 9;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "PULL";
			ListeDesMotsNiveau3 [Index, 2] = "BUCHE";
			ListeDesMotsNiveau3 [Index, 3] = "LUGE";
			ListeDesMotsNiveau3 [Index, 4] = "VOITURE";
			ListeDesMotsNiveau3 [Index, 5] = "COSTUME";
		
			GrillesFinaleNiveau3 [Index, 1] = "       B  ";
			GrillesFinaleNiveau3 [Index, 2] = "    C PULL";
			GrillesFinaleNiveau3 [Index, 3] = "    O  C U";
			GrillesFinaleNiveau3 [Index, 4] = "    S  H G";
			GrillesFinaleNiveau3 [Index, 5] = " VOITURE E";
			GrillesFinaleNiveau3 [Index, 6] = "    U     ";
			GrillesFinaleNiveau3 [Index, 7] = "    M     ";
			GrillesFinaleNiveau3 [Index, 8] = "    E     ";
		
			// Grille 8
			Index++;
			NbLignesGrilleNiveau3 [Index] = 8;
			NbColonnesGrilleNiveau3 [Index] = 9;
			NbMotsNiveau3 [Index] = 5;
		
			ListeDesMotsNiveau3 [Index, 1] = "COIFFEUSE";
			ListeDesMotsNiveau3 [Index, 2] = "ETOILE";
			ListeDesMotsNiveau3 [Index, 3] = "ARMOIRE";
			ListeDesMotsNiveau3 [Index, 4] = "VOITURE";
			ListeDesMotsNiveau3 [Index, 5] = "ROI";
		
			GrillesFinaleNiveau3 [Index, 1] = "  V       ";
			GrillesFinaleNiveau3 [Index, 2] = " COIFFEUSE";
			GrillesFinaleNiveau3 [Index, 3] = "  I       ";
			GrillesFinaleNiveau3 [Index, 4] = " ETOILE   ";
			GrillesFinaleNiveau3 [Index, 5] = "  U       ";
			GrillesFinaleNiveau3 [Index, 6] = " ARMOIRE  ";
			GrillesFinaleNiveau3 [Index, 7] = "  E  O    ";
			GrillesFinaleNiveau3 [Index, 8] = "     I    ";
		
		}

		void InitialisationDesMotsNiveau4 ()
		//-----------------------------
		{
			int Index = 0;
		
			// Grille 1
			Index++;
			NbLignesGrilleNiveau4 [Index] = 8;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
		
			ListeDesMotsNiveau4 [Index, 1] = "TREIZE";
			ListeDesMotsNiveau4 [Index, 2] = "CERISE";
			ListeDesMotsNiveau4 [Index, 3] = "DOUZE";
			ListeDesMotsNiveau4 [Index, 4] = "OISEAU";
			ListeDesMotsNiveau4 [Index, 5] = "CISEAUX";
			ListeDesMotsNiveau4 [Index, 6] = "CHEMISE";
		
		
			GrillesFinaleNiveau4 [Index, 1] = "      T   ";
			GrillesFinaleNiveau4 [Index, 2] = "    CERISE";
			GrillesFinaleNiveau4 [Index, 3] = "    H E   ";
			GrillesFinaleNiveau4 [Index, 4] = "    E I D ";
			GrillesFinaleNiveau4 [Index, 5] = "    M Z O ";
			GrillesFinaleNiveau4 [Index, 6] = "   CISEAUX";
			GrillesFinaleNiveau4 [Index, 7] = "    S   Z ";
			GrillesFinaleNiveau4 [Index, 8] = " OISEAU E ";
		
			// Grille 2
			Index++;
			NbLignesGrilleNiveau4 [Index] = 8;
			NbColonnesGrilleNiveau4 [Index] = 8;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "RENARD";
			ListeDesMotsNiveau4 [Index, 2] = "ROBINET";
			ListeDesMotsNiveau4 [Index, 3] = "PIRATE";
			ListeDesMotsNiveau4 [Index, 4] = "CAMERA";
			ListeDesMotsNiveau4 [Index, 5] = "ROBE";
			ListeDesMotsNiveau4 [Index, 6] = "RADIS";
		
			GrillesFinaleNiveau4 [Index, 1] = "  RENARD ";
			GrillesFinaleNiveau4 [Index, 2] = "  O   O  ";
			GrillesFinaleNiveau4 [Index, 3] = "  B C B  ";
			GrillesFinaleNiveau4 [Index, 4] = " PIRATE  ";
			GrillesFinaleNiveau4 [Index, 5] = "  N M    ";
			GrillesFinaleNiveau4 [Index, 6] = "  E E    ";
			GrillesFinaleNiveau4 [Index, 7] = "  T RADIS";
			GrillesFinaleNiveau4 [Index, 8] = "    A    ";
		
		
			// Grille 3
			Index++;
			NbLignesGrilleNiveau4 [Index] = 11;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "POMME";
			ListeDesMotsNiveau4 [Index, 2] = "MONTAGNE";
			ListeDesMotsNiveau4 [Index, 3] = "TOMATE";
			ListeDesMotsNiveau4 [Index, 4] = "GOMME";
			ListeDesMotsNiveau4 [Index, 5] = "MAIN";
			ListeDesMotsNiveau4 [Index, 6] = "ARMOIRE";
		
			GrillesFinaleNiveau4 [Index, 1] = "  A       ";
			GrillesFinaleNiveau4 [Index, 2] = "  R       ";
			GrillesFinaleNiveau4 [Index, 3] = "  M       ";
			GrillesFinaleNiveau4 [Index, 4] = " POMME    ";
			GrillesFinaleNiveau4 [Index, 5] = "  I O     ";
			GrillesFinaleNiveau4 [Index, 6] = "  R N     ";
			GrillesFinaleNiveau4 [Index, 7] = "  E TOMATE";
			GrillesFinaleNiveau4 [Index, 8] = "    A     ";
			GrillesFinaleNiveau4 [Index, 9] = "    GOMME ";
			GrillesFinaleNiveau4 [Index, 10] = " MAIN     ";
			GrillesFinaleNiveau4 [Index, 11] = "    E     ";
		
		
			// Grille 4
			Index++;
			NbLignesGrilleNiveau4 [Index] = 9;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "BANANE";
			ListeDesMotsNiveau4 [Index, 2] = "BIBERON";
			ListeDesMotsNiveau4 [Index, 3] = "BICHE";
			ListeDesMotsNiveau4 [Index, 4] = "ROBINET";
			ListeDesMotsNiveau4 [Index, 5] = "BEURRE";
			ListeDesMotsNiveau4 [Index, 6] = "ROBE";
		
			GrillesFinaleNiveau4 [Index, 1] = " BANANE   ";
			GrillesFinaleNiveau4 [Index, 2] = " I        ";
			GrillesFinaleNiveau4 [Index, 3] = " BICHE    ";
			GrillesFinaleNiveau4 [Index, 4] = " E    B   ";
			GrillesFinaleNiveau4 [Index, 5] = " ROBINET  ";
			GrillesFinaleNiveau4 [Index, 6] = " O    U   ";
			GrillesFinaleNiveau4 [Index, 7] = " N    R   ";
			GrillesFinaleNiveau4 [Index, 8] = "      ROBE";
			GrillesFinaleNiveau4 [Index, 9] = "      E   ";
		
			// Grille 5
			Index++;
			NbLignesGrilleNiveau4 [Index] = 10;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "CAROTTE";
			ListeDesMotsNiveau4 [Index, 2] = "LOCOMOTIVE";
			ListeDesMotsNiveau4 [Index, 3] = "CADEAU";
			ListeDesMotsNiveau4 [Index, 4] = "COLLIER";
			ListeDesMotsNiveau4 [Index, 5] = "BARQUE";
			ListeDesMotsNiveau4 [Index, 6] = "QUILLE";
		
			GrillesFinaleNiveau4 [Index, 1] = "    L     ";
			GrillesFinaleNiveau4 [Index, 2] = " CAROTTE B";
			GrillesFinaleNiveau4 [Index, 3] = " A  C    A";
			GrillesFinaleNiveau4 [Index, 4] = " D COLLIER";
			GrillesFinaleNiveau4 [Index, 5] = " E  M    Q";
			GrillesFinaleNiveau4 [Index, 6] = " A  O    U";
			GrillesFinaleNiveau4 [Index, 7] = " U  T    E";
			GrillesFinaleNiveau4 [Index, 8] = "  QUILLE  ";
			GrillesFinaleNiveau4 [Index, 9] = "    V     ";
			GrillesFinaleNiveau4 [Index, 10] = "    E     ";
		
		
		
			// Grille 6
			Index++;
			NbLignesGrilleNiveau4 [Index] = 9;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "GALETTE";
			ListeDesMotsNiveau4 [Index, 2] = "GORILLE";
			ListeDesMotsNiveau4 [Index, 3] = "GARCON";
			ListeDesMotsNiveau4 [Index, 4] = "ESCARGOT";
			ListeDesMotsNiveau4 [Index, 5] = "GUITARE";
			ListeDesMotsNiveau4 [Index, 6] = "VAGUE";
		
			GrillesFinaleNiveau4 [Index, 1] = " GALETTE  ";
			GrillesFinaleNiveau4 [Index, 2] = " O        ";
			GrillesFinaleNiveau4 [Index, 3] = " R  GARCON";
			GrillesFinaleNiveau4 [Index, 4] = " I  U     ";
			GrillesFinaleNiveau4 [Index, 5] = " L  I V   ";
			GrillesFinaleNiveau4 [Index, 6] = " L  T A   ";
			GrillesFinaleNiveau4 [Index, 7] = " ESCARGOT ";
			GrillesFinaleNiveau4 [Index, 8] = "    R U   ";
			GrillesFinaleNiveau4 [Index, 9] = "    E E   ";
		
		
			// Grille 7
			Index++;
			NbLignesGrilleNiveau4 [Index] = 11;
			NbColonnesGrilleNiveau4 [Index] = 8;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "LAPIN";
			ListeDesMotsNiveau4 [Index, 2] = "SAPIN";
			ListeDesMotsNiveau4 [Index, 3] = "CINQ";
			ListeDesMotsNiveau4 [Index, 4] = "PEINTURE";
			ListeDesMotsNiveau4 [Index, 5] = "TIMBRE";
			ListeDesMotsNiveau4 [Index, 6] = "MAIN";
		
			GrillesFinaleNiveau4 [Index, 1] = "       C ";
			GrillesFinaleNiveau4 [Index, 2] = "    L  I ";
			GrillesFinaleNiveau4 [Index, 3] = "   SAPIN ";
			GrillesFinaleNiveau4 [Index, 4] = "    P  Q ";
			GrillesFinaleNiveau4 [Index, 5] = "    I    ";
			GrillesFinaleNiveau4 [Index, 6] = " PEINTURE";
			GrillesFinaleNiveau4 [Index, 7] = "     I   ";
			GrillesFinaleNiveau4 [Index, 8] = "     MAIN";
			GrillesFinaleNiveau4 [Index, 9] = "     B   ";
			GrillesFinaleNiveau4 [Index, 10] = "     R   ";
			GrillesFinaleNiveau4 [Index, 11] = "     E   ";
		
		
			// Grille 8
			Index++;
			NbLignesGrilleNiveau4 [Index] = 8;
			NbColonnesGrilleNiveau4 [Index] = 9;
			NbMotsNiveau4 [Index] = 6;
		
			ListeDesMotsNiveau4 [Index, 1] = "SALADE";
			ListeDesMotsNiveau4 [Index, 2] = "CARTABLE";
			ListeDesMotsNiveau4 [Index, 3] = "AVION";
			ListeDesMotsNiveau4 [Index, 4] = "BANANE";
			ListeDesMotsNiveau4 [Index, 5] = "CADENAS";
			ListeDesMotsNiveau4 [Index, 6] = "TASSE";
		
			GrillesFinaleNiveau4 [Index, 1] = "  S       ";
			GrillesFinaleNiveau4 [Index, 2] = " CARTABLE ";
			GrillesFinaleNiveau4 [Index, 3] = "  L   A   ";
			GrillesFinaleNiveau4 [Index, 4] = "  AVION  T";
			GrillesFinaleNiveau4 [Index, 5] = "  D   A  A";
			GrillesFinaleNiveau4 [Index, 6] = "  E   N  S";
			GrillesFinaleNiveau4 [Index, 7] = "   CADENAS";
			GrillesFinaleNiveau4 [Index, 8] = "         E";
		
		}

		void InitialisationGrilleJeu ()
		//-----------------------------
		{
			int L1;
			int C1;
			int C2;
			int Index = 1;
			string StringTempo;
			int NbLettres = 0;
		
			// Création d'un vecteur avec toutes les lettres;
			for (L1=1; L1<=NbMots[NumJeu]; L1++)
				NbLettres += ListeDesMots [NumJeu, L1].Length;
			VecteurGrille = new string[NbLettres];
			NbLettres = 1;
			for (L1=1; L1<=NbLignesGrille[NumJeu]; L1++)
				for (C1=0; C1<GrillesFinale[NumJeu,L1].Length; C1++) {
					string Mot;
					Mot = GrillesFinale [NumJeu, L1].Substring (C1, 1);
					if (Mot != " ") {
						NbLettres++;
						VecteurGrille [Index++] = Mot;
					}
				}	
			// Mélange des lettres dans le vecteur
			for (Index=1; Index<=100; Index++) {
				C1 = Random.Range (1, NbLettres);
				C2 = Random.Range (1, NbLettres);
				StringTempo = VecteurGrille [C1];
				VecteurGrille [C1] = VecteurGrille [C2];
				VecteurGrille [C2] = StringTempo;
			}
		
			// Placement des lettres dans la grille de jeu
			Index = 1;
			for (L1=1; L1<=NbLignesGrille[NumJeu]; L1++)
				for (C1=1; C1<=NbColonnesGrille[NumJeu]; C1++)
					if (GrillesFinale [NumJeu, L1].Substring (C1, 1) != " ") {
						GrilleJeu [L1, C1] = VecteurGrille [Index];
						Index++;
					} else	
						GrilleJeu [L1, C1] = " ";
		
		}

		int TestGagne ()
		//-------------------
		{
			int Ligne;
			int Colonne;
			int Resultat;
		
		
			Resultat = 0;
			for (Ligne=1; Ligne<=NbLignesGrille[NumJeu]; Ligne++)
				for (Colonne=1; Colonne<=NbColonnesGrille[NumJeu]; Colonne++) {
					if (GrilleJeu [Ligne, Colonne] != GrillesFinale [NumJeu, Ligne].Substring (Colonne, 1))
						Resultat++;
				}
			return Resultat;
		}

		bool TestDansRectangle (Rect Rectangle)
		//--------------------------------
		{
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

		void CalculDimentionBoutons ()
		//-------------------
		{
			Larg_Bouton = (Largeur - 6 * EcartL) * 3 / 4 / NbColonnesGrille [NumJeu]; // 3/4 pour laisser de la place aux mots
			Haut_Bouton = (Hauteur - 6 * EcartH) / NbLignesGrille [NumJeu];
			if (Larg_Bouton < Haut_Bouton)
				Haut_Bouton = Larg_Bouton;
			else
				Larg_Bouton = Haut_Bouton;
		
		}

		void OnEnable ()
		//----------------
		{     	
			guiColor = Color.white;
			Start ();
		
		}
		//----------------------

	}
}