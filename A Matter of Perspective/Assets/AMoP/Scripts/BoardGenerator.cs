using UnityEngine;
using System.Collections;

public class BoardGenerator
{
    private BoardData data;

    public BoardGenerator(BoardData data)
    {
        this.data = data;
    }
    
}

public class NoBoardDataException : System.InvalidOperationException
{
    public NoBoardDataException() : base("No board data in generator.") { }
    public NoBoardDataException(string message) : base(message) { }
}