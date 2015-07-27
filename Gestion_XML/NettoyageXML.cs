using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic ;
using System.Xml.Serialization;
using System.IO;

namespace Game2D
{
	public class NettoyageXML : MonoBehaviour {
		public static bool suppr( string nameXML)
		{
			// Delete a file by using File class static method...
			if(System.IO.File.Exists("C:\\Users\\Sacha\\Documents\\Bubble_Shooter\\"+nameXML))
			{
				// Use a try block to catch IOExceptions, to
				// handle the case of the file already being
				// opened by another process.
				try
				{
					System.IO.File.Delete("C:\\Users\\Sacha\\Documents\\Bubble_Shooter\\"+nameXML);
				}
				catch (System.IO.IOException e)
				{
					Console.WriteLine(e.Message);
					return false;
				}
				return true;
			}
			return true;
		}
	}
}
