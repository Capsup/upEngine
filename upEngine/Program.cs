using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.ES10;

namespace upEngine
{
    class Program
    {
        static void Main( string[] args )
        {
            using( var game = new Game() )
            {
                game.Window.Title = "upEngine";
                game.Window.Run();
            }
        }

    }
}
