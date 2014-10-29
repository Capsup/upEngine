using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace upEngine
{
    public class Game : IDisposable
    {
        private GameWindow _window;
        public GameWindow Window
        {
            get
            {
                return _window ?? ( _window = new GameWindow() );
            }
        }

        public KeyboardState Keyboard{ get; set; }
        public Camera Player{ get; set; }

        public Game()
        {
            Window.Load += Window_OnLoad;
            Window.UpdateFrame += Window_OnUpdate;
            Window.RenderFrame += Window_OnRender;
            Window.Resize += Window_OnResize;
            Window.KeyUp += Window_OnKeyUp;
            Player = new Camera( 45f, Window.Width / Window.Height, 0.1f, 100000f );
        }

        public void Dispose()
        {
            Window.Dispose();
        }

        private void Window_OnLoad( object sender, EventArgs e )
        {
            Window.VSync = VSyncMode.On;

            ShaderManager.GetInstance().LoadShader( "/old/flat" );
        }

        private void Window_OnUpdate( object sender, FrameEventArgs e )
        {
            InputHandling();
            Player.Update( this );

            //Necessary to allow the OS to interrupt the CPU properly and allows the CPU to not idle at 100%
            Thread.Sleep( 1 );
        }

        private void Window_OnRender( object sender, FrameEventArgs e )
        {
            GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            
            var mvpMatrix = Player.ViewMatrix * Player.ProjectionMatrix;
            IShader flatShader = ShaderManager.GetInstance().GetShader("flat");
            flatShader.SetUniform( "mvpMatrix", mvpMatrix );

            flatShader.Use();

            GL.Begin( PrimitiveType.Triangles );
                /*GL.Vertex3( Vector4d.Transform( new Vector4d(0d, 0d, 0d, 1d), Player.ViewMatrix * Player.ProjectionMatrix ).Xyz );
                GL.Vertex3( Vector4d.Transform( new Vector4d(1d, 0d, 0d, 1d), Player.ViewMatrix * Player.ProjectionMatrix ).Xyz );
                GL.Vertex3( Vector4d.Transform( new Vector4d(0d, 1d, 0d, 1d), Player.ViewMatrix * Player.ProjectionMatrix ).Xyz );*/
            GL.Vertex3( new Vector3( 0, 0, 0 ) );
            GL.Vertex3( new Vector3( 1, 0, 0 ) );
            GL.Vertex3( new Vector3( 0, 1, 0 ) );
            GL.End();

            GL.UseProgram( 0 );
            Window.SwapBuffers();
        }

        private void Window_OnResize( object sender, EventArgs e )
        {
            GL.Viewport( 0, 0, Window.Width, Window.Height );
        }

        private void InputHandling()
        {
            Keyboard = OpenTK.Input.Keyboard.GetState();


        }

        private void Window_OnKeyUp( object sender, KeyboardKeyEventArgs e )
        {
            switch( e.Key )
            {
                case Key.Escape:
                    Window.Exit();
                break;
            }
        }
    }
}
