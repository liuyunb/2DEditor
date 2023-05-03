using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvent : MonoBehaviour
{
    public static void ReturnToMain()
    {
        SceneManager.LoadScene("GameStart");
    }

    public static void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
