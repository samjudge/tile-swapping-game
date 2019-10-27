using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ScriptedSpawner : MonoBehaviour
{
    public abstract void StartSpawning();
    public abstract void StopSpawning();
    public abstract UnitSpawner GetSpawner();
    public abstract void SetSpawner(UnitSpawner Spawner);
}
