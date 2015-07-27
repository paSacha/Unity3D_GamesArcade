using UnityEngine;
using System.Collections;

namespace Game2D
{
	public class Jeu_du_Morpion : MonoBehaviour
	{
		//---paramètres des boutons---
		private float posX ;
		private float posY ;
		private float TailleCase ;
		//----------------------------
		private bool quitter ;
		private int[] Tableau ;
		private bool Fin = false;
		private int CasesRestantes;
		private int rand = 0;
		private bool Occupe = false;
		private float tps_att = 0;
		private float tps_ref = 0;
		private int Niveau = 0;
		private int Taille;
		private int Taille1 = 3;
		private int Taille2 = 6;
		private int ScoreJoueur = 0;
		private int ScoreOrdinateur = 0;
		private int OldScoreJoueur = 0;
		private int OldScoreOrdinateur = 0;
		private int NonLibre = 3;
		private int NombreHitJoueur ;
		private int NombreHitOrdinateur;
		private string Joueur;
	
		// caractéristiques interface
	
		public GUIStyle  skinStyleSupportJeu;
		public GUIStyle  skinStyleExit ;
		public GUIStyle  skinStyleScore;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;
		private Color guiColor ;
		private float Largeur ;
		private float Hauteur ;
		private float EcartL ;
		private float EcartH ;
		private  float X  ;
		private float Y  ;
		private float Larg_But  ;
		private float Haut_But ;
		private Rect R_SupportJeu;
		private Rect R_SupportGo ;
		private Rect R_SupportScoreJoueur ;
		private Rect R_SupportScoreOrdinateur ;
		private Rect R_LabelOrdinateur ;
		private Rect R_LabelJoueur ;
		private Rect windowRect;
	
		//---Textures----------------------
		public Texture2D rond_tex ; //rond bleu
		public Texture2D croix_tex ; //croix rouge
		public Texture2D interdit_tex ; //carré plein noir
		public Texture2D defaut_tex ; //carré gris
		public Texture2D sablierH_tex ; //sablier horizontal
		public Texture2D sablierV_tex ; //sablier vertical
		public Texture2D sablier ; //sablier utilisé
		public Texture2D TextureCase ;
		//---------------------------------
	
