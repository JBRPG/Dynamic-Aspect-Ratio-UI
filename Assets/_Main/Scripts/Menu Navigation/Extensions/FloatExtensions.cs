using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static bool Approximately (float a, float b, float threshold)
    {
        if (threshold < 0) threshold = -threshold;
        float result = ((a - b) < 0 ? ((a - b) * -1) : (a - b));
        return result <= threshold;
    }
}
