using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace upEngine
{
    public enum ShaderError
    {
        NoError = -1,
        CreationError,
        SourceError,
        CompileError,
        LinkError
    }

    /*public enum ShaderType
    {
        VertexShader,
        FragmentShader,
        GeometryShader
    }*/

    public interface IShader
    {
        int ID
        {
            get;
        }

        void AttachSource( string source, ShaderType shaderType );
        void CompileShader();
        void LinkShader();
        void Use();
        void SetUniform( string uniformName, Vector3 v3 );
        void SetUniform( string uniformName, Vector4 v4 );
        void SetUniform( string uniformName, Matrix4 m4 );
        void SetUniform( string uniformName, int i );
        void SetUniform( string uniformName, float f );
    }
}
