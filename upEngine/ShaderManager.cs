using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upEngine
{
    public class ShaderManager
    {
        private static ShaderManager _instance;
        private readonly Dictionary<string, IShader> _dictionary;

        public ShaderManager()
        {
            _dictionary = new Dictionary<string, IShader>();
        }

        public static ShaderManager GetInstance()
        {
            return _instance ?? ( _instance = new ShaderManager() );
        }

        public string LoadShader( string dir )
        {
            if( !Directory.Exists( Utils.GetContentPath( "/shaders/" + dir ) ) )
                throw new FileNotFoundException( "ERROR! Unable to locate directory: " + dir );

            string id = dir.Substring( dir.LastIndexOf( "/" ) + 1 );
            _dictionary.Add( id, new GLSLShader( dir ) );

            return id;
        }

        public IShader GetShader( string id )
        {
            IShader shader;
            if( !_dictionary.TryGetValue( id, out shader ) )
                throw new ArgumentException( "ERROR! Shader by ID: " + id + " not found." );

            return shader;
        }

        public void UseShader( string id )
        {
            IShader shader;
            if( _dictionary.TryGetValue( id, out shader ) )
            {
                shader.Use();
            }
            else
            {
                throw new ArgumentException( "ERROR! Shader by ID: " + id + " not loaded." );
            }
        }
    }
}
