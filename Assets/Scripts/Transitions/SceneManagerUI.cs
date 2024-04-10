using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerUI : MonoBehaviour
{
    Scene scene;
    int sceneIndex;
    int sceneCountInBuildSettings;
    int maxSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();

        sceneIndex = scene.buildIndex;
        Debug.Log("Index :  " + scene.buildIndex);

        sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Total Number of Scenes = " + SceneManager.sceneCountInBuildSettings);

        maxSceneIndex = sceneCountInBuildSettings - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNextScene()
    {
        if (sceneIndex < maxSceneIndex)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else if (sceneIndex == maxSceneIndex)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GoToPreviousScene()
    {
        if(sceneIndex == 0)
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(sceneIndex - 1);
        }
    }
}
