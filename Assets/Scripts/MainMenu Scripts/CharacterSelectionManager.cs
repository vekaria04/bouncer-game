using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public Material playerMaterial; 
    //public Image characterPreviewImage; 
    public string gameSceneName = "Game"; 

    private int currentColorIndex = 0;

    private Color[] colors = new Color[] {
        Color.blue,
        Color.green,
        Color.yellow,
        Color.red,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black,
        Color.grey,
    };

    public void NextColor()
    {
        currentColorIndex = (currentColorIndex + 1) % colors.Length;
        UpdateCharacterColor();
    }

    public void PreviousColor()
    {
        currentColorIndex--;
        if (currentColorIndex < 0)
            currentColorIndex = colors.Length - 1;
        UpdateCharacterColor();
    }

    private void UpdateCharacterColor()
    {
        playerMaterial.color = colors[currentColorIndex];
        //characterPreviewImage.color = colors[currentColorIndex];
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("PlayerColor", ColorUtility.ToHtmlStringRGBA(colors[currentColorIndex]));
        SceneManager.LoadScene(gameSceneName);
    }

    private void Start()
    {
        string savedColorHtml = PlayerPrefs.GetString("PlayerColor", "");
        Color savedColor;
        if (ColorUtility.TryParseHtmlString("#" + savedColorHtml, out savedColor))
        {
            currentColorIndex = System.Array.IndexOf(colors, savedColor);
            if (currentColorIndex == -1) currentColorIndex = 0;
        }
        UpdateCharacterColor();
    }
}
