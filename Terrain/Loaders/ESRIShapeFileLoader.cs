using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Binary;
using Terrain.Data;
using System.Reflection.PortableExecutable;

namespace Terrain.Loaders
{

    public enum ShapeType
    {
        NullShape = 0,
        Point = 1,
        PolyLine = 3,
        Polygon = 5,
        MultiPoint = 8,
        PointZ = 11,
        PolyLineZ = 13,
        PolygonZ = 15,
        MultiPointZ = 18,
        PointM = 21,
        PolyLineM = 23,
        PolygonM = 25,
        MultiPointM = 28,
        MultiPatch = 31
    }

    public struct ESRIFileHeader
    {
        public int FileCode;
        public int FileLength;
        public int Version;
        public ShapeType ShapeType;
        public double XMin;
        public double YMin;
        public double XMax;
        public double YMax;
        public double ZMin;
        public double ZMax;
        public double MMin;
        public double MMax;
    }

    public struct ESRIRecordHeader
    {
        public int RecordNumber { get; set; }
        public int ContentLength { get; set; }
        public ShapeType ShapeType { get; set; }
    }
    public  class ESRIShapeFileLoader
    {

        public ESRIFileHeader ReadHeader(BinaryReader reader)
        {
            var header = new ESRIFileHeader();
            header.FileCode = BinaryPrimitives.ReverseEndianness(reader.ReadInt32());

            if (header.FileCode != 9994)
            {
                throw new ArgumentException("Invalid file.");
            }

            //Unused bytes
            reader.ReadBytes(20);

            header.FileLength = BinaryPrimitives.ReverseEndianness(reader.ReadInt32());
            header.Version = reader.ReadInt32();

            if (header.Version != 1000)
            {
                throw new ArgumentException("Invalid shapefile version");
            }

            var shapeType = (ShapeType)reader.ReadInt32();
            header.XMin = reader.ReadDouble();
            header.YMin = reader.ReadDouble();
            header.XMax = reader.ReadDouble();
            header.YMax = reader.ReadDouble();
            header.ZMin = reader.ReadDouble();
            header.ZMax = reader.ReadDouble();
            header.MMin = reader.ReadDouble();
            header.MMax = reader.ReadDouble();

            return header;
        }

        public ESRIRecordHeader ReadRecordHeader(BinaryReader reader)
        {
            return new ESRIRecordHeader
            {
                RecordNumber = BinaryPrimitives.ReverseEndianness(reader.ReadInt32()),
                ContentLength = BinaryPrimitives.ReverseEndianness(reader.ReadInt32()),
                ShapeType = (ShapeType)reader.ReadInt32()
            };
        }

        public Point[] ReadPointRecords(BinaryReader reader, ESRIRecordHeader header)
        {
            List<Point> values = new List<Point>();
            int totalToRead = header.ContentLength * 2;
            for (int i = 0; i < totalToRead; i++)
            {
                values.Add(new Point (reader.ReadDouble(), reader.ReadDouble() ));
            }

            return values.ToArray();
        }

        public void ReadPolylineRecords(BinaryReader reader, ESRIRecordHeader header)
        {
            double xMin = reader.ReadDouble();
            double yMin = reader.ReadDouble();
            double xMax = reader.ReadDouble();
            double yMax = reader.ReadDouble();

            int numParts = reader.ReadInt32();
            int numPoints = reader.ReadInt32();

            Int32[] parts = new Int32[numParts];
            for (int i = 0; i < numParts; i++)
            {
                parts[i] = reader.ReadInt32();
            }

            Point[] points = new Point[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                points[i] = new Point (reader.ReadDouble(), reader.ReadDouble() );
            }
        }


        public void ProcessRecords(BinaryReader reader, ESRIFileHeader fileHeader)
        {
            while (true)
            {
                var recordHeader = ReadRecordHeader(reader);

                switch(recordHeader.ShapeType)
                {
                    case ShapeType.Point:
                        var points = ReadPointRecords(reader, recordHeader);
                        break;
                    case ShapeType.PolyLine:
                        ReadPolylineRecords(reader, recordHeader);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported shape type {recordHeader.ShapeType}");
                }
            }
        }

        public ESRIFileHeader LoadFile(string path)
        {
            using (var fle = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fle))
                {
                    var header= ReadHeader(reader);
                    ProcessRecords(reader, header);
                    return header;

                }
            }
        }
    }
}
