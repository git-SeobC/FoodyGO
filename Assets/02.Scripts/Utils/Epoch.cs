using System;

namespace FoodyGo.Utils
{
    public static class Epoch
    {
        public static double Now
        {
            get
            {
                DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
                return cur_time;
            }
        }
    }
}