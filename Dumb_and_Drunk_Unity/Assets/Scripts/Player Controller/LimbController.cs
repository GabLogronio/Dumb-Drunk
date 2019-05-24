using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimbController : MonoBehaviour
{
    protected bool Moving = false, Active = true;

    protected Vector2 CurrentDirection;

    public virtual bool Move()
    {
        Moving = true;
        // Debug.Log("Moving " + gameObject.name + "\n");
        return true;
    }

    public virtual bool UpdateDirection(Vector2 ToSet, bool direction)
    {
        CurrentDirection = ToSet;

        if (Moving) return true;
        return false;
    }

    public virtual bool Set(bool ToSet)
    {
        Active = ToSet;
        return ToSet;
    }

    public virtual bool Release()
    {
        Moving = false;
        // Debug.Log("Releasing " + gameObject.name + "\n");
        return true;
    }

}
