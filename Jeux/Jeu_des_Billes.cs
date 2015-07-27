using UnityEngine;
using System.Collections;

namespace Game2D
{
	public class Jeu_des_Billes : MonoBehaviour
	{
		private int[] PresenceBilles ;
		private Vector3[] PositionCentreBilles ;
		private Vector3[] DirectionBilles ;
		private int NombreBilles ;
		private int NombreBillesMax ;
		private Texture2D[] TextureBilles ;
		private int Numero ;
		private int Niveau ;
		public AudioClip Explosion ;
	
		// caractéristiques interface
	
		public GUIStyle skinStyleSupportJeu;
		public GUIStyle skinStyleBille ;
		public GUIStyle skinStyleExit ;
		public GUIStyle skinStyleReste ;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;
		private Color guiColor;
		private float Largeur ;
		private float Hauteur ;
		private float Profondeur;
		private float EcartL ;
		private float EcartH ;
		private float X ;
		private float Y ;
		private float Larg_But ;
		private float Haut_But;
		private Rect R_SupportJeu;
		private Rect R_SupportGo;
		private Rect R_SupportReste ;
		private float BordTapis ;
		private float LargeurBille ;
		private float RayonBille ;
		private float Vitesse ;
		private float Type3D ;
		private Vector3 positionSouris ;
		private bool quitter;
		private Rect windowRect;
	
		void Start ()
		//---------------
		{
			int i;
			quitter = false;
			NombreBillesMax = 10;
			Profondeur = 1000;
			Niveau = 2;
			GestionSize ();

			TextureBilles = new Texture2D[NombreBillesMax + 1];
			for (i = 1; i<NombreBillesMax+1; i++)
				TextureBilles [i] = Resources.Load ("Textures/bubble") as Texture2D;
		
			PositionCentreBilles = new Vector3[NombreBillesMax + 1];
			PresenceBilles = new int[NombreBillesMax + 1];
			DirectionBilles = new Vector3[NombreBillesMax + 1];

			skinStyleSupportJeu.normal.background = Resources.Load ("Textures/TapisBillard") as Texture2D;
			skinStyleExit.normal.background = Resources.Load ("Textures/Exit_1") as Texture2D;
			skinStyleExit.hover.background = Resources.Load ("Textures/Exit_2") as Texture2D;
			skinStyleReste.normal.background = Resources.Load ("Textures/FondKaki") as Texture2D;
			skinStyleReste.alignment = TextAnchor.MiddleCenter;
		
			Initialisation ();
		}

		void Update () 
		//---------------
		{
			GestionSize ();
		}

