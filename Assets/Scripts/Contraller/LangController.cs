using UnityEngine;

public class LangController : MonoBehaviour
{   
    public string lang = "zh_TW";
    public string[] langList = new string[] { "zh_TW", "en_US" };

    public void ChangeLang()
    {
        int index = System.Array.IndexOf(langList, lang);
        index = (index + 1) % langList.Length;
        lang = langList[index];
    }

}