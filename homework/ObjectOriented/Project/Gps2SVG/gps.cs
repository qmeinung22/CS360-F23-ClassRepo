using System;

public class GPS
{
    public struct GPSData
    {
        public DateTime timestamp;
        public float latitude;
        public float longitude;
        public float speed;
        public float heading;
        public bool valid_satellite_fix;
    }

    public static GPSData GPSParseNMEA0183Sentence(string sentence)
    {
        string[] parts = sentence.Split(',');

        if (parts.Length < 10 || parts[0] != "$GPRMC")
        {
            return new GPSData { valid_satellite_fix = false };
        }

        DateTime timestamp = DateTime.ParseExact(parts[1], "HHmmss.fff", null);
        float latitude = float.Parse(parts[3]) / 100;
        float longitude = float.Parse(parts[5]) / 100;
        float speed = float.Parse(parts[7]) * 1.852f; // Convert from knots to km/h
        float heading = float.Parse(parts[8]);

        if (parts[4] == "S")
        {
            latitude = -latitude;
        }
        if (parts[6] == "W")
        {
            longitude = -longitude;
        }

        return new GPSData
        {
            timestamp = timestamp,
            latitude = latitude,
            longitude = longitude,
            speed = speed,
            heading = heading,
            valid_satellite_fix = (parts[2] == "A")
        };
    }
}
