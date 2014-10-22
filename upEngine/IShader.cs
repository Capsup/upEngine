using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //ShaderError AttachSource( string source, ShaderType shaderType );
        ShaderError CompileShader();
        ShaderError LinkShader();
    }
}
