using UnityEngine;
using System.Collections;
using Game2D;

public class FenetreJeu : MonoBehaviour {
    private float Largeur;
    private float Hauteur;
    private float EcartL;
    private float EcartH;
    private float Larg_But;
    private float Haut_But;
    private float demi = 0.5f;
    private Rect R_SupportGo;
    public GUIStyle skinStyleExit;

    public Camera camJeux;
    private Camera camJoueur;

	// Use this for initialization
	void Start () 
    {
        skinStyleExit.normal.background = (Texture2D)Resources.Load("Textures/Exit_1_met");
        skinStyleExit.hover.background = (Texture2D)Resources.Load("Textures/Exit_2_met");
        camJoueur = GameObject.Find("Joueur").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Largeur = Screen.width * 0.78f * demi;
        Hauteur = Screen.height * 0.90f * demi;
        EcartL = 0.01f * Largeur;
        EcartH = 0.01f * Hauteur;
        Larg_But = (Screen.width * demi - Largeur) / 2 - 2 * EcartL;
        Haut_But = Hauteur / 4;

        R_SupportGo = new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Larg_But, Haut_But * 0.8f);
        
        
    }

    void OnGUI()
    {
        //Créer Bouton qui lance le menu de jeux
        if (GUI.Button(R_SupportGo, "", skinStyleExit)) 
        {
            GameObject.Find("Focus").GetComponent<GestionFocus>().enabled = true;
            RenderTexture TextureCamera = Resources.Load("Textures/CameraJoueurText") as RenderTexture;
            GameObject.Find("Joueur").GetComponent<Camera>().targetTexture = TextureCamera;
            camJoueur.camera.enabled = false;
            camJeux.camera.enabled = true;
            camJoueur.camera.enabled = true;
            GameObject.Find("MenuJeux").GetComponent<GestionJeux>().enabled = true;
        }
    }

}
