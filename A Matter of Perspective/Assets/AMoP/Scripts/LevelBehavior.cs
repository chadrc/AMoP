using UnityEngine;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{

    [SerializeField]
    private BoardData boardData;

    [SerializeField]
    private BoardNodeFactory boardNodeFactory;

	// Use this for initialization
	void Start () {
        Board board = new Board(boardData, boardNodeFactory);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
