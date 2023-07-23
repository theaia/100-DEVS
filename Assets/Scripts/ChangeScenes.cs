using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScenes : MonoBehaviour
{
 //   public void ChangeSceneRight(){ 
 //       if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1){
	//		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	//	} else{
	//		SceneManager.LoadScene(0);

	//	}
        
 //   }

	//public void ChangeSceneLeft()
	//{
	//	if (SceneManager.GetActiveScene().buildIndex > 0)
	//	{
	//		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
	//	}
	//	else
	//	{
	//		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);

	//	}

	//}

	public void ChooseScene(int sceneID)
    {
		SceneManager.LoadScene(sceneID);
    }
}
