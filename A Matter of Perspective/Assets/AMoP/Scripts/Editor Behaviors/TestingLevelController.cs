using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLevelController : LevelBehavior
{
    public BoardData TestBoard;

    public override void StartGame()
    {
        if (TestBoard == null)
        {
            Debug.Log("No board to test.");
            return;
        }

        ResetState();
        SetUpBoard(TestBoard);
        DoStartGame();
        InitializeBoardInfoClass(TestBoard);
    }
}
