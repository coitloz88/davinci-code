using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Manager : MonoBehaviour
{
    //===================================================
    #region Main Menu
    public void MainMenu_Btn_Play() // 게임 시작
    {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu_Btn_OpenSettingMenu() // 세팅 메뉴 열기
    {
        SceneManager.LoadScene("Setting");
    }
    public void MainMenu_Btn_Quit() // 프로그램 종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit() // 어플리케이션 종료
#endif
    }
    private void OnApplicationQuit() //프로그램이 종료되었을 경우 // 보통 정보 저장 기능 구현
    {

    }
    #endregion
    //===================================================
    #region Setting Menu
    public void Setting_Slider_Sound() // 사운드 조절 Slider
    {

    }
    public void Setting_Btn_OpenMainMenu() // 메인 메뉴 열기
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Setting_Dropdown_SelectGameLevel() // 게임 난이도 설정
    {

    }
    #endregion
    //===================================================
}
