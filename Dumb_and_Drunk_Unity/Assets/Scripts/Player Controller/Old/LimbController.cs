using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimbController : MonoBehaviour
{
    public abstract bool Detach();

    public abstract bool Move();

    public abstract bool Set(bool ToSet);

}
