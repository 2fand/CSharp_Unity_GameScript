using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public static Font gameFont;
    public static bool useHealth = false;
    public static string hpUnit = "";
    public static bool useMp = false;
    public static string mpUnit = "";
    public static bool useExp = false;
    public static string expUnit = "";
    public static bool useLevel = false;
    public static string levelUnit = "";
    public static int defaultHp = 1;
    public static int defaultMp = 0;
    public static int defaultExp = 0;
    public static int defaultLevel = 0;
    public static int defaultMaxHp = 1;
    public static int defaultMaxMp = 0;
    public static int defaultMaxLevel = 0;
    public static char me = 'I';
    public static char empty = ' ';
    public static char wall = 'X';
    public static char npc = 'N';
    public static float enterTime = 0.5f;
    public static float exitTime = 0.5f;
}