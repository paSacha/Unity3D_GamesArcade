using UnityEngine;
using System;
using System.Collections.Generic ;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
//using BUS;


namespace Game2D
{
	public sealed class Jeu_du_BubbleShooter : MonoBehaviour
	{
		private static  Jeu_du_BubbleShooter instance ;

		private Jeu_du_BubbleShooter ()
		{
		
		}

		public static Jeu_du_BubbleShooter Instance {
			get {
				return instance;
			}
		}
		private string CHEMIN ;
		private long Idjoueur;

        private float Largeur;
        private float Hauteur;
        private float EcartL;
        private float EcartH;
        private float X;
        private float Y;
        private float Larg_But;
        private float Haut_But;
        private Rect R_SupportGo;
        public GUIStyle skinStyleExit;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;
        private bool quitter;

		private float taille_boule = 0.4f;
		private Color couleur_invi = new Color (1, 1, 1, 0);
		private float X0ss;
		private float Y0ss;
		private float Wss;
		private float Hss;
		private GameObject clone ;
		private GameObject pivot;
		private GameObject debut_boules;
		private float turnSpeed = 50f;
		private int num_couleur;
		private int w_MatriceBoules = 23;
		private int h_MatriceBoules;
		private int tp_bl_MatriceBoules;
		private float vitesse_descente;
		private float tempo;
		private Rect windowRect;
		private bool finJeu;
		private bool bloquage;
		private bool decalage;
		private int[][] MatriceTempo;
		private Tab_Personnes TabPersonnes = null;
		/*variables public*/		
		public int score;
		public GameObject boule ;
		public static int[][]MatriceBoules;
		public static Color[] Couleur_MatriceBoules;
		public bool testeur; 



		// Use this for initialization
		void Start ()
		{
            quitter = false;

			CHEMIN = Application.dataPath + "/FichiersXML/ScoresBubble.xml";
			//ouverture ou création du fichier XML*********************
			if (File.Exists (CHEMIN))
				TabPersonnes = Tab_Personnes.Charger (CHEMIN);
			else
				TabPersonnes = new Tab_Personnes ();
			//*******************************************

			//Récupération de l'ID du joueur
			Idjoueur = 12345; //test ID
			//Idjoueur = CUser.id;

            /******** Gestion des fenetre pour quitter *******/
            skinStyleWindowsModal.normal.background = (Texture2D)Resources.Load("Textures/Fond_gris");
            skinStyleWindowsModal.alignment = TextAnchor.UpperCenter;
            skinStyleButtonModal.normal.background = (Texture2D)Resources.Load("Textures/FondBlanc");
            skinStyleButtonModal.hover.background = (Texture2D)Resources.Load("Textures/unchecked");
            skinStyleButtonModal.alignment = TextAnchor.UpperCenter;
            /*************************************************/

            skinStyleExit.normal.background = (Texture2D)Resources.Load("Textures/Exit_1_met");
            skinStyleExit.hover.background = (Texture2D)Resources.Load("Textures/Exit_2_met");

			Affichage_Score ();

			//active/désactive les différents sons du jeu
			GameObject.Find ("BS_SonFinJeu").GetComponent<AudioSource>().enabled = false;
			GameObject.Find ("BS_SonJeu").GetComponent<AudioSource>().enabled = true;

			//évite de tirer un boule en recommencant une partie
			if (GameObject.Find ("boulet(Clone)"))
				Destroy (GameObject.Find ("boulet(Clone)"));

			bloquage = finJeu = decalage =false;
			instance = this;
			tempo = score = 0;
			//définit le nombre de boules en hauteur
			h_MatriceBoules = 10+1; //+1 pour la ligne qui définit si l'on a perdu
			tp_bl_MatriceBoules = 3; //permet de définir le nombre de couleurs jouées
			vitesse_descente = 20.0f; //permet de faire descendre les boules plus ou moins vite (en secondes)

			Vector4 vectTrans = new Vector4 (1, 1, 1, 1f);
			Couleur_MatriceBoules = new Color[7]; // 7 couleurs disponibles en tout
			Couleur_MatriceBoules [0] =Vector4.Scale( Color.red,vectTrans);
			Couleur_MatriceBoules [1] =Vector4.Scale(  Color.blue ,vectTrans);
			Couleur_MatriceBoules [2] =Vector4.Scale(  Color.green,vectTrans);
			Couleur_MatriceBoules [3] =Vector4.Scale(  Color.cyan,vectTrans);
			Couleur_MatriceBoules [4] =Vector4.Scale(  Color.magenta,vectTrans);
			Couleur_MatriceBoules [5] =Vector4.Scale( Color.yellow,vectTrans);
			Couleur_MatriceBoules [6] =Vector4.Scale( Color.gray,vectTrans);

			//********* initialisation matrice boule ************//
			MatriceBoules = new int[h_MatriceBoules][];
			for (int i=0; i< h_MatriceBoules; i++)
				MatriceBoules [i] = new int[w_MatriceBoules];
		
		
			for (int i=0; i< h_MatriceBoules/2; i++) 
				for (int k=0; k< w_MatriceBoules; k++)
					MatriceBoules [i] [k] = UnityEngine.Random.Range (0, tp_bl_MatriceBoules);

			for (int i=h_MatriceBoules/2; i< h_MatriceBoules; i++) 
				for (int k=0; k< w_MatriceBoules; k++)
					MatriceBoules [i] [k] = -1;
			//***************************************************//

			pivot = GameObject.Find ("BS_Pivot_boule");
			debut_boules = GameObject.Find ("BS_Matrice_boules");

			AfficherMatriceBoules ();
		}
	//cette fonction est appeller selon des paramètres de temps qu'on définit
		void FixedUpdate(){
            if (!bloquage || GestionFocus.JeuxEnPause)
            {//permet de bloquer le jeu tant que le joueur n'as pas décider si il continue ou arrete
				if (tempo >= vitesse_descente) {
					if (decalage)
						decalage = false;
					else
						decalage = true;
					descente_boule (); //fait descendre les boules d'un rang et créer un nouvelle ligne aléatoire
					tempo = 0;
				} else
					tempo+=Time.fixedDeltaTime; 
			}
		}

		void Update ()
		{
            Largeur = Screen.width * 0.78f * GestionJeux.demi;
            Hauteur = Screen.height * 0.90f * GestionJeux.demi;
            EcartL = 0.01f * Largeur;
            EcartH = 0.01f * Hauteur;
            Larg_But = (Screen.width * GestionJeux.demi - Largeur) / 2 - 2 * EcartL;
            Haut_But = Hauteur / 4;

            X = Screen.width * 0.1f * GestionJeux.demi + Screen.width * GestionJeux.demi;
            Y = Screen.height * 0.1f * GestionJeux.demi + Screen.height * GestionJeux.demi / 2;
            R_SupportGo = new Rect(X + Largeur + EcartL, Y + Hauteur - Haut_But*0.9f, Larg_But, Haut_But*0.8f);

			if (!bloquage && !GestionFocus.JeuxEnPause) { //permet de bloquer le jeu tant que le joueur n'as pas décider si il continue ou arrete

				game_over (); //vérifie si on a perdu
				if (finJeu){
					for (int i=0; i< h_MatriceBoules-1; i++) 
						for (int k=0; k< w_MatriceBoules; k++) {
							new WaitForSeconds (0.6f);
							MatriceBoules [i] [k] =  UnityEngine.Random.Range (0, tp_bl_MatriceBoules);
						}
					AfficherMatriceBoules();
				}

                float HauteurModal = Screen.height * 0.3f * GestionJeux.demi;
                float LargeurModal = Screen.width * 0.5f * GestionJeux.demi;
                windowRect = new Rect(X + (Largeur - LargeurModal) / 2, Y + (Hauteur - HauteurModal) / 2, LargeurModal, HauteurModal); //fenetre pour la fin du jeu

                skinStyleWindowsModal.fontSize = (int)(Screen.height * 0.04f);
                skinStyleButtonModal.fontSize = (int)(Screen.height * 0.04f);

/********** gestion de la rotation de notre flèche *****************************************/
				if (transform.rotation.z <= 0.61)
				if (Input.GetKey (KeyCode.LeftArrow))
					transform.Rotate (Vector3.forward, turnSpeed * Time.deltaTime);

				if (transform.rotation.z >= -0.61)
				if (Input.GetKey (KeyCode.RightArrow))
					transform.Rotate (Vector3.back, turnSpeed * Time.deltaTime);
/*******************************************************************************************/

/********** création d'une nouvelle boule à lancé si on à lancé la notre *******************/
				if (!GameObject.Find ("boulet(Clone)")) {
					clone = (GameObject)GameObject.Instantiate (boule, pivot.transform.position, new Quaternion (0,0,0,0));
					num_couleur = (int)( UnityEngine.Random.Range (0, tp_bl_MatriceBoules));
					clone.GetComponent<SpriteRenderer> ().color = Couleur_MatriceBoules [num_couleur]; 
					clone.gameObject.transform.localScale = new Vector3 (0.8f * taille_boule*4f, 0.8f * taille_boule*4f, 1);
					clone.transform.parent = pivot.transform;
					clone.GetComponent<CBouleManager> ().Actif = true;
				}
/********************************************************************************************/
			}
		}
		/**
	 * 
	 * 
	 **/
		//****** Gestion de la descente des boules ********//
		void descente_boule() {
			int i = h_MatriceBoules-2;
			//décale toutes les boules de une ligne en copiant la ligne supérieur sur la ligne sélectionnée
			while (i>=0){ 
				for (int k=0; k< w_MatriceBoules; k++)
					MatriceBoules [i+1] [k] = MatriceBoules [i] [k];
				i--;
			}
			//remplie notre première ligne de manière aléatoire
			for (int k=0; k< w_MatriceBoules; k++)
				MatriceBoules [0] [k] =  UnityEngine.Random.Range (0, tp_bl_MatriceBoules);
			//mise à jour de l'affichage de notre matrice
			AfficherMatriceBoules ();
			vitesse_descente--; //accélère la vitesse du jeu après chaque descente
		}
		//*************************************************//

		//****** Gestion de la fin du jeu******************//
		void game_over() {
			for (int k=0; k< w_MatriceBoules; k++)
				// vérifie la dernière ligne de jeu
				if (MatriceBoules [h_MatriceBoules - 1] [k] != -1) {
					finJeu = true;
					bloquage = true;
					//rajoute un score de la personne jouant
					TabPersonnes.Add(new Personne(){ Id = Idjoueur , score = score });
					//Sauvegarde sur le fichier xml
					TabPersonnes.Enregistrer(CHEMIN);
					Affichage_Score();
			}
		}
		//*************************************************//

		void OnGUI(){

            //Créer Bouton Exit
            if (GUI.Button(R_SupportGo, "", skinStyleExit))
                quitter = true;
            if (quitter)
            {
                if (GestionFocus.JeuxEnPause)
                    GUI.ModalWindow(0, windowRect, confirme_clickPause, "Voulez vous vraiment quitter?", skinStyleWindowsModal);
                else
                    GUI.ModalWindow(0, windowRect, confirme_click, "Voulez vous vraiment quitter?", skinStyleWindowsModal);
            }

			if (finJeu) {
                if (GestionFocus.JeuxEnPause)
                {
                    GUI.ModalWindow(0, windowRect, confirme_clickPause, "Fin de la partie \n Voulez vous retenter votre chance ?", skinStyleWindowsModal);
                }
                else
                {
                    GameObject.Find("BS_SonFinJeu").GetComponent<AudioSource>().enabled = true;
                    GameObject.Find("BS_SonJeu").GetComponent<AudioSource>().enabled = false;
                    GUI.ModalWindow(0, windowRect, confirme_click, "Fin de la partie \n Voulez vous retenter votre chance ?", skinStyleWindowsModal);
                }
			}
		}
        /*******************************/
		void confirme_click(int windowID) {
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button("Recommencer", skinStyleButtonModal))
            {
				Start ();
			}
            if (GUILayout.Button("Quitter", skinStyleButtonModal))
            {
				//testeur = NettoyageXML.suppr("Test.xml");

				//réactive notre menu de jeu
				GameObject.Find ("MenuJeux").GetComponent<GestionJeux>().enabled = true;
                //destruction de la dernière boule à lancer
                Destroy(GameObject.Find("boulet(Clone)"));
				//destruction du jeu instancié
				Destroy (GameObject.Find("BubbleShooter(Clone)"));
			}
		}
        /*******************************/
        void confirme_clickPause(int windowID)
        {
            GUILayout.FlexibleSpace();
            GUILayout.Button("Recommencer", skinStyleButtonModal);
            GUILayout.Button("Quitter", skinStyleButtonModal);
        }
		/**
	 * 
	 * 
	 **/
		//**** fonction qui regénère notre matrice ***********//
		void Nettoyage (int compte)
		{
			//si on a trouvé au moins 3 boules voisines alors on les suppriment 
			if (compte >= 3) {
				score += compte; //augmente le score du joueur pour chaques boules éliminées
				StartCoroutine (GestionSon());
				for (int i=0; i< h_MatriceBoules; i++) 
					for (int k=0; k< w_MatriceBoules; k++)
						if (MatriceBoules [i] [k] <= -2) {
							MatriceBoules [i] [k] = -1;
						}
			} else { //sinon on les remets à la valeur qu'elles avaient
				for (int i=0; i< h_MatriceBoules; i++) 
					for (int k=0; k< w_MatriceBoules; k++)
						if (MatriceBoules [i] [k] == -2)
							MatriceBoules [i] [k] = Game2D.CBouleManager.ancienne_couleur ;
			}
			compte = 0;
			//on recréer la matrice
			AfficherMatriceBoules ();
		}
		//***************************************************//
	/**
	 * 
	 * 
	 **/

		IEnumerator GestionSon () {
			GameObject.Find ("BS_SonBulle").GetComponent<AudioSource>().enabled = true;
			yield return new WaitForSeconds (1.0f);
			GameObject.Find ("BS_SonBulle").GetComponent<AudioSource>().enabled = false;
		}

	//****** vérifie si une boule voisine est de la meme couleurs *******************//
		int verif_boule (int a, int b)
		{
			int nb_vois = 1;
			bool ligne_paire;
			int val_boule = MatriceBoules [a] [b];
			MatriceBoules [a] [b] = -2;
			if (decalage)
				ligne_paire = (a % 2)==0;
			else
				ligne_paire = (a % 2)==1;
			//continue permet de passer à l'itération suivante
			for(int i=-1+a; i<=1+a;i++){
				if(i<0 || i>=h_MatriceBoules) continue;
				for(int j=-1+b; j<=1+b;j++){
					// vérification par rapport à la limite de largeur
					if(j<0 || j>=w_MatriceBoules || (i==a && j==b)) continue;
					// si la ligne est impaire on ignore les boules voisines haute et basse de droite
					if(!ligne_paire && (((i==a-1)&&(j==b+1))||((i==a+1)&&(j==b+1)))) continue;
					// si la ligne est paire on ignore les boules voisines haute et basse de gauche
					if(ligne_paire && (((i==a-1)&&(j==b-1))||((i==a+1)&&(j==b-1)))) continue;

					if(val_boule==MatriceBoules[i][j]){
						nb_vois+=verif_boule(i,j);
					}
				}
			}
			return nb_vois;
		}
		//*******************************************************************************//

		//****** vérifie si une boule ou un groupe de boule n'ont pas de voisins collé à une limite ****//
		int verif_GroupeSeul (int a, int b)
		{
			int nb_vois = 1;
			MatriceTempo [a] [b] = 2;// boule vérifiée

			bool ligne_paire;
			if (decalage)
				ligne_paire = (a % 2)==0;
			else
				ligne_paire = (a % 2)==1;

			//continue permet de passer à l'itération suivante
			for(int i=-1+a; i<=1+a;i++){
				if(i<0 || i>=h_MatriceBoules) continue;
				for(int j=-1+b; j<=1+b; j++){
					// vérification par rapport à la limite de largeur
					if(j<0 || j>=w_MatriceBoules || (i==a && j==b)) continue;
					// si la ligne est impaire on ignore les boules voisines haute et basse de droite
					if(!ligne_paire && (((i==a-1)&&(j==b+1))||((i==a+1)&&(j==b+1)))) continue;
					// si la ligne est paire on ignore les boules voisines haute et basse de gauche
					if(ligne_paire && (((i==a-1)&&(j==b-1))||((i==a+1)&&(j==b-1)))) continue;

					if(MatriceTempo[i][j] == 1) {
						//appel récursif si le voisin possède une couleur
						nb_vois +=verif_GroupeSeul(i,j); 
					}
				}
			}
			return  nb_vois ;
		}
		//***************************************************************************************************//

	/**
	 * 
	 * 
	 **/
		void AfficherMatriceBoules ()
		{
			//Mise à jour de l'affichage du score
			GameObject.Find ("BS_TextMeshScore").GetComponent<TextMesh>().text = score.ToString();

			//***** détruit notre affichage de l'ancienne matrice *******//
			GameObject[] AllGo = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
			foreach (GameObject go in AllGo) {
				if (go.name == "BS_MatriceBoules" || go.name == "BS_MatriceBoules_Invi")
					Destroy (go);
			}
			//***********************************************************//

			Vector3 pos_rel = Vector3.zero;

			for (int i=0; i< h_MatriceBoules; i++)
				for (int k=0; k< w_MatriceBoules; k++) {
					pos_rel.y = -i * taille_boule;
					//permet un descente des boules avec toujours la meme forme pour les groupes de couleurs
					if (decalage)
						pos_rel.x = k * taille_boule + (i % 2 == 1 ? 0 : taille_boule / 2.0f); //permet de décaler une ligne sur deux
					else
						pos_rel.x = k * taille_boule + (i % 2 == 0 ? 0 : taille_boule / 2.0f); //permet de décaler une ligne sur deux
	
				clone = (GameObject)GameObject.Instantiate (boule, debut_boules.transform.position + pos_rel, new Quaternion (0,0,0,0));
					clone.rigidbody2D.isKinematic = true; //permet de rendre la boule immobile
					clone.GetComponent<CBouleManager> ().enabled = false; //désactive le script sur celle-ci
					clone.GetComponent<CBouleManager> ().Actif = false; //sécurité supplémentaire pour le script
					//permet la récupération des index de la matrice
					clone.GetComponent<CBouleManager> ().xi = i;
					clone.GetComponent<CBouleManager> ().xj = k;
					//choix de la couleur en fonction de la valeur de la case matricielle
					if (MatriceBoules [i] [k] != -1) {
					//if( Couleur_MatriceBoules [MatriceBoules [i] [k]]!=Couleur_MatriceBoules[0])
						clone.GetComponent<SpriteRenderer> ().color = Couleur_MatriceBoules [MatriceBoules [i] [k]];
						clone.GetComponent<CircleCollider2D> ().enabled = true;
					clone.gameObject.name = "BS_MatriceBoules";
					}//si on a -1 c'est qu'il n'y as pas de boule physique donc pas de Collider actif
					else {
						clone.GetComponent<SpriteRenderer> ().color = couleur_invi;
						clone.GetComponent<CircleCollider2D> ().enabled = false;
					clone.gameObject.name = "BS_MatriceBoules_Invi";
					}
					//on place la boule et la déclare enfant du GameObject Matrice_boules
					clone.gameObject.transform.localScale = new Vector3 (0.8f * taille_boule*4f, 0.8f * taille_boule*4f, 1);
					clone.transform.parent = debut_boules.transform;
				}
		}

		public void UpdateGameState (int l, int c)
		{
			//verif_boule est une fonction qui regarde si les boules voisines sont de la meme couleur et les comptent
			Nettoyage (verif_boule (l, c)); //reconstruit notre matrice et son affichage

			//** init matrice temporaire **//
			MatriceTempo = new int[h_MatriceBoules][];
			for (int i=0; i< h_MatriceBoules; i++)
				MatriceTempo [i] = new int[w_MatriceBoules];
			
			for (int i=0; i< h_MatriceBoules; i++)
				for (int k=0; k< w_MatriceBoules; k++) 
					MatriceTempo[i][k]=(MatriceBoules[i][k] >= 0)?1:0;
			//*****************************//

			//test toutes les lignes du haut
			for (int k=0; k< w_MatriceBoules; k++)
				if (MatriceTempo [0] [k] == 1)
					verif_GroupeSeul (0, k); //verif_GroupSeul permet de repérer les groupes de boules isolées
			//remplace les boules isolées par des boules inactives
			for (int i=0; i< h_MatriceBoules; i++)
				for (int k=0; k< w_MatriceBoules; k++)
					if (MatriceTempo [i] [k] != 2)
						MatriceBoules [i] [k] = -1;

			Nettoyage (0);

		}
		public void Affichage_Score()
		{
			int[] tabScores = new int[50];
			int i = 0;
			//récupération de tout les scores du joueur actuel
			foreach (Personne perso in TabPersonnes) {
				if (perso.Id == Idjoueur) {
					tabScores[i] = perso.score ;
					i++;
				}
			}
			//classement du tableau
			ordonnerTableau (tabScores , 50);
			//écriture des score sur un TextMesh dans le jeu (en bas à gauche)
			GameObject.Find ("BS_MScores").GetComponent<TextMesh>().text = "1er : "+tabScores[49].ToString() + "\n2ème : " + tabScores[48].ToString() + "\n3ème : " + tabScores[47].ToString();


		}

		//permet de mettre le tableau dans l'ordre croissant
		void ordonnerTableau(int[] tableau, int tailleTableau)
		{
			int tmp;
			
			for(int j=0;j<tailleTableau - 1;j++){
				for(int i=0;i<tailleTableau - 1;i++){
					if(tableau[i] > tableau[i+1]){
						tmp = tableau[i];
						tableau[i] = tableau[i+1];
						tableau[i+1] = tmp;
					}
				}
			}
		}

	}


}