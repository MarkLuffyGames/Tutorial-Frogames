using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNewPlace : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;
    [SerializeField] private Vector3 NewPlacePosition;
    [SerializeField] private bool isInteractive;

    private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isInteractive)
            {
                SceneManager.LoadScene(SceneToLoad);
                other.transform.position = NewPlacePosition;
            }
            else
            {
                playerController = other.GetComponent<PlayerController>();
                playerController.GetInputPlayer().Player.Interact.started += Interact_started;
            }

        }
       
    }

    private void Interact_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerController.SetNewPosition(NewPlacePosition);
        playerController.GetInputPlayer().Player.Interact.started -= Interact_started;
        playerController = null;
        SceneManager.LoadScene(SceneToLoad);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(playerController != null)playerController.GetInputPlayer().Player.Interact.started -= Interact_started;
        playerController = null;
    }
}
