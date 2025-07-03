using UnityEngine;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;


    public void OptionsMenuClose()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

    }

}
