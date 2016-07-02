using UnityEngine;
using System.Collections;

public static class AMoPUtils
{
    public static Color GetColorForAffiliation(BoardNodeAffiliation affiliation)
    {
        Color newClr;
        switch (affiliation)
        {
            case BoardNodeAffiliation.Player:
                newClr = Color.cyan;
                break;

            case BoardNodeAffiliation.Enemy:
                newClr = new Color(1.0f, 1.0f, 0);
                break;

            case BoardNodeAffiliation.Neutral:
                newClr = Color.white;
                break;

            default:
                newClr = Color.magenta;
                break;
        }
        return newClr;
    }
}
