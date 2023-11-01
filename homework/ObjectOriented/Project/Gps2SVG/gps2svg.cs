using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class GPS2SVG
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

    public static GPSData? GPSParseNMEA0183Sentence(string sentence)
    {
        string[] parts = sentence.Split(',');

        if (parts.Length < 10 || parts[0] != "$GPRMC")
        {
            return null;
        }

        DateTime timestamp = DateTime.ParseExact(parts[1], "HHmmss.fff", null);

        if (!float.TryParse(parts[3], out float latitude) ||
            !float.TryParse(parts[5], out float longitude) ||
            !float.TryParse(parts[7], out float speed) ||
            !float.TryParse(parts[8], out float heading))
        {
            return null;
        }

        latitude /= 100;
        longitude /= 100;
        speed *= 1.852f; // Convert from knots to km/h

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

    public static void ProcessGPSData(string inputFilePath, string outputFilePath)
    {
        try
        {
            List<GPSData> gpsDataList = new List<GPSData>();

            float latitude_min = float.MaxValue;
            float latitude_max = float.MinValue;
            float longitude_min = float.MaxValue;
            float longitude_max = float.MinValue;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = GPSParseNMEA0183Sentence(line);
                    if (data.HasValue && data.Value.valid_satellite_fix)
                    {
                        gpsDataList.Add(data.Value);

                        // Find bounds
                        if (data.Value.latitude < latitude_min) latitude_min = data.Value.latitude;
                        if (data.Value.latitude > latitude_max) latitude_max = data.Value.latitude;
                        if (data.Value.longitude < longitude_min) longitude_min = data.Value.longitude;
                        if (data.Value.longitude > longitude_max) longitude_max = data.Value.longitude;
                    }
                }
            }

            float longitude_width = longitude_max - longitude_min;
            float latitude_width = latitude_max - latitude_min;

            int width = 1024;
            int height = 1024;

            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                // Write the SVG file header
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
                writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
                writer.WriteLine($"<svg width=\"{width}\" height=\"{height}\" viewBox=\"-50 -50 {width+124} {height+124}\" xmlns=\"http://www.w3.org/2000/svg\">");
                writer.WriteLine("\t<g opacity=\"0.8\">");

                // Write polyline points
                StringBuilder points = new StringBuilder("\t\t<polyline points=\"");
                foreach (var data in gpsDataList)
                {
                    float x = width * (data.longitude - longitude_min) / longitude_width;
                    float y = height - height * (data.latitude - latitude_min) / latitude_width;
                    points.AppendFormat("{0},{1} ", (int)x, (int)y);
                }
                points.Append("\" stroke=\"red\" stroke-width=\"4\" fill=\"none\" />");
                writer.WriteLine(points.ToString());

                writer.WriteLine("\t</g>");
                writer.WriteLine("</svg>");
            }

            Console.WriteLine("GPS data processed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: dotnet run -- <inputFilePath> <outputFilePath>");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = args[1];

        ProcessGPSData(inputFilePath, outputFilePath);
    }
}




