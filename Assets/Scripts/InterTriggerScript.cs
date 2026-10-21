using UnityEngine;
using UnityEngine.SceneManagement;
// When the player enters it, save the score and load the End scene.
public class InterTriggerScript : MonoBehaviour
{
    //[SerializeField] private string endSceneName = "EndScene";
 
    private void OnTriggerEnter(Collider other)
    {
        // Only the player can finish the level
        if (!other.CompareTag("Player")) return;
 
        // Save the score BEFORE loading: the ScoreManager is about
        // to be destroyed together with the rest of SampleScene
        ScoreManager.Instance.SaveFinalScore();
 
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
