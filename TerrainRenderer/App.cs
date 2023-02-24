using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using TerrainRenderer.Shaders;
using Terrain.Loaders;
using System.Drawing;
using Terrain.Data;

namespace TerrainRenderer
{
	public class App : GameWindow
	{

        private Mesh _mesh;
        private ShaderProgram _shader;
        private SimpleCamera _camera;
        private WorldState _state;
        private Landscape _landscape;

		public App()
			: base (new GameWindowSettings
            {
                RenderFrequency = 60,
                UpdateFrequency = 60
            },
				  new NativeWindowSettings
			{
				APIVersion = new Version(4,1),
				Profile = ContextProfile.Core,
				Flags = ContextFlags.ForwardCompatible,
				Size = new OpenTK.Mathematics.Vector2i(1024,768),
                
			})
		{
            this.CenterWindow();
		}

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Blue);
            GL.FrontFace(FrontFaceDirection.Cw);
            CursorState = CursorState.Grabbed;
            var exampleData = new float[]
            {
                0.0f, 0.5f, 0f,
                0.5f, -0.5f, 0f,
                -0.5f, -0.5f, 0f
            };

            _state = new WorldState(new WorldVariable[]{
                new WorldVariable("matView", typeof(Matrix4)),
                new WorldVariable("matProj", typeof(Matrix4)),
                new WorldVariable("matModel", typeof(Matrix4)),
                new WorldVariable("matWVP", typeof(Matrix4)),
                new WorldVariable("matVP", typeof(Matrix4))
            });

            _camera = new SimpleCamera(1024, 768, 24.0f, 100.0f);
            _camera.Position = new Vector3(0.0f, 0.0f, 1.0f);
            _camera.Forward = new Vector3(0.0f, 0.0f, -1.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            
            _mesh = new Mesh();
            _mesh.LoadFromArray(exampleData);
            _shader = new ShaderProgram(File.ReadAllText("Shaders/vertex.glsl"), File.ReadAllText("Shaders/fragment.glsl"), _state);
            _shader.Load();


            AsciiGridLoader ld = new AsciiGridLoader();
            string path = "/Users/krismcalpine/personal/mapdata/nt"; 
            var tile = ld.LoadTerrain(path);
            float[][] data = new float[2][];
            data[0] = new float[2] { 0.0f, 0.0f };
            data[1] = new float[2] { 0.0f, 0.0f };
            var tile2 = new TerrainTile(data, 10, 0, 0);
            _landscape = new Landscape(tile);
            _landscape.Load();
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
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            _camera.Update(MouseState.Delta.X, MouseState.Delta.Y, (float)args.Time);

            KeyboardState input = KeyboardState;
            float time = (float)args.Time;

            if (input.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }

            if (input.IsKeyDown(Keys.W))
            {
                _camera.MoveForward(time);
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.MoveBackward(time);
            }

            if (input.IsKeyDown(Keys.A))
            {
                _camera.MoveLeft(time);
            }

            if (input.IsKeyDown(Keys.D))
            {
                _camera.MoveRight(time);
            }

            if (input.IsKeyDown(Keys.Space))
            {
                _camera.MoveUp(time);
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.MoveDown(time);
            }
        
            _state.SetValue<Matrix4>("matView", _camera.ViewMatrix);
            _state.SetValue<Matrix4>("matProj", _camera.ProjectionMatrix);
            _state.SetValue<Matrix4>("matVP", _camera.ViewMatrix * _camera.ProjectionMatrix);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //_mesh.Draw(_shader, _state);
            _landscape.Draw(_shader, _state);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

    }
}