		void GestionSize()
		{
			Largeur = Screen.width * 0.78f*GestionJeux.demi;
			Hauteur = Screen.height * 0.90f*GestionJeux.demi;
			EcartL = 0.01f * Largeur;
			EcartH = 0.01f * Hauteur;
			Larg_But = (Screen.width*GestionJeux.demi - Largeur) / 2 - 2 * EcartL;
			Haut_But = Hauteur / 4;

			X= Screen.width * 0.1f*GestionJeux.demi+Screen.width*GestionJeux.demi;
			Y = Screen.height * 0.1f*GestionJeux.demi+Screen.height*GestionJeux.demi/2;

			R_SupportJeu = new Rect (X, Y, Largeur, Hauteur);
			R_SupportGo = new Rect (X + Largeur + EcartL, Y + Hauteur - Haut_But, Larg_But, Haut_But);
			R_SupportReste = new Rect (X + Largeur + EcartL, Y, Larg_But, Haut_But);
           
            float HauteurModal = Screen.height * 0.3f*GestionJeux.demi;
            float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
            windowRect = new Rect(X + (Largeur - LargeurModal) / 2, Y + (Hauteur - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

            skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f);
            skinStyleButtonModal.fontSize = (int)(Screen.height * 0.04f);
			skinStyleReste.fontSize = (int)(Haut_But * 0.8f*GestionJeux.demi);

			BordTapis = Largeur / 60;
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
		void OnGUI ()  
		//-----------------
		{
            if (GestionFocus.JeuxEnPause)
            {
                //Créer Bouton Exit
                GUI.Button(R_SupportGo, "", skinStyleExit);

                GUI.Label(R_SupportReste, "" + NombreBillesRestantes(), skinStyleReste);
                // Créer zone de jeux
                GUI.Box(R_SupportJeu, "", skinStyleSupportJeu);
                // Déplacer billes en tant que bouton en position
                MiseEnPositionCentreBillesPause();
            }
            else
            {
                //Créer Bouton Exit
                if (GUI.Button(R_SupportGo, "", skinStyleExit))
                    quitter = true;
                if (quitter)
                    GUI.ModalWindow(0, windowRect, confirme_click, "Voulez vous vraiment quitter ?",skinStyleWindowsModal);
                if (NombreBillesRestantes() <= 0)
                    GUI.ModalWindow(0, windowRect, Gestion_gagne, "Bravo vous avez gagné !", skinStyleWindowsModal);
                //Créer Zone résultat
                GUI.Label(R_SupportReste, "" + NombreBillesRestantes(), skinStyleReste);

                // Créer zone de jeux
                GUI.Box(R_SupportJeu, "", skinStyleSupportJeu);

                // Déplacer billes en tant que bouton en position
                MiseEnPositionCentreBilles();
            }
		}
		/****** Gestion générique pour fin d'un jeu *****/
		void confirme_click (int windowID)
		{
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button("Recommencer", skinStyleButtonModal))
				Start ();
            if (GUILayout.Button("Quitter", skinStyleButtonModal))
            {
				//réactive notre menu de jeu
				GameObject.Find ("MenuJeux").GetComponent<GestionJeux> ().enabled = true;
				//destruction du jeu instancié
				Destroy (gameObject);
			}
		}
		/************************************************/	
		void MiseEnPositionCentreBilles ()
		//-----------------
		{
			Rect Pos;
			int i;
			int j;
			float Variation = 0.0f;
		
			// DeplacerBilles
			for (i=1; i<=NombreBilles; i++) {
				PositionCentreBilles [i].x += DirectionBilles [i].x;
				PositionCentreBilles [i].y += DirectionBilles [i].y;
				PositionCentreBilles [i].z += DirectionBilles [i].z;
			}
		
			// Gestion des collisions inter-billes
			for (i=1; i<=NombreBilles-1; i++)
				for (j=i+1; j<=NombreBilles; j++) {
					if (PresenceBilles [i] == 1 && PresenceBilles [j] == 1) {
						if (DistanceEntreBilles (PositionCentreBilles [i], PositionCentreBilles [j]) < LargeurBille) {
							DirectionBilles [i].x = -DirectionBilles [i].x;
							DirectionBilles [i].y = -DirectionBilles [i].y;
							DirectionBilles [i].z = -DirectionBilles [i].z;
							DirectionBilles [j].x = -DirectionBilles [j].x;
							DirectionBilles [j].y = -DirectionBilles [j].y;
							DirectionBilles [j].z = -DirectionBilles [j].z;
						}
					}
				}
		
			// Gestion collisions bord du jeu
			for (i=1; i<=NombreBilles; i++) {
				if (PositionCentreBilles [i].x > (Largeur - RayonBille - BordTapis))
					DirectionBilles [i].x = -DirectionBilles [i].x - Variation * Random.value;
				if (PositionCentreBilles [i].x < (RayonBille + BordTapis))
					DirectionBilles [i].x = -DirectionBilles [i].x + Variation * Random.value;
			
				if (PositionCentreBilles [i].y > (Hauteur - RayonBille - BordTapis))
					DirectionBilles [i].y = -DirectionBilles [i].y - Variation * Random.value;
				if (PositionCentreBilles [i].y < (RayonBille + BordTapis))
					DirectionBilles [i].y = -DirectionBilles [i].y + Variation * Random.value;
				if (PositionCentreBilles [i].z > (Profondeur))
					DirectionBilles [i].z = -DirectionBilles [i].z - Variation * Random.value;
				if (PositionCentreBilles [i].z < RayonBille / 2)
					DirectionBilles [i].z = -DirectionBilles [i].z + Variation * Random.value;
			
			}
		
			// PlacerBilles et vérifier si clic;
			for (i=1; i<=NombreBilles; i++) {
				if (PresenceBilles [i] == 1) {
					float Larg_Bille;
					if (Type3D == 1)
						Larg_Bille = LargeurBille * Type3D * PositionCentreBilles [i].z / Profondeur;
					else
						Larg_Bille = LargeurBille;
					Pos = new Rect (X + PositionCentreBilles [i].x - RayonBille, Y + PositionCentreBilles [i].y - RayonBille, Larg_Bille, Larg_Bille);
					skinStyleBille.normal.background = TextureBilles [1];
					if (GUI.Button (Pos, "", skinStyleBille))
						PresenceBilles [i] = 0;// Effacer la bille cliquée
				}
			}
		
		
		}
        void MiseEnPositionCentreBillesPause()
        //-----------------
        {
            Rect Pos;
            int i;

            // PlacerBilles et vérifier si clic;
            for (i = 1; i <= NombreBilles; i++)
            {
                if (PresenceBilles[i] == 1)
                {
                    float Larg_Bille;
                    if (Type3D == 1)
                        Larg_Bille = LargeurBille * Type3D * PositionCentreBilles[i].z / Profondeur;
                    else
                        Larg_Bille = LargeurBille;
                    Pos = new Rect(X + PositionCentreBilles[i].x - RayonBille, Y + PositionCentreBilles[i].y - RayonBille, Larg_Bille, Larg_Bille);
                    skinStyleBille.normal.background = TextureBilles[1];
                    GUI.Button(Pos, "", skinStyleBille);
                }
            }
        }
		int NombreBillesRestantes ()
		//-----------------
		{
			int Nombre = 0;
			int i;
			for (i=1; i<=NombreBilles; i++)
				if (PresenceBilles [i] == 1)
					Nombre++;
			return Nombre;
		}

		float DistanceEntreBilles (Vector3 Source, Vector3 But)
		//------------------------------
		{
			return (Mathf.Sqrt ((Source.x - But.x) * (Source.x - But.x) + (Source.y - But.y) * (Source.y - But.y) + (Source.z - But.z) * (Source.z - But.z)));
		}

		void Initialisation ()
		//--------------------------------
		{
			int i;
		
			switch (Niveau) {
			case  1:
				Niveau = 1;
				NombreBilles = 1;
				Vitesse = 0.5f;
				LargeurBille = Largeur / 7;
				Type3D = 0;
				break;
			case  2:
				Niveau = 2;
				NombreBilles = 5;
				Vitesse = 1;
				LargeurBille = Largeur / 7;
				Type3D = 0;
				break;
			case  3:
				Niveau = 3;
				NombreBilles = 5;
				Vitesse = 2;
				LargeurBille = Largeur / 15;
				Type3D = 0;
				break;
			case  4:
				Niveau = 4;
				NombreBilles = 5;
				Vitesse = 3;
				LargeurBille = Largeur / 7;
				Type3D = 0;
				break;
			default:
				break;
			}
		
			for (i=1; i<=NombreBilles; i++) {
				PresenceBilles [i] = 1;
			}
		
			//InitialiserTrajectoires;
			for (i=1; i<=NombreBilles; i++) {
				float Norme;
				DirectionBilles [i] = new Vector3 (Random.Range (-100, 100), Random.Range (-100, 100), Random.Range (-100, 100));
				Norme = DirectionBilles [i].x * DirectionBilles [i].x + DirectionBilles [i].y * DirectionBilles [i].y;
				Norme = Mathf.Sqrt (Norme);
				DirectionBilles [i].x = DirectionBilles [i].x / Norme * Vitesse;
				DirectionBilles [i].y = DirectionBilles [i].y / Norme * Vitesse;
				DirectionBilles [i].z = Vitesse * Type3D;
			}
			//MiseEnpositionInit par rapport à référentiel monde 0,0,0
			for (i=1; i<=NombreBilles; i++)
				do {
					PositionCentreBilles [i].x = (Largeur + (Largeur - 2 * LargeurBille) * (-1 + 2 * Random.value)) / 2;
					PositionCentreBilles [i].y = (Hauteur + (Hauteur - 2 * LargeurBille) * (-1 + 2 * Random.value)) / 2;
					PositionCentreBilles [i].z = Profondeur / 2 * Type3D;
				} while(DistanceMin(i)<LargeurBille);
		
			RayonBille = LargeurBille / 2;

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            skinStyleButtonModal.normal.background = (Texture2D)Resources.Load("Textures/FondBlanc");
            skinStyleButtonModal.hover.background = (Texture2D)Resources.Load("Textures/unchecked");
            skinStyleButtonModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/
		}

		float DistanceMin (int Num)
		//------------------------------
		{
			float Dist;
			float DistMin;
			int i;
		
			DistMin = Largeur;
			for (i=1; i<=Num-1; i++) {
				Dist = DistanceEntreBilles (PositionCentreBilles [Num], PositionCentreBilles [i]);
				if (Dist < DistMin)
					DistMin = Dist;
			}
			return DistMin;
		}

		void OnEnable ()
		//----------------
		{     	
			guiColor = Color.white;
		}
		//---------------------- 
		void F_PresenceBille (int Valeur, int index)
			//-----------------------------------------------
		{
			PresenceBilles [index] = Valeur;
		}
	}
}