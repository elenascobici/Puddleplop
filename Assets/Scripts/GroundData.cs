using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

[Serializable]
public class GroundData
{
    public List<Tuple<int,int,int>> soilTiles;
}
