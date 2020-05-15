using System;

namespace g3.intersection
{
    public class IntrLine3Plane3
    {
        Line3d line;
        public Line3d Line
        {
            get { return line; }
            set { line = value; Result = IntersectionResult.NotComputed; }
        }

        Plane3d plane;
        public Plane3d Plane
        {
            get { return plane; }
            set { plane = value; Result = IntersectionResult.NotComputed; }
        }

        public IntersectionResult Result = IntersectionResult.NotComputed;
        public IntersectionType Type = IntersectionType.Empty;

         double lineParameter;
         public double LineParameter
         {
             get { return lineParameter; }
         }
        // Test-intersection query.
        public bool Test()
        {
            var DdN = line.Direction.Dot(plane.Normal);
            if (Math.Abs(DdN) > MathUtil.ZeroTolerance)
            {
                // The line is not parallel to the plane, so they must intersect.
                // The line parameter is *not* set, since this is a test-intersection
                // query.
                Type = IntersectionType.Point;
                return true;
            }

            // The line and plane are parallel.  Determine if they are numerically
            // close enough to be coincident.
            var signedDistance = plane.DistanceTo(line.Origin);
            if (Math.Abs(signedDistance) <= MathUtil.ZeroTolerance)
            {
                Type = IntersectionType.Line;
                return true;
            }

            Type = IntersectionType.Empty;
            return false;
        }

        // Find-intersection query.  The point of intersection is
        // P = origin + t*direction.
        public bool Find()
        {
            var DdN = line.Direction.Dot(plane.Normal);
            var signedDistance = plane.DistanceTo(line.Origin);
            if (Math.Abs(DdN) > MathUtil.ZeroTolerance)
            {
                // The line is not parallel to the plane, so they must intersect.
                lineParameter = -signedDistance / DdN;
                Type = IntersectionType.Point;
                Result = IntersectionResult.Intersects;
                return true;
            }

            // The Line and plane are parallel.  Determine if they are numerically
            // close enough to be coincident.
            if (Math.Abs(signedDistance) <= MathUtil.ZeroTolerance)
            {
                // The line is coincident with the plane, so choose t = 0 for the
                // parameter.
                lineParameter = 0;
                Type = IntersectionType.Line;
                Result = IntersectionResult.Intersects;
                return true;
            }

            Type = IntersectionType.Empty;
            Result = IntersectionResult.NoIntersection;
            return false;
        }
    }

}
