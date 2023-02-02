using System;
using OpenTK.Graphics.OpenGL4;
namespace TerrainRenderer
{
	public class Mesh : IDisposable
	{
        private bool disposedValue;
        private int vertexBufferHandle=-1;
        private int vertexAttribArrayHandle = -1;

        public Mesh()
		{
		}

        #region Disposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if(vertexBufferHandle == -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.DeleteBuffer(vertexBufferHandle);
                    vertexBufferHandle = -1;
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Mesh()
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
        #endregion

        public void LoadFromArray(float[] data)
        {
            this.vertexBufferHandle = GL.GenBuffer();
            this.vertexAttribArrayHandle = GL.GenVertexArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(this.vertexAttribArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexAttribArrayHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.BindVertexArray(0);
        }
    }
}

