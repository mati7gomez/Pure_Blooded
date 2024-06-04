using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Levelloader : MonoBehaviour {

	void OnGUI ()
	{
		// Make a background box
		GUI.Box (new Rect (10, 10, 140, 310), "Loader Menu");

		//Buttons
		if (GUI.Button (new Rect (20, 40, 120, 30), "Blue Sky")) {
			SceneManager.LoadScene(0);
		}
		if (GUI.Button (new Rect (20, 70, 120, 30), "Blue Sky 02")) {
            SceneManager.LoadScene(1);
		}
		if (GUI.Button (new Rect (20, 100, 120, 30), "Bright Morning")) {
            SceneManager.LoadScene(2);
		}
		if (GUI.Button (new Rect (20, 130, 120, 30), "Golden Horizon")) {
            SceneManager.LoadScene(3);
		}
		if (GUI.Button (new Rect (20, 160, 120, 30), "Moody Sunrise")) {
			SceneManager.LoadScene (4);
		}
		if (GUI.Button (new Rect (20, 190, 120, 30), "Moody Sunrise 02")) {
			SceneManager.LoadScene (5);
		}
		if (GUI.Button (new Rect (20, 220, 120, 30), "Pink Sunset")) {
			SceneManager.LoadScene (6);
		}
		if (GUI.Button (new Rect (20, 250, 120, 30), "Pink Sunset 02")) {
			SceneManager.LoadScene (7);
		}
		if (GUI.Button (new Rect (20, 280, 120, 30), "Stormy Sky")) {
			SceneManager.LoadScene (8);
		}
		
		
	}
}
