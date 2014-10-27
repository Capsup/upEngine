using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace upEngine
{
    public static class Utils
    {
        /*public static ShaderType? TryFindFromString( this String filename )
        {
            var enums = Enum.GetNames( typeof( ShaderType ) );
            string first = null;
            foreach( string selEnum in enums )
            {
                if( selEnum.Contains( filename.Substring( filename.LastIndexOf( "." ) ) ) && selEnum.Substring( selEnum.Length - 6 ) == "Shader" )
                {
                    first = selEnum;
                    break;
                }
            }
            return first == null ? null : (ShaderType?) Enum.Parse( typeof( ShaderType ), first );
        }*/

        #region Extensions
        #region String
        public static bool Contains( this string source, string toCheck, StringComparison comp )
        {
            return source.IndexOf( toCheck, comp ) >= 0;
        }

        #endregion
        #endregion

        public static string GetContentPath()
        {
            return Environment.CurrentDirectory + "/../../content/";
        }

        public static string GetContentPath( string path )
        {
            return Environment.CurrentDirectory + "/../../content/" + ( path.Substring( 0, 1 ) == "/" ? path : "/" + path );
        }
    }
}