		void Start ()
		//-------------
		{	
			quitter = false;
			Initialisations ();
		}
		void Update()
		{
			GestionSize ();
			Niveau = 2;
			if (Occupe) {
				tps_att += Time.deltaTime;
				if (tps_att - tps_ref >= 0.7) { 
					if (sablier == sablierV_tex)
						sablier = sablierH_tex;
					else
						sablier = sablierV_tex;
					tps_ref = tps_att;
				}
			}
		}
		//--------------------------------
		void GestionSize()
		{
			TailleCase = Screen.height / 8* GestionJeux.demi;

			Largeur = Screen.width * 0.78f* GestionJeux.demi;
			Hauteur = Screen.height * 0.90f* GestionJeux.demi;
			EcartL = 0.01f * Largeur;
			EcartH = 0.01f * Hauteur;
			Larg_But = (Screen.width* GestionJeux.demi - Largeur) / 2 - 2 * EcartL;
			Haut_But = Hauteur / 4;

            X = Screen.width * 0.1f * GestionJeux.demi + Screen.width * GestionJeux.demi;
			Y = Screen.height * 0.1f * GestionJeux.demi + Screen.height*GestionJeux.demi/2;

            float HauteurModal = Screen.height * 0.3f * GestionJeux.demi;
            float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
            windowRect = new Rect(X + (Largeur - LargeurModal) / 2, Y + (Hauteur - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

            posX = X + (-Taille * TailleCase + Largeur) / 2;
            posY = Y + (-Taille * TailleCase + Hauteur) / 2;

			R_SupportJeu = new Rect (X, Y, Largeur, Hauteur);
			R_SupportGo = new Rect (X + Largeur + EcartL, Y + Hauteur - Haut_But, Larg_But, Haut_But);
			R_SupportScoreJoueur = new Rect (X + Largeur + EcartL, Y, Larg_But, Haut_But);
			R_SupportScoreOrdinateur = new Rect (X + Largeur + EcartL, Y + 1.5f * Haut_But + EcartH, Larg_But, Haut_But);
			R_LabelJoueur = new Rect (X + Largeur + 2f * EcartL, Y + EcartH, Larg_But, Haut_But);
            R_LabelOrdinateur = new Rect(X + Largeur + 2f * EcartL, Y + 1.5f * Haut_But + 2f * EcartH, Larg_But, Haut_But);

			skinStyleScore.fontSize = (int)(Haut_But * 0.4f* GestionJeux.demi);
            skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f);
            skinStyleButtonModal.fontSize = (int)(Screen.height * 0.04f);
		}
		void OnGUI () 
		//--------------
		{

            if (GestionFocus.JeuxEnPause)
                InterfaceJeuPause();
            else
                InterfaceJeu();
		}
        /************/
        void InterfaceJeu()
        {
            float PositionX;
            float PositionY;
            int i;
            //Créer Bouton Exit
            if (GUI.Button(R_SupportGo, "", skinStyleExit))
                quitter = true;
            if (quitter)
                GUI.ModalWindow(0, windowRect, confirme_click, "Voulez vous vraiment quitter?", skinStyleWindowsModal);
            if (ScoreJoueur > OldScoreJoueur)
                GUI.ModalWindow(0, windowRect, Gestion_gagne, "Bravo vous avez gagné !", skinStyleWindowsModal);
            if (ScoreOrdinateur > OldScoreOrdinateur)
                GUI.ModalWindow(0, windowRect, Gestion_gagne, "L'ordinateur a gagné !", skinStyleWindowsModal);
            //Créer Zone résultat
            GUI.Label(R_SupportScoreJoueur, "J1 \n\n" + ScoreJoueur, skinStyleScore);
            GUI.Label(R_SupportScoreOrdinateur, "J2 \n\n" + ScoreOrdinateur, skinStyleScore);

            // Créer zone de jeux
            GUI.Box(R_SupportJeu, "", skinStyleSupportJeu);


            // ---------- Le joueur joue
            for ( i = 1; i <= Taille * Taille; i++)
            {

                //--------------gestion de l'affichage des lignes/colonnes----------------------

                PositionY = posY + ((i - 1) / Taille) * TailleCase;
                PositionX = posX + ((i - 1) % Taille) * TailleCase;
                //-----------------------------------------------------------------

                switch (Tableau[i])
                {
                    case 0:
                        TextureCase = defaut_tex;
                        break;
                    case 1:
                        TextureCase = croix_tex;
                        break;
                    case 2:
                        TextureCase = rond_tex;
                        break;
                    case 3:
                        TextureCase = interdit_tex;
                        break;
                    default:
                        break;
                }

                var BoutonCase = GUI.Button(new Rect(PositionX, PositionY, TailleCase, TailleCase), TextureCase);
                if (!Fin && Joueur == "Joueur" && BoutonCase && Tableau[i] == 0 && !Occupe)
                {
                    Tableau[i] = 1;
                    Joueur = "Ordinateur";
                    CasesRestantes--;
                }

            }

            if (Fin)
                return; // affiche mais ne joue plus

            //--------------l'ordinateur joue ------------------------

            if (CasesRestantes > 0 || !Occupe)
            {
                var Proposition = PropositionJeuContre();
                if (Joueur == "Ordinateur" && !Occupe)
                {
                    if (Proposition == 0)
                    {
                        do
                        {
                            rand = Random.Range(1, Taille * Taille + 1);
                        } while (Tableau[rand] != 0);

                    }
                    else
                        rand = Proposition;

                    Occupe = true;
                    tps_ref = 0;

                }

                //-------------------------------------------------------------------------------------------------

                //------gestion de l'attente de l'ordinateur------------------------------------
                if (Occupe)
                {
                    Vector3 positionSouris = Input.mousePosition;

                    GUI.DrawTexture(new Rect(positionSouris.x + TailleCase / 2, Screen.height - positionSouris.y - TailleCase / 2, TailleCase / 2, TailleCase / 2), sablier);
                    if (tps_att >= 0.1)
                    {	// durée de la réflexion de l'ordinateur
                        Occupe = false;
                        tps_att = 0;
                        Tableau[rand] = 2;
                        Joueur = "Joueur";
                        CasesRestantes--;
                    }

                }
            }

            //--------------vérification Fin de jeu--------------------------------------------
            NombreHitJoueur = TestHit(1);
            NombreHitOrdinateur = TestHit(2);
            if (!Fin)
            {
                if (((Niveau == 1 || Niveau == 2) && (NombreHitJoueur > 0 || NombreHitOrdinateur > 0)) ||
                    ((Niveau == 3 || Niveau == 4) && CasesRestantes == 0))
                {
                    Fin = true;
                    ScoreJoueur += NombreHitJoueur;
                    ScoreOrdinateur += NombreHitOrdinateur;
                }
            }
        }
        void InterfaceJeuPause()
        {
            float PositionX;
            float PositionY;
            int i;
            //Créer Bouton Exit
            GUI.Button(R_SupportGo, "", skinStyleExit);
            //Créer Zone résultat
            GUI.Label(R_SupportScoreJoueur, "J1 \n\n" + ScoreJoueur, skinStyleScore);
            GUI.Label(R_SupportScoreOrdinateur, "J2 \n\n" + ScoreOrdinateur, skinStyleScore);
            // Créer zone de jeux
            GUI.Box(R_SupportJeu, "", skinStyleSupportJeu);

            // ---------- Le joueur joue
            for (i = 1; i <= Taille * Taille; i++)
            {
                //--------------gestion de l'affichage des lignes/colonnes----------------------
                PositionY = posY + ((i - 1) / Taille) * TailleCase;
                PositionX = posX + ((i - 1) % Taille) * TailleCase;
                //-----------------------------------------------------------------
                switch (Tableau[i])
                {
                    case 0:
                        TextureCase = defaut_tex;
                        break;
                    case 1:
                        TextureCase = croix_tex;
                        break;
                    case 2:
                        TextureCase = rond_tex;
                        break;
                    case 3:
                        TextureCase = interdit_tex;
                        break;
                    default:
                        break;
                }

                var BoutonCase = GUI.Button(new Rect(PositionX, PositionY, TailleCase, TailleCase), TextureCase);
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
		void Gestion_gagne(int ID) 
		{
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button("Rejouer", skinStyleButtonModal))
            {
				Start ();
				OldScoreJoueur = ScoreJoueur;
				OldScoreOrdinateur = ScoreOrdinateur;
			}
            if (GUILayout.Button("Quitter", skinStyleButtonModal))
            {
				//réactive notre menu de jeu
				GameObject.Find ("MenuJeux").GetComponent<GestionJeux> ().enabled = true;
				//destruction du jeu instancié
				Destroy (gameObject);
			}
		}
		int TestHit (int Type)
		//--------------------------
		{
			int IndexLigne;
			int IndexColonne;
			int Index;
			int Compteur;
			int NbHit;
			int MinHit;
		
			if (Niveau == 1)
				MinHit = 3;
			else
				MinHit = 4;
			NbHit = 0;
		
		
			// Vérification en ligne
		
			for (IndexLigne=1; IndexLigne<=Taille; IndexLigne++) {
				Compteur = 0;
				for (IndexColonne=1; IndexColonne<=Taille; IndexColonne++) {
					if (Tableau [(IndexLigne - 1) * Taille + IndexColonne] == Type) {
						Compteur++;
						if (Compteur >= MinHit)
							NbHit++;
					} else
						Compteur = 0;
				}
			}
			// Vérification en colonne
		
			for (IndexColonne=1; IndexColonne<=Taille; IndexColonne++) {
				Compteur = 0;
				for (IndexLigne=1; IndexLigne<=Taille; IndexLigne++) {
					if (Tableau [(IndexLigne - 1) * Taille + IndexColonne] == Type) {
						Compteur++;
						if (Compteur >= MinHit)
							NbHit++;
					} else
						Compteur = 0;
				}
			}
		
			// Vérification en oblique montant
			for (IndexColonne=1-Taille+MinHit; IndexColonne<=(Taille-MinHit+1); IndexColonne++) {
				Compteur = 0;
				IndexLigne = Taille;
				for (Index=0; Index<=Taille-1; Index++) {
					if ((IndexLigne - Index - 1) <= Taille && (IndexColonne + Index) <= Taille && (IndexColonne + Index) > 0) {
						if (Tableau [(IndexLigne - 1 - Index) * Taille + IndexColonne + Index] == Type) {
							Compteur++;
							if (Compteur >= MinHit)
								NbHit++;
						} else
							Compteur = 0;
					}
				}
			}	
		
			// Vérification en oblique Descendant
			for (IndexColonne=1-Taille+MinHit; IndexColonne<=(Taille-MinHit+1); IndexColonne++) {
				Compteur = 0;
				IndexLigne = 1;
				for (Index=0; Index<=Taille-1; Index++) {
					if ((IndexLigne + Index - 1) <= Taille && (IndexColonne + Index) <= Taille && (IndexColonne + Index) > 0) {
						if (Tableau [(IndexLigne + Index - 1) * Taille + IndexColonne + Index] == Type) {
							Compteur++;
							if (Compteur >= MinHit)
								NbHit++;
						} else
							Compteur = 0;
					}
				}
			}
		
			return NbHit;
		}

		int PropositionJeuContre () // calcul du meilleur jeu pour l'ordinateur pour contrer le joueur
		//--------------------------
		{
			int IndexLigne;
			int IndexColonne;
			int Index;
			int Compteur;
			int MinHit;
			//var JoueurOrdinateur=2; // Code de l'ordinateur;
			int JoueurJoueur = 1; // Code du Joueur;
			int Vide = 0; // Code case vide
		
			if (Niveau == 1)
				MinHit = 2;
			else
				MinHit = 3;
			// Vérification en ligne
			for (IndexLigne=1; IndexLigne<=Taille; IndexLigne++) {
				Compteur = 0;
				for (IndexColonne=1; IndexColonne<Taille; IndexColonne++) {
					if (Tableau [(IndexLigne - 1) * Taille + IndexColonne] == JoueurJoueur) {
						Compteur++;
						if (Compteur >= MinHit && Tableau [(IndexLigne - 1) * Taille + IndexColonne + 1] == 0)
							return (IndexLigne - 1) * Taille + IndexColonne + 1;
					} else
						Compteur = 0;
				}
			}
			// Vérification en colonne
			for (IndexColonne=1; IndexColonne<=Taille; IndexColonne++) {
				Compteur = 0;
				for (IndexLigne=1; IndexLigne<Taille; IndexLigne++) {
					if (Tableau [(IndexLigne - 1) * Taille + IndexColonne] == JoueurJoueur) {
						Compteur++;
						if (Compteur >= MinHit && Tableau [(IndexLigne - 1 + 1) * Taille + IndexColonne] == 0)
							return (IndexLigne - 1 + 1) * Taille + IndexColonne;
					} else
						Compteur = 0;
				}
			}
		
			// Vérification en oblique montant
			for (IndexColonne=1-Taille+MinHit; IndexColonne<=(Taille-MinHit); IndexColonne++) {
				Compteur = 0;
				IndexLigne = Taille;
				for (Index=0; Index<Taille; Index++) {
					if ((IndexLigne - Index - 1) >= 1 && (IndexColonne + Index) < Taille && (IndexColonne + Index) > 0) {
						if (Tableau [(IndexLigne - Index - 1) * Taille + IndexColonne + Index] == JoueurJoueur) {
							Compteur++; 
							if (Compteur >= MinHit && Tableau [(IndexLigne - 1 - Index - 1) * Taille + IndexColonne + Index + 1] == Vide)
								return (IndexLigne - 1 - Index - 1) * Taille + IndexColonne + Index + 1;
						} else
							Compteur = 0;
					}
				}
			}	
		
			// Vérification en oblique Descendant
			for (IndexColonne=1-Taille+MinHit+1; IndexColonne<=(Taille-MinHit); IndexColonne++) {
				Compteur = 0;
				IndexLigne = 1;
				for (Index=0; Index<Taille; Index++) {
					if ((IndexLigne + Index) < Taille && (IndexColonne + Index) < Taille && (IndexColonne + Index) > 0) {
						if (Tableau [(IndexLigne + Index - 1) * Taille + IndexColonne + Index] == JoueurJoueur) {
							Compteur++;
							if (Compteur >= MinHit && Tableau [(IndexLigne + Index - 1 + 1) * Taille + IndexColonne + Index + 1] == Vide)
								return (IndexLigne + Index - 1 + 1) * Taille + IndexColonne + Index + 1;
						} else
							Compteur = 0;
					}
				}
			}
			return 0;
		}

		void Initialisations ()
		//------------------------
		{
			int i;
			Taille1 = 3;
			Taille2 = 6;
		
			if (Niveau == 1)
				Taille = Taille1;
			else
				Taille = Taille2;
		
			Tableau = new int[Taille * Taille + 1];
		
			for (i=1; i <= Taille*Taille; i++)
				Tableau [i] = 0;
		
			CasesRestantes = Taille * Taille;
		
			if (Niveau == 4) {// créer Taille cases non libres
				for (i=1; i <= Taille; i++) {
					do {
						rand = Random.Range (1, Taille * Taille + 1);
					} while(Tableau[rand]!=0);
					Tableau [rand] = NonLibre;
				}
				CasesRestantes -= Taille; // les cases interdites sont retirées des cases restantes
			}

			skinStyleSupportJeu.normal.background = (Texture2D)Resources.Load ("Textures/TapisBillard");
			skinStyleExit.normal.background = (Texture2D)Resources.Load ("Textures/Exit_1");
			skinStyleExit.hover.background = (Texture2D)Resources.Load ("Textures/Exit_2");
			skinStyleScore.normal.background = (Texture2D)Resources.Load ("Textures/FondKaki");

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            skinStyleButtonModal.normal.background = (Texture2D)Resources.Load("Textures/FondBlanc");
            skinStyleButtonModal.hover.background = (Texture2D)Resources.Load("Textures/unchecked");
            skinStyleButtonModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/

			skinStyleScore.alignment = TextAnchor.MiddleCenter;
		

		
			float CentreX;
			float CentreY;
			CentreX = X + Largeur / 2;
			CentreY = Y + Hauteur / 2;
		
			if (Hauteur < Largeur)
				TailleCase = Hauteur / (Taille + 1);
			else
				TailleCase = Largeur / (Taille + 1);
			posX = CentreX - TailleCase * Taille / 2;
			posY = CentreY - TailleCase * Taille / 2;	
		
			Joueur = "Joueur";
			Fin = false;
		}
		void OnEnable ()
		//----------------
		{     	
			guiColor = Color.white;
			ScoreJoueur = 0;
			ScoreOrdinateur = 0;
			Initialisations ();
		
		}
		void F_Initialisation (int[] Tab)
			//-----------------------------------------------
		{
			int i;
			for (i=1; i<=Taille*Taille; i++)
				Tableau [i] = Tab [i];
		
		}
		void F_TransfertElement (int Val, int Indice, int NbCases)
			//-----------------------------------------------
		{
			Tableau [Indice] = Val;	
			CasesRestantes = NbCases;
		}
	}
}