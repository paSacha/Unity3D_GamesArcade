using UnityEngine;
using System.Collections;

namespace Game2D
{
	public class GestionFocus : MonoBehaviour {

        public static bool JeuxEnPause;
        public GUIStyle skinStyleWindowsModal;
        public GUIStyle skinStyleButtonModal;

		public bool ecran ;
		private Rect rectBordGauche;
		private Rect rectBordDroite;
		private GUISkin skin_Menu; 

		void Start () {
			skin_Menu = Resources.Load ("Skins/SkinMenuJeux") as GUISkin;

            JeuxEnPause = ecran = true;
		}
		

		void Update () {
			rectBordDroite = new Rect (Screen.width*0.5f+0.1f, 0.1f, Screen.width*0.5f , Screen.height); //bord rouge
			rectBordGauche = new Rect (0.1f, 0.1f, Screen.width*0.5f , Screen.height); //bord rouge
			
            //swtich entre les deux parties de l'écran
			if (Input.GetKeyDown ("space")) 
            {
                if (ecran)
                {
                    ecran = false;
                }
                else
                {
                    ecran = true;
                }
			}

            JeuxEnPause = ecran;

            //gestion de la taille de la police des fenètres pour quitter les jeux


            //gestion des scripts de contrôle du joueur
            if (JeuxEnPause)
            {
                GameObject.Find("Joueur").GetComponent<MouseLook>().enabled = true;
                GameObject.Find("Joueur").GetComponent<CharacterController>().enabled = true;
                GameObject.Find("Joueur").GetComponent<FPSInputController>().enabled = true;
            }
            else
            {
                GameObject.Find("Joueur").GetComponent<MouseLook>().enabled = false;
                GameObject.Find("Joueur").GetComponent<CharacterController>().enabled = false;
                GameObject.Find("Joueur").GetComponent<FPSInputController>().enabled = false;
            }
		}

		void OnGUI() {
			GUI.skin = skin_Menu;
			
			if (ecran)
				GUI.Box (rectBordGauche, "", GUI.skin.customStyles [5]);
			else 
				GUI.Box (rectBordDroite, "", GUI.skin.customStyles [5]);
		}

	}
}