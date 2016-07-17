using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public enum BoardCompletionLevel : int
{
    Completed = -1,
    Bronze = 0,
    Silver = 1,
    Gold = 2,
}

[System.Serializable]
public class BoardScores : IEnumerable<Pair<BoardCompletionLevel, int>>
{
    [SerializeField]
    private int[] scores = new int[3];

    public int HighestScore
    {
        get
        {
            return scores[2];
        }
    }

    public void SetScore(BoardCompletionLevel level, int score)
    {
        if (level == BoardCompletionLevel.Completed)
        {
            Debug.LogError("Cannot set score for BoardCompletionLevel.Completed");
            return;
        }

        scores[(int)level] = score;
    }

    public BoardCompletionLevel GetCompletionLevel(int score)
    {
        int highest = -1;
        int current = 0;
        foreach (var s in scores)
        {
            if (score >= s)
            {
                highest = current;
                current++;
            }
            else
            {
                break;
            }
        }
        return (BoardCompletionLevel)highest;
    }

    public IEnumerator<Pair<BoardCompletionLevel, int>> GetEnumerator()
    {
        return new BoardScoresEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static List<BoardCompletionLevel> Levels { get; private set; }

    static BoardScores()
    {
        Levels = new List<BoardCompletionLevel>((BoardCompletionLevel[])System.Enum.GetValues(typeof(BoardCompletionLevel)));
        Levels.Remove(BoardCompletionLevel.Completed);
    }

    private class BoardScoresEnumerator : IEnumerator<Pair<BoardCompletionLevel, int>>
    {
        private int currentIndex;
        private BoardScores scores;

        public BoardScoresEnumerator(BoardScores scores)
        {
            this.scores = scores;
            currentIndex = -1;
        }

        public Pair<BoardCompletionLevel, int> Current
        {
            get
            {
                return makePair();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return makePair();
            }
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < Levels.Count;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        private Pair<BoardCompletionLevel, int> makePair()
        {
            var level = Levels[currentIndex];
            var score = scores.scores[(int)level];
            return new Pair<BoardCompletionLevel, int>(level, score);
        }
    }
}
