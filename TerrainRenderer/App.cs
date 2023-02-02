using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using TerrainRenderer.Shaders;

namespace TerrainRenderer
{
	public class App : GameWindow
	{

        private Mesh _mesh;
        private ShaderProgram _shader;

		public App()
			: base (GameWindowSettings.Default,
				  new NativeWindowSettings
			{
				APIVersion = new Version(4,1),
				Profile = ContextProfile.Core,
				Flags = ContextFlags.ForwardCompatible,
				Size = new OpenTK.Mathematics.Vector2i(640,480)
			})
		{
            this.CenterWindow();
		}

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Blue);

            var exampleData = new float[]
            {
                0.0f, 0.5f, 0f,
                0.5f, -0.5f, 0f,
                -0.5f, -0.5f, 0f
            };

            _mesh = new Mesh();
            _mesh.LoadFromArray(exampleData);
            _shader = new ShaderProgram(File.ReadAllText("Shaders/vertex.glsl"), File.ReadAllText("Shaders/fragment.glsl"));
            _shader.Load();
        }

        protected override void OnUnload()
        {
            _mesh.Dispose();
            _shader.Dispose();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Activate();
            _mesh.Draw();
            _shader.Deactivate();

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

    }
}

