using System;

namespace FoodyGo.Mapping
{
    /// <summary>
    /// 맵 위치 모델
    /// </summary>
    [Serializable]
    public struct MapLocation
    {
        public MapLocation(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double latitude; // 위도
        public double longitude; // 경도
    }
}
