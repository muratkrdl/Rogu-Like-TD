using UnityEngine;

public class LookObj : MonoBehaviour
{
    [SerializeField] GetInputs getInputs;

    [SerializeField] float Range;

    MainMenuButton currentlyMainMenuButton;

    void Update() 
    {
        Vector2 direciton;
        if(getInputs.GetMoveInput != Vector2.zero)
        {
            direciton = getInputs.GetMoveInput;
        }
        else
        {
            direciton = getInputs.GetLastMoveDir;
        }
        
        transform.localPosition = Vector2.zero + direciton * Range;

        if(Input.GetKeyDown(KeyCode.E) && currentlyMainMenuButton != null)
        {
            currentlyMainMenuButton.InteractWithButton();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent<MainMenuButton>(out var mainMenuButtonComponent))
        {
            currentlyMainMenuButton = mainMenuButtonComponent;
            mainMenuButtonComponent.OnObjHitted(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.TryGetComponent<MainMenuButton>(out var mainMenuButtonComponent) && currentlyMainMenuButton == mainMenuButtonComponent)
        {
            currentlyMainMenuButton = null;
            mainMenuButtonComponent.OnObjHitted(false);
        }
    }

}
