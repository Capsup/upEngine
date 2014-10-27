using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace upEngine
{
    public class GLSLShader : IShader
    {
        private readonly List<int> _shaderIDs;

        public int ID
        {
            get;
            protected set;
        }

        public GLSLShader()
        {
            _shaderIDs = new List<int>();
        }

        public GLSLShader( string shaderDir ) : this()
        {
            foreach( var file in Directory.GetFiles( Utils.GetContentPath( "/shaders/" + shaderDir ) ) )
            {
                var enums = Enum.GetNames( typeof( ShaderType ) );
                string first = null;
                foreach( string selEnum in enums )
                {
                    if( selEnum.Contains( file.Substring( file.LastIndexOf( "." ) + 1 ), StringComparison.OrdinalIgnoreCase ) && selEnum.Substring( selEnum.Length - 6 ) == "Shader" )
                    {
                        first = selEnum;
                        break;
                    }
                }

                if( first != null )
                    AttachSource( new StreamReader( file ).ReadToEnd(), (ShaderType) Enum.Parse( typeof( ShaderType ), first ) );
            }

            this.CompileShader();
            this.LinkShader();
        }

        public void AttachSource( string source, ShaderType shaderType )
        {
            int shaderID = GL.CreateShader( shaderType );
            GL.ShaderSource( shaderID, source );
            _shaderIDs.Add( shaderID );
        }

        public void AttachSourceFromContent( string path, ShaderType shaderType )
        {
            AttachSource( new StreamReader( Utils.GetContentPath( "/shaders/" + path ) ).ReadToEnd(), shaderType );
        }

        public void CompileShader()
        {
            foreach( var shaderID in _shaderIDs )
            {
                GL.CompileShader( shaderID );
            }
        }

        public void LinkShader()
        {
            if( ID != 0 )
                throw new InvalidOperationException( "ERROR! Cannot relink shader" );

            ID = GL.CreateProgram();

            foreach( var shaderID in _shaderIDs )
            {
                GL.AttachShader( ID, shaderID );
            }

            GL.LinkProgram( ID );
        }

        public void Use()
        {
            GL.UseProgram( this.ID );
        }

        public void SetUniform( string uniformName, int i )
        {
            int loc = GL.GetUniformLocation( this.ID, uniformName );
            GL.Uniform1( loc, i );
        }

        public void SetUniform( string uniformName, float f )
        {
            int loc = GL.GetUniformLocation( this.ID, uniformName );
            GL.Uniform1( loc, f );
        }

        public void SetUniform( string uniformName, Vector3 v3 )
        {
            int loc = GL.GetUniformLocation( this.ID, uniformName );
            GL.Uniform3( loc, v3 );
        }

        public void SetUniform( string uniformName, Vector4 v4 )
        {
            int loc = GL.GetUniformLocation( this.ID, uniformName );
            GL.Uniform4( loc, v4 );
        }

        public void SetUniform( string uniformName, Matrix4 m4 )
        {
            int loc = GL.GetUniformLocation( this.ID, uniformName );
            unsafe
            {
                GL.ProgramUniformMatrix4( this.ID, loc, 1, false, (float*) &m4 );
            }
        }
    }
}
