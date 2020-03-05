using System.Collections.Generic;
using Pcx;
using UnityEngine;

public static class PointCloudDataExstensions
{
    private const int ElementSize = sizeof(float) * 4;
    
    /// <summary>
    /// Mock method to simulate decimation of point cloud
    /// </summary>
    /// <returns>Compute buffer containing our filtered points</returns>
    public static ComputeBuffer GetNewDensity(this PointCloudData data, int sparseness)
    {
        var filteredData = new List<PointCloudData.Point>();
        for (var i = 0; i < data.Points.Length; i += sparseness)
        {
            filteredData.Add(data.Points[i]);
        }
        
        var buf = new ComputeBuffer(filteredData.Count, ElementSize);
        
        buf.SetData(filteredData);
        return buf;
    }
}
