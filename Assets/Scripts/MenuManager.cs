using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] Canvas _levelSelect;
    [SerializeField] Canvas _menu;

    [SerializeField] GameObject _levelButtonPrefab;
    [SerializeField] GameObject _levelGrid;

    private void Awake()
    {
        _levelSelect.enabled = false;
        _menu.enabled = true;

        int numberOfLevels = SceneManager.sceneCountInBuildSettings - 1;

        for (int i = 0; i < numberOfLevels; i++)
        {
            GameObject levelI = Instantiate(_levelButtonPrefab, _levelGrid.transform);

            levelI.name = "Level" + (i + 1).ToString();
            levelI.GetComponent<Button>().onClick.AddListener(LoadLevel);
            levelI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleLevelSelect(bool state)
    {
        _levelSelect.enabled = state;
        _menu.enabled = !state;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
    }
}
