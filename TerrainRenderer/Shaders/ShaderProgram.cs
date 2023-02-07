using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace TerrainRenderer.Shaders
{
	public class ShaderProgram : IDisposable
	{
        private bool disposedValue;

        public string VertexShader { get; }
		public string FragmentShader { get; }
		public int ShaderProgramHandle { get; private set; }
		public WorldState WorldState { get; private set; }
		private Dictionary<string, int> _uniformMappings;


		public ShaderProgram(string vertexShader, string fragmentShader, WorldState state)
		{
			VertexShader = vertexShader;
			FragmentShader = fragmentShader;
			WorldState = state;
		}

		public void Load()
		{
			int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShaderHandle, VertexShader);
			GL.CompileShader(vertexShaderHandle);
			string vertshaderLog = GL.GetShaderInfoLog(vertexShaderHandle);
			if(vertshaderLog != string.Empty)
			{
				throw new Exception($"Error compiling vertex shader:\n{vertshaderLog}");
			}

			int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderHandle, FragmentShader);
			GL.CompileShader(fragmentShaderHandle);
            string fragmentShaderLog = GL.GetShaderInfoLog(fragmentShaderHandle);
            if (fragmentShaderLog != string.Empty)
            {
                throw new Exception($"Error compiling fragment shader:\n{fragmentShaderLog}");
            }

            ShaderProgramHandle = GL.CreateProgram();
			GL.AttachShader(ShaderProgramHandle, vertexShaderHandle);
			GL.AttachShader(ShaderProgramHandle, fragmentShaderHandle);
			GL.LinkProgram(ShaderProgramHandle);

			string log = GL.GetProgramInfoLog(ShaderProgramHandle);
            if (log != string.Empty)
            {
                throw new Exception($"Error linking shader program:\n{log}");
            }

			_uniformMappings = new Dictionary<string, int>();

            foreach (var worldVariable in WorldState.GetVariableNames())
			{
				int variableLocation = GL.GetUniformLocation(ShaderProgramHandle, worldVariable);
				
				if(variableLocation != -1)
				{
					_uniformMappings.Add(worldVariable, variableLocation);
				}
            }

            GL.DetachShader(ShaderProgramHandle, vertexShaderHandle);
			GL.DetachShader(ShaderProgramHandle, fragmentShaderHandle);

			GL.DeleteShader(vertexShaderHandle);
			GL.DeleteShader(fragmentShaderHandle);
		}


		public void Unload()
		{
			if(ShaderProgramHandle != -1)
			{
				GL.UseProgram(0);
				GL.DeleteProgram(ShaderProgramHandle);
				ShaderProgramHandle = -1;
			}
		}

		public void Activate(WorldState state)
		{
			GL.UseProgram(this.ShaderProgramHandle);

			foreach(var mapping in _uniformMappings)
			{
				var value = state.GetVariable(mapping.Key);
				ApplyUniform(value);
			}
		}

		private void ApplyUniform(WorldVariable variable)
		{
			if(variable.Value == null)
			{
				return;
			}
			
			switch(variable.Type.Name)
			{
				case nameof(Vector3):
					GL.Uniform3(_uniformMappings[variable.Name], (Vector3)variable.Value);
					break;
				case nameof(Vector4):
					GL.Uniform4(_uniformMappings[variable.Name], (Vector4)variable.Value);
					break;
				case nameof(Matrix4):
					GL.UniformMatrix4(_uniformMappings[variable.Name], (Matrix4)variable.Value);
					break;
				default:
					break;
			}
		}

		public void Deactivate()
		{
			GL.UseProgram(0);
		}

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

				this.Unload();
                disposedValue = true;
            }
        }

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~ShaderProgram()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

