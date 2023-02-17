using System;
using Terrain.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;
using System.Transactions;
using TerrainRenderer.Shaders;

namespace TerrainRenderer
{
    public class Landscape : IDisposable
    {
        private bool disposedValue;
        private int _vertexBufferHandle;
        private int _vertexAttribArrayHandle;
        private int _indexBufferHandle;
        private int _numTriangles;

        public TerrainTile Terrain { get; }

        public Landscape(TerrainTile terrain)
        {
            Terrain = terrain;
        }

        public float[] GenerateVertices()
	    {
            float[] data = new float[Terrain.NumColumns * Terrain.NumRows * 3];
            float spacing = (float)Terrain.CellSize;

            for (int row = 0; row < Terrain.NumRows; row++)
            {
                for (int col = 0; col < Terrain.NumColumns; col++)
                {
                    int idx = (row * Terrain.NumColumns + col) * 3;
                    data[idx] = col * spacing;
                    data[idx + 1] = Terrain.Data[row][col];
                    data[idx + 2] = -(row * spacing);
                }
            }
            return data;
        }

        public ushort[] GenerateIndices()
        {
            List<ushort> indices = new List<ushort>();
            for (int quadY = 0; quadY < Terrain.NumRows - 1; quadY++)
            {
                for (int quadX = 0; quadX < Terrain.NumColumns - 1; quadX++)
                {
                    indices.Add((ushort)(quadY * Terrain.NumColumns + quadX));
                    indices.Add((ushort)((quadY + 1) * Terrain.NumColumns + quadX));
                    indices.Add((ushort)((quadY + 1) * Terrain.NumColumns + quadX + 1));

                    indices.Add((ushort)((quadY + 1) * Terrain.NumColumns + quadX + 1));
                    indices.Add((ushort)(quadY * Terrain.NumColumns + quadX + 1));
                    indices.Add((ushort)(quadY * Terrain.NumColumns + quadX));
                }
            }
            return indices.ToArray();
        }

        public void Load()
        {
            this._vertexBufferHandle = GL.GenBuffer();
            this._vertexAttribArrayHandle = GL.GenVertexArray();
            this._indexBufferHandle = GL.GenBuffer();

            var data = GenerateVertices();
            var indices = GenerateIndices();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(ushort) * indices.Length, indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.BindVertexArray(_vertexAttribArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float)*3, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferHandle);
            GL.BindVertexArray(0);

            _numTriangles = indices.Length / 3;
        }

        public void Unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_indexBufferHandle);
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexAttribArrayHandle);
        }

        public void Draw(ShaderProgram program, WorldState state)
        {
            state.SetValue<Matrix4>("matModel", Matrix4.CreateScale(0.001f));
            program.Activate(state);

            GL.BindVertexArray(_vertexAttribArrayHandle);
            GL.DrawElements(BeginMode.Triangles, _numTriangles, DrawElementsType.UnsignedShort, 0);
            GL.BindVertexArray(0);

            program.Deactivate();
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
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' metho≠d
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

