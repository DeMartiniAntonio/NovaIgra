using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text sliderText;


    public void OptionsMenuClose()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

    }

}
