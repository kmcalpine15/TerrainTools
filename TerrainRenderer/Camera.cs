using System;
using OpenTK.Mathematics;
namespace TerrainRenderer
{
	public class Camera
	{
		public Vector3 Position { get; set; }
		public Vector3 Target { get; set; }
		public Vector3 Up { get; set; }
		public int ViewWidth { get; set; }
		public int ViewHeight { get; set; }
		public Matrix4 ProjectionMatrix
		{
			get
			{
                return Matrix4.CreatePerspectiveFieldOfView(0.785398f, ViewWidth / ViewHeight, 1.0f, 10000.0f);
            }
		}

		public Matrix4 ViewMatrix
		{
			get
			{
				return Matrix4.LookAt(Position, Target, Up);
			}
		}

		public Camera(int viewWidth, int viewHeight)
		{
			ViewWidth = viewWidth;
			ViewHeight = viewHeight;
		}

	}
}

