using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStats { Start,Play,Over, Completed};
public class GameManager : MonoBehaviour
{
    public static int currentLevel = 1;
    public static GameStats stats;
}
