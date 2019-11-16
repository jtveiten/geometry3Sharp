using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g3.intersection
{
    public class IntrSegment3Plane3
    {
        Segment3d segment;

        public Segment3d Segment
        {
            get { return segment; }
            set
            {
                segment = value;
                Result = IntersectionResult.NotComputed;
            }
        }

        Plane3d plane;

        public Plane3d Plane
        {
            get { return plane; }
            set
            {
                plane = value;
                Result = IntersectionResult.NotComputed;
            }
        }

        public IntersectionResult Result = IntersectionResult.NotComputed;

        public IntersectionType Type = IntersectionType.Empty;
        double segmentParameter;

        public double SegmentParameter
        {
            get { return segmentParameter; }
        }

        // Test-intersection query.
        public bool Test()
        {
            Vector3d P0 = segment.P0;
            var sdistance0 = plane.DistanceTo(P0);
            if (Math.Abs(sdistance0) <= MathUtil.ZeroTolerance)
            {
                sdistance0 = 0;
            }

            var P1 = segment.P1;
            var sdistance1 = plane.DistanceTo(P1);
            if (Math.Abs(sdistance1) <= MathUtil.ZeroTolerance)
            {
                sdistance1 = 0;
            }

            var prod = sdistance0 * sdistance1;
            if (prod < 0)
            {
                // The segment passes through the plane.
                Type = IntersectionType.Point;
                return true;
            }

            if (prod > 0)
            {
                // The segment is on one side of the plane.
                Type = IntersectionType.Empty;
                return false;
            }

            if (sdistance0 != 0 || sdistance1 != 0)
            {
                // A segment end point touches the plane.
                Type = IntersectionType.Point;
                return true;
            }

            // The segment is coincident with the plane.
            Type = IntersectionType.Segment;
            return true;
        }

        public bool Find()
        {
            var line = new Line3d(segment.Center, segment.Direction);
            IntrLine3Plane3 intr = new IntrLine3Plane3 {Line = line, Plane = plane};
            if (intr.Find())
            {
                // The line intersects the plane, but possibly at a point that is
                // not on the segment.
                Type = intr.Type;
                segmentParameter = intr.LineParameter;
                return Math.Abs(segmentParameter) <= segment.Extent;
            }

            Type = IntersectionType.Empty;
            return false;
        }
    }
}
