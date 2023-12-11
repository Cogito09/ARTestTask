using UnityEngine;

public static class GameObjectExtensions
{
    public static void Deactivate(this GameObject go)
    {
        if (go != null && go.activeSelf)
        {
            go.SetActive(false);
        }
    }

    public static void Activate(this GameObject go)
    {
        if (go != null && go.activeSelf == false)
        {
            go.SetActive(true);
        }
    }

    public static void ChangeActive(this GameObject go, bool value)
    {
        if (value)
        {
            if (go != null)
            {
                Activate(go);
            }
        }
        else
        {
            if (go != null)
            {
                Deactivate(go);
            }
        }
    }
}