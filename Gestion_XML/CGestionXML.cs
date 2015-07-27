using UnityEngine;
using System;
using System.Collections.Generic ;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Game2D
{
	[Serializable]
	public class Personne //création de la classe personne utlisisé dans le jeu BubbleShooter
	{
		public long Id { get; set; }
		
		public int score { get; set; }
	}
	[Serializable]
	public class Jeu //création de la classe Jeu qui définit un jeu
	{
		public long Id { get; set; }
		
		public string Titre { get; set; }
		
		public string Description { get; set; }
	}

	[Serializable()]
	public class Tab_Personnes : List<Personne>     //On hérite d'une liste de personnes.
	{
		public void Enregistrer (string chemin)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(Tab_Personnes));
			StreamWriter ecrivain = new StreamWriter (chemin);
			serializer.Serialize (ecrivain, this);
			ecrivain.Close ();
		}
		
		public static Tab_Personnes Charger (string chemin)
		{
			XmlSerializer deserializer = new XmlSerializer (typeof(Tab_Personnes));
			StreamReader lecteur = new StreamReader (chemin);
			Tab_Personnes p = (Tab_Personnes)deserializer.Deserialize (lecteur);
			lecteur.Close ();
			
			return p;
		}
	}
	[Serializable()]
	public class Tab_Jeux : List<Jeu>     //On hérite d'une liste de jeux.
	{
		public void Enregistrer (string chemin)
		{
			XmlSerializer serializer2 = new XmlSerializer (typeof(Tab_Jeux));
			StreamWriter ecrivain2 = new StreamWriter (chemin);
			serializer2.Serialize (ecrivain2, this);
			ecrivain2.Close ();
		}
		
		public static Tab_Jeux Charger (string chemin)
		{
			XmlSerializer deserializer2 = new XmlSerializer (typeof(Tab_Jeux));
			StreamReader lecteur2 = new StreamReader (chemin);
			Tab_Jeux p2 = (Tab_Jeux)deserializer2.Deserialize (lecteur2);
			lecteur2.Close ();
			
			return p2;
		}
	}

}
