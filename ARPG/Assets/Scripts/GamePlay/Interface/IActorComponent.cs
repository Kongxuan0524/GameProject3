using UnityEngine;
using System.Collections;

public interface IActorComponent
{
    void Execute();
    void Release();
}