using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "pl1")
        {
            Debug.Log("Insert Player death here!");
            SceneManager.LoadScene("SecondaryTestScene", LoadSceneMode.Single);
        }
    }
}