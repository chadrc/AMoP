using UnityEngine;
using System.Collections;

public class BoardGenerator
{
    private BoardData data;

    public BoardGenerator(BoardData data)
    {
        this.data = data;
    }

    public Board Generate(BoardNodeFactory nodeFactory)
    {
        if (data == null)
        {
            throw new NoBoardDataException();
        }

        Board board = new Board(data, nodeFactory);

        return board;
    }
}

public class NoBoardDataException : System.InvalidOperationException
{
    public NoBoardDataException() : base("No board data in generator.") { }
    public NoBoardDataException(string message) : base(message) { }
}