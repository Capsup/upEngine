using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using OpenTK;
using OpenTK.Input;

namespace upEngine
{
    public class Camera
    {
        public Matrix4 ViewMatrix{ get; private set; }
        public Matrix4 ProjectionMatrix{ get; private set; }

        public Vector3 Origin{ get; private set; }
        public Vector3 TargetPos{ get; private set; }
        public Vector3 Up{ get; private set; }

        public float Speed{ get; set; }

        public Camera( Vector3 up, Vector3 targetPos, Vector3 origin, Matrix4 projectionMatrix, Matrix4 viewMatrix, float speed )
        {
            Up = up;
            TargetPos = targetPos;
            Origin = origin;
            ProjectionMatrix = projectionMatrix;
            ViewMatrix = viewMatrix;
            Speed = speed;
        }

        public Camera( float fov, float aspectRatio, float near, float far )
        {
            Up = new Vector3( 0, 1, 0 );
            TargetPos = new Vector3( 0, 0, -1 );
            Origin = new Vector3( 0, 0, 0 );
            Speed = 0.1f;
            ViewMatrix = Matrix4.LookAt( Origin, TargetPos, Up );
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView( fov * ((float)Math.PI / 180f), aspectRatio, near, far );
        }

        public void Update( Game game )
        {
            if( !game.Window.Focused )
                return;

            MoveMouse( game.Window );

            var forward = Vector3.Normalize( TargetPos - Origin ) * Speed;
            var strafe = Vector3.Normalize( Vector3.Cross( TargetPos - Origin, Up ) ) * Speed;
            var moveVector = new Vector3();

            if( game.Keyboard[ Key.W ] )
                moveVector += forward;
            if( game.Keyboard[ Key.S ] )
                moveVector -= forward;
            if( game.Keyboard[ Key.A ] )
                moveVector -= strafe;
            if( game.Keyboard[ Key.D ] )
                moveVector += strafe;

            const double sensitivity = 0.01;
            if(game.Keyboard[Key.Up])
                MoveCamera(0.0, 1*sensitivity);
            if(game.Keyboard[Key.Down])
                MoveCamera(0.0, -1*sensitivity);
            if(game.Keyboard[Key.Left])
                MoveCamera(-1*sensitivity, 0.0);
            if(game.Keyboard[Key.Right])
                MoveCamera(1*sensitivity, 0.0);
            if(game.Keyboard[Key.T])
                Test();

            TargetPos += moveVector;
            Origin += moveVector;

            Apply();
        }

        private void Test()
        {
            TargetPos = TargetPos;
            Apply();
        }

        public void MoveMouse( GameWindow window )
        {
            double DeltaX = ( (window.X + window.Width / 2 ) - System.Windows.Forms.Cursor.Position.X ) * 0.005f;
            double DeltaY = ( (window.Y + window.Height / 2 ) - System.Windows.Forms.Cursor.Position.Y ) * 0.005f;
            System.Windows.Forms.Cursor.Position = new Point( window.X + window.Width / 2, window.Y + window.Height / 2 );

            MoveCamera(DeltaX, DeltaY);
           
        }

        private void MoveCamera(double deltaX, double deltaY)
        {
            if (deltaX != 0f)
            {
                TargetPos -= Origin;
                TargetPos = Vector3.Transform(TargetPos, Quaternion.FromAxisAngle(Up, (float)deltaX));
                //TargetPos = Vector3.TransformVector( TargetPos, Matrix4.CreateFromAxisAngle( new Vector3( 0f, 1f, 0f ), (float)DeltaX ) );
                TargetPos += Origin;
            }
            if (deltaY != 0f)
            {
                Vector3 DirTargetToOrigin = Vector3.Normalize(TargetPos - Origin);
                Vector3 DirForward = Vector3.Normalize(new Vector3(DirTargetToOrigin.X, 0f, DirTargetToOrigin.Z));
                double Angle = MathHelper.RadiansToDegrees(Math.Acos(Vector3.Dot(DirForward, DirTargetToOrigin)));
                Console.WriteLine(Angle);
                //if (DirTargetToOrigin.Y < 0)
                //    Angle *= -1f;

                Console.WriteLine(DirTargetToOrigin.ToString());

                Angle += deltaY;
                if (Angle > 89.50f)
                    Console.WriteLine("Too big angle");

                var cross1 = Vector3.Normalize(Vector3.Cross(TargetPos - Origin, Up));
                var cross2 = Vector3.Normalize(Vector3.Cross(DirForward, Up));
                Console.WriteLine("Cross1: {0}", cross1);
                Console.WriteLine("Cross2: {0}", cross2);
                Console.WriteLine(Angle);
                if (Angle < 89.50f)
                {
                    var oldTargetPos = new Vector3(TargetPos.X, TargetPos.Y, TargetPos.Z);
                    Console.WriteLine("Old TargetPos: {0}", TargetPos);
                    TargetPos -= Origin;
                    //TargetPos = Vector3.Transform( TargetPos, Quaternion.FromAxisAngle( Vector3.Normalize( Vector3.Cross( DirForward, Up ) ), (float)DeltaY ) );
                    TargetPos = Vector3.TransformVector(TargetPos, Matrix4.CreateFromAxisAngle(Vector3.Normalize(Vector3.Cross(DirForward, Up)), (float)deltaY));
                    TargetPos += Origin;

                    DirTargetToOrigin = Vector3.Normalize(TargetPos - Origin);
                    DirForward = Vector3.Normalize(new Vector3(DirTargetToOrigin.X, 0f, DirTargetToOrigin.Z));
                    Angle = MathHelper.RadiansToDegrees(Math.Acos(Vector3d.Dot(new Vector3d(DirForward.X, DirForward.Y, DirForward.Z), new Vector3d(DirTargetToOrigin.X, DirTargetToOrigin.Y, DirTargetToOrigin.Z))));
                    if (Angle > 89.00f)
                        TargetPos = oldTargetPos;
                    Console.WriteLine("New TargetPos: {0}", TargetPos);
                    Console.WriteLine(TargetPos);
                    Console.WriteLine("New Angle :{0}", Angle);
                }
            }

            Apply();
        }

        public void Apply()
        {
            ViewMatrix = Matrix4.LookAt( Origin, TargetPos, Up );
        }
    }
}
