using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand  {


    void Execute(GameObject actor);
    void Undo(GameObject actor);
}
