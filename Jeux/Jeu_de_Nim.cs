using UnityEngine;
using System.Collections;

namespace Game2D {
	public class Jeu_de_Nim : MonoBehaviour
	{
		private float X0ss;
		private float Y0ss;
		private float Wss;
		private float Hss;

		private int Score_joueur;
		private int Score_ordi;
		private int level;
		private int nb_Allumettes;
		private int nb_Allumettes_prises;
		private float allumette_largeur;
		private float allumette_longueur;
		private int cas ;
		private bool confirmation;
		private bool[] tab_allumette;
		private string texte_affiché;
		private int nNb_Allmtt_HeighLight = 0;
		private bool blocage ;
        private float X;
        private float Y;
		private Rect windowRect;

		public Texture Allumette_Texture;
		public GUISkin skin_Allumettes;
        public GUIStyle skinStyleWindowsModal;
		/**
		 * 
		 *  
		 **/ 
		void Start ()
		{
			//charge les différentes textures ou skin
			Allumette_Texture = (Texture2D) (Resources.Load ("Textures/allumettes"));
			skin_Allumettes = (GUISkin)(Resources.Load ("Skins/SkinNim"));

			allumette_largeur = Screen.width*0.015f; //15
			allumette_longueur = Screen.height*0.20f; //100

			nb_Allumettes = 15;
			nb_Allumettes_prises = 0;
			
			Score_ordi = 0;
			Score_joueur = 0;
			cas = 0;
			confirmation = false;
			blocage = false;

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/

			texte_affiché = "Choisissez 1, 2, ou 3 allumettes \n puis terminer votre tour";

			level = 10; // 0 pas d'intelligence -- 10 algo
			
			tab_allumette = new bool[nb_Allumettes]; 
			for (int i=0; i<nb_Allumettes; i++) {
				tab_allumette [i] = false;
			}
		}
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
		void Update ()
		{
			//paramètres de l'area en fonction de la taille de l'écran
			parametre (Screen.width * 0.1f*GestionJeux.demi+Screen.width*GestionJeux.demi, Screen.height * 0.1f*GestionJeux.demi+Screen.height*GestionJeux.demi/2 , Screen.width * 0.8f*GestionJeux.demi, Screen.height * 0.8f*GestionJeux.demi);

			allumette_largeur = Screen.width*0.035f*GestionJeux.demi; //15
			allumette_longueur = Screen.height*0.15f*GestionJeux.demi; //100
            X = Screen.width * 0.1f * GestionJeux.demi + Screen.width * GestionJeux.demi;
            Y = Screen.height * 0.1f * GestionJeux.demi + Screen.height * GestionJeux.demi / 2;

            float HauteurModal = Screen.height * 0.3f * GestionJeux.demi;
            float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
            windowRect = new Rect(X + (Wss - LargeurModal) / 2, Y + (Hss - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

            skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f * 0.5f);
        }
		/**
		 * 
		 * 
		 **/ 
		void OnGUI ()
		{
			GUI.skin = skin_Allumettes;
			skin_Allumettes.label.fontSize =  skin_Allumettes.button.fontSize = (int) (Screen.height* 0.04f*GestionJeux.demi);
            skin_Allumettes.box.fontSize = (int)(Screen.height * 0.04f * GestionJeux.demi);

			GUILayout.BeginArea (new Rect (X0ss, Y0ss, Wss, Hss),GUI.skin.customStyles[0]);

			GUILayout.BeginHorizontal ();

			/******** Espace score et récompense joueur ***********************************************/	
			GUILayout.BeginVertical (GUILayout.Width ( Screen.width * 0.1f*GestionJeux.demi));
			GUILayout.FlexibleSpace ();		
			GUILayout.Label (Score_joueur.ToString ());
			GUILayout.Box ("Joueur");
			GUILayout.EndVertical ();
			/*******************************************************************************************/

			GUILayout.BeginVertical (GUILayout.Width ( Screen.width * 0.5f*GestionJeux.demi));
			/******** Espace titre et marge haute ******************************************************/		
			GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.025f*GestionJeux.demi));	
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal ();
					
			/*GUILayout.BeginHorizontal (GUILayout.Height (Screen.height * 0.05f*GestionJeux.demi));	
			GUILayout.Box ("JEU DE NIM");
			GUILayout.EndHorizontal ();
			/********************************************************************************************/
            if (GestionFocus.JeuxEnPause)
            {
                /******** Espace allumettes jouablent *******************************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.15f * GestionJeux.demi));
                GUILayout.FlexibleSpace();
                int i_i = 0;
                for (int i = 0; i < nb_Allumettes; i++)
                {
                    if (tab_allumette[i] == false)
                    { //test si c'est une allumette disponible
                        GUIStyle gui_style_all = GUI.skin.button;
                        if (i_i < nNb_Allmtt_HeighLight)
                        {
                            gui_style_all = GUI.skin.customStyles[1];
                        }
                        GUILayout.Button(Allumette_Texture, gui_style_all, GUILayout.Width(allumette_largeur), GUILayout.Height(allumette_longueur));

                        i_i++;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace allumettes utilisées actuellement ******************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.15f * GestionJeux.demi));
                //GUILayout.FlexibleSpace ();	
                for (int i = 0; i < nb_Allumettes; i++)
                {
                    if (tab_allumette[i] == true)
                    { //test si c'est une allumette prise
                        //permet au joueur d'annuler un coup, les allumettes sélectionnées ce mettent en-dessous des autres
                        GUILayout.Button(Allumette_Texture, GUILayout.Width(allumette_largeur), GUILayout.Height(allumette_longueur));
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace Bouton Fin de tour et récompense *******************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.25f * GestionJeux.demi));
                GUILayout.Button("Fin de tour");
                GUILayout.FlexibleSpace();
                GUILayout.Box(texte_affiché);
                GUILayout.FlexibleSpace();
                if (cas == 1)
                {
                    texte_affiché = "Vous avez perdu =( \n appuyer sur 'Continuer' \n pour retenter votre chance";
                    GUILayout.Button("Continuer");
                }
                if (cas == 2)
                {
                    texte_affiché = "Vous avez gagné =) \n appuyer sur 'Continuer' \n pour relancer une partie";
                    GUILayout.Button("Continuer");
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace boutons Quitter et Recommencer ********************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.05f * GestionJeux.demi));
                GUILayout.FlexibleSpace();
                GUILayout.Button("Recommencer");
                GUILayout.FlexibleSpace();
                GUILayout.Button("Quitter");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/
            }
            else
            {
                /******** Espace allumettes jouablent *******************************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.15f * GestionJeux.demi));
                GUILayout.FlexibleSpace();
                int i_i = 0;
                for (int i = 0; i < nb_Allumettes; i++)
                {
                    if (tab_allumette[i] == false)
                    { //test si c'est une allumette disponible
                        GUIStyle gui_style_all = GUI.skin.button;
                        if (i_i < nNb_Allmtt_HeighLight)
                        {
                            gui_style_all = GUI.skin.customStyles[1];
                        }
                        if (GUILayout.Button(Allumette_Texture, gui_style_all, GUILayout.Width(allumette_largeur), GUILayout.Height(allumette_longueur)))
                        {
                            if (nb_Allumettes_prises < 3 && blocage == false)
                            { //test si le joueur à déjà sélectionner 3 allumettes et que l'ordi ne joue pas
                                tab_allumette[i] = true;
                                nb_Allumettes_prises++;
                            }
                        }
                        i_i++;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace allumettes utilisées actuellement ******************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.15f * GestionJeux.demi));
                //GUILayout.FlexibleSpace ();	
                for (int i = 0; i < nb_Allumettes; i++)
                {
                    if (tab_allumette[i] == true)
                    { //test si c'est une allumette prise
                        //permet au joueur d'annuler un coup, les allumettes sélectionnées ce mettent en-dessous des autres
                        if (GUILayout.Button(Allumette_Texture, GUILayout.Width(allumette_largeur), GUILayout.Height(allumette_longueur)))
                        {
                            if (blocage == false)
                            { //test si l'ordi n'est pas entrain de jouer
                                tab_allumette[i] = false;
                                nb_Allumettes_prises--;
                            }
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace Bouton Fin de tour et récompense *******************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.25f * GestionJeux.demi));
                if (GUILayout.Button("Fin de tour") && blocage == false)
                {
                    if (nb_Allumettes_prises != 0 || nb_Allumettes <= 0)
                    { //test si le joueur à pris au moins une allumette
                        nb_Allumettes = nb_Allumettes - nb_Allumettes_prises;
                        Validation_tour();
                        if (nb_Allumettes <= 0)
                        { //si il n'y a plus d'allumettes à notre tour on a perdu
                            cas = 1;
                        }
                        else
                        {
                            int allu_supp = ordinateur_joue();
                            nNb_Allmtt_HeighLight = 1;
                            StartCoroutine(Tempo_allu_supp(allu_supp));
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.Box(texte_affiché);
                GUILayout.FlexibleSpace();
                if (cas == 1)
                {
                    texte_affiché = "Vous avez perdu =( \n appuyer sur 'Continuer' \n pour retenter votre chance";
                    if (GUILayout.Button("Continuer"))
                    { //affiche un bouton "Perdu" qui relance une partie
                        Score_ordi++; //incrément le score de l'ordinateur
                        Nouvelle_partie();	//nouvelle partie réinitialisation des paramètres
                    }
                }
                if (cas == 2)
                {
                    texte_affiché = "Vous avez gagné =) \n appuyer sur 'Continuer' \n pour relancer une partie";
                    if (GUILayout.Button("Continuer"))
                    { //affiche un bouton "Gagné" qui relance une partie
                        Score_joueur++; //incrément le score du joueur
                        Nouvelle_partie();	//nouvelle partie réinitialisation des paramètres
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/

                /******** Espace boutons Quitter et Recommencer ********************************************/
                GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.05f * GestionJeux.demi));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Recommencer"))
                    confirmation = true;
                if (confirmation == true)
                    GUI.ModalWindow(0, windowRect, confirme_click, "Etes vous sur de vouloir recommencer ? \n (remise à zéro des scores)", skinStyleWindowsModal);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Quitter"))
                {
                    //réactive notre menu de jeu
                    GameObject.Find("MenuJeux").GetComponent<GestionJeux>().enabled = true;
                    //destruction du jeu instancié
                    Destroy(gameObject);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                /********************************************************************************************/
            }
			GUILayout.EndVertical ();

			/******** Espace score et récompense ordinateur *********************************************/		
			GUILayout.BeginVertical (GUILayout.Width (Screen.width * 0.1f*GestionJeux.demi));
			GUILayout.FlexibleSpace ();		
			GUILayout.Label (Score_ordi.ToString ());
			GUILayout.Box ("Ordinateur");
			GUILayout.EndVertical ();
			/********************************************************************************************/

			GUILayout.EndHorizontal ();
			
			GUILayout.EndArea ();
		}
		/**************** Coroutine *********************************************************************/
		IEnumerator Tempo_allu_supp (int _nb)
		{
			blocage = true; //bloque les coups du joueur pendant le tour de l'ordi

			if (nNb_Allmtt_HeighLight<=_nb) 
				yield return new WaitForSeconds (0.6f);

				nNb_Allmtt_HeighLight++;
			if (nNb_Allmtt_HeighLight<=_nb) 
				yield return new WaitForSeconds (0.6f);

			nNb_Allmtt_HeighLight++;
			if (nNb_Allmtt_HeighLight<=_nb) 
				yield return new WaitForSeconds (0.6f);

			nNb_Allmtt_HeighLight=_nb;
			
			if (nNb_Allmtt_HeighLight<=_nb)yield return null;


			nb_Allumettes = nb_Allumettes - _nb;
			
			if (nb_Allumettes != 0) { //si il n'y a plus d'allumettes on ne valide pas le tour
				Validation_tour ();
			}
			else { //si il n'y a plus d'allumettes à la fin du tour de l'ordinateur on a gagné
				cas = 2;
			}
			nNb_Allmtt_HeighLight = 0;
			blocage = false;
		}
		/************************************************************************************************/
		/**
		 * 
		 * 
		 **/ 
		int ordinateur_joue ()
		{
			int nb_Allumettes_a_enlever;
			float j = 10 * Random.value;
			
			if (j <= level) //détermine si l'ordinateur joue avec intelligence
				nb_Allumettes_a_enlever = nb_Allumettes - nb_Allumettes_opti (nb_Allumettes);
			else
				nb_Allumettes_a_enlever = nb_Allumettes > 3 ? 3 : nb_Allumettes;
			
			if (nb_Allumettes_a_enlever == 0)
				nb_Allumettes_a_enlever = 1; //pas de combinaison optimal possible
			
			

			return nb_Allumettes_a_enlever;
		}
		/**
		 * 
		 *
		 */
		void Validation_tour ()
		{
			
			tab_allumette = new bool[nb_Allumettes];
			for (int i=0; i<nb_Allumettes; i++) {
				tab_allumette [i] = false;
			}
			nb_Allumettes_prises = 0;
		}
		/**
		 * 
		 *
		 */
		private int nb_Allumettes_opti (int n)
		{
			return ((n - 1) / 4) * 4 + 1;
		}
		/**
		 * 
		 *
		 */
		void Nouvelle_partie ()
		{
			nb_Allumettes = 15;
			nb_Allumettes_prises = 0;

			cas = 0;

			texte_affiché = "Choisissez 1, 2, ou 3 allumettes \n puis terminer votre tour";

			tab_allumette = new bool[nb_Allumettes]; 
			for (int i=0; i<nb_Allumettes; i++) {
				tab_allumette [i] = false;
			}
		}
		/**
		 * 
		 *
		 */
		void confirme_click(int windowID) {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Oui"))
				Start ();
            if (GUILayout.Button("Non"))
				confirmation = false;
		}
	}
}