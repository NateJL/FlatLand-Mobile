using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationConstants : MonoBehaviour
{
    public enum MainMenuPage
    {
        MAIN_PAGE = 0,
        NEW_GAME_PAGE,
        LOAD_GAME_PAGE,
        SETTINGS_PAGE
    }

    public enum GameOverlay
    {
        CLOSED = 0,
        MENU,
        SETTINGS,
        DIALOGUE
    }

    public enum Scenes
    {
        MAIN_MENU = 0,
        FLATLAND
    }

    public enum Faction
    {
        LOWER_CLASS = 0,
        MIDDLE_CLASS,
        NOBILITY,
        LINELAND,
        SPACELAND,
        POINTLAND
    }

    public static Vector3 chromastistonTownCenter = new Vector3(300, 0, 300);
    public static Vector3 homeTownCenter = new Vector3(0, 0, 0);

    public static float PLAYER_MAX_MOUSE_DISTANCE = 250f;
    public static float PLAYER_MAX_SINGLE_STEP = 0.24f;
}
