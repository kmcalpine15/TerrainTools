using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TerrainRenderer
{
	public class SimpleCamera 
	{
		public Vector3 Position { get; set; }
		public Vector3 Forward { get; set; }
		public Vector3 Up { get; set; }
		public int ViewWidth { get; set; }
		public int ViewHeight { get; set; }
		private float _pitch=0.0f;
		private float _yaw=-90.0f;
		public float Sensitivity{ get; set; }
		public float Speed { get; set; }

		public Matrix4 ProjectionMatrix { get; private set; }

		public Matrix4 ViewMatrix
		{
			get
			{
				return new Matrix4(
					1.0f, 0.0f, 0.0f,0.0f,
					0.0f,1.0f, 0.0f, 0.0f,
					0.0f, 0.0f, 1.0f, 0.0f,
					-Position.X, -Position.Y, -Position.Z, 1.0f
				);
			}
		}

		public SimpleCamera(int viewWidth, int viewHeight, float speed=100.0f, float sensitivity=1.0f)
		{
			ViewWidth = viewWidth;
			ViewHeight = viewHeight;
			Sensitivity = sensitivity;
			Speed = speed;
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)viewWidth / (float)viewHeight, 1.0f, 10000.0f);
		}

		public void MoveForward(float timeElapsed)
		{
			Position += Forward * Speed * timeElapsed;
		}

        public void MoveBackward(float timeElapsed)
        {
            Position -= Forward * Speed * timeElapsed;
        }

		public void MoveLeft(float timeElapsed)
		{

		}


        public void Update(float mouseDeltaX, float mouseDeltaY, float timeElapsed)
		{
			//_pitch += mouseDeltaY * Sensitivity;

			//if (_pitch > 89.0f) { _pitch = 89.0f; }
			//else if (_pitch < -89.0f) { _pitch = -89.0f; }

			//_yaw += mouseDeltaX * Sensitivity;

			//Forward = new Vector3(
			//	(float)Math.Cos(MathHelper.DegreesToRadians(_pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(_yaw)),
			//	(float)Math.Sin(MathHelper.DegreesToRadians(_pitch)),
			//	(float)Math.Cos(MathHelper.DegreesToRadians(_pitch) * (float)Math.Sin(MathHelper.DegreesToRadians(_yaw)))
			//	);

			//Forward = Vector3.Normalize(Forward);
		}
	}
}

