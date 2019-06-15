using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimbController : MonoBehaviour
{
    protected bool Moving = false, Active = true;

    protected Vector3 CurrentDirection;

    public virtual bool Move()
    {
        Moving = true;
        //DebugText.instance.Set("Moving " + gameObject.name);
        return true;
    }

    public virtual void UpdateDirection(Vector3 ToSet)
    {
        CurrentDirection = ToSet;
    }

    public virtual bool Set(bool ToSet)
    {
        Active = ToSet;
        return ToSet;
    }

    public virtual bool Release()
    {
        Moving = false;
        //DebugText.instance.Set("Releasing " + gameObject.name);
        return true;
    }

    public virtual void Detach()
    {

    }
}
