using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upEngine
{
    /*public class GLSLShader : IShader
    {
        private int _vertexID;
        private int _fragmentID;
        private int _geometryID;

        public int ID
        {
            get;
            protected set;
        }

        public ShaderError AttachSource( string source, ShaderType shaderType )
        {
            switch( shaderType )
            {
                case ShaderType.VertexShader:
                    _vertexID = GL.CreateShader();
                    GL.ShaderSource( _vertexID, source );
                    GL.CompileShader( _vertexID );
                break;
                case ShaderType.FragmentShader:
                    _fragmentID = GL.CreateShader();
                    GL.ShaderSource( _fragmentID, source );
                    GL.CompileShader( _fragmentID );
                break;
                case ShaderType.GeometryShader:
                    _geometryID = GL.CreateShader();
                    GL.ShaderSource( _geometryID, source );
                    GL.CompileShader( _geometryID );
                break;
                default:
                    throw new ArgumentOutOfRangeException( "Unknown ShaderType" );
            }
        }

        public ShaderError CompileShader()
        {
            throw new NotImplementedException();
        }

        public ShaderError LinkShader()
        {
            throw new NotImplementedException();
        }
    }*/
}
