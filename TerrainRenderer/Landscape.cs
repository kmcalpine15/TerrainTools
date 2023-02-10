using System;
using Terrain.Data;
using OpenTK.Graphics.OpenGL4;

namespace TerrainRenderer
{
    public class Landscape : IDisposable
    {
        private bool disposedValue;
        private int _vertexBufferHandle;
        private int _vertexAttribArrayHandle;

        public TerrainTile Terrain { get; }

        public Landscape(TerrainTile terrain)
        {
            Terrain = terrain;
        }

        public void Load()
        {
            this._vertexBufferHandle = GL.GenBuffer();
            this._vertexAttribArrayHandle = GL.GenVertexArray();

            float[] data = new float[Terrain.NumColumns * Terrain.NumRows * 3];
            float spacing = (float)Terrain.CellSize;

            for (int row = 0; row < Terrain.NumRows; row++)
            {
                for (int col = 0; col < Terrain.NumColumns; col++)
                {
                    int idx = (row * Terrain.NumColumns + col) * 3;
                    data[idx] = col * spacing;
                    data[idx + 1] = row * spacing;
                    data[idx + 2] = 0.0f;
                }
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(_vertexAttribArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        public void Unload()
        {

        }

        public void Draw()
        {

            GL.BindVertexArray(this._vertexAttribArrayHandle);
            GL.DrawArrays(PrimitiveType.Points, 0, 3);
            GL.BindVertexArray(0);

        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Landscape()
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
    }
}

