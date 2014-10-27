using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;

namespace upEngine
{
    public class Camera
    {
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
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView( fov * ( (float) Math.PI / 180f ), aspectRatio, near, far );
        }

        public Matrix4 ViewMatrix
        {
            get;
            private set;
        }

        public Matrix4 ProjectionMatrix
        {
            get;
            private set;
        }

        public Vector3 Origin
        {
            get;
            private set;
        }

        public Vector3 TargetPos
        {
            get;
            private set;
        }

        public Vector3 Up
        {
            get;
            private set;
        }

        public float Speed
        {
            get;
            set;
        }

        public void Update( Game game )
        {
            if( !game.Window.Focused )
                return;

            MoveMouse( game.Window );

            Vector3 forward = Vector3.Normalize( TargetPos - Origin ) * Speed;
            Vector3 strafe = Vector3.Normalize( Vector3.Cross( TargetPos - Origin, Up ) ) * Speed;
            var moveVector = new Vector3();

            if( game.Keyboard[ Key.W ] )
                moveVector += forward;
            if( game.Keyboard[ Key.S ] )
                moveVector -= forward;
            if( game.Keyboard[ Key.A ] )
                moveVector -= strafe;
            if( game.Keyboard[ Key.D ] )
                moveVector += strafe;

            TargetPos += moveVector;
            Origin += moveVector;

            Apply();
        }

        public void MoveMouse( GameWindow window )
        {
            double DeltaX = ( ( window.X + window.Width / 2 ) - Cursor.Position.X ) * 0.005f;
            double DeltaY = ( ( window.Y + window.Height / 2 ) - Cursor.Position.Y ) * 0.005f;
            Cursor.Position = new Point( window.X + window.Width / 2, window.Y + window.Height / 2 );

            MoveCamera( DeltaX, DeltaY );
        }

        private void MoveCamera( double deltaX, double deltaY )
        {
            if( deltaX != 0f )
            {
                TargetPos -= Origin;
                TargetPos = Vector3.Transform( TargetPos, Quaternion.FromAxisAngle( Up, (float) deltaX ) );
                TargetPos += Origin;
            }
            if( deltaY != 0f )
            {
                Vector3 DirTargetToOrigin = Vector3.Normalize( TargetPos - Origin );
                Vector3 DirForward = Vector3.Normalize( new Vector3( DirTargetToOrigin.X, 0f, DirTargetToOrigin.Z ) );
                double Angle = MathHelper.RadiansToDegrees( Math.Acos( Vector3.Dot( DirForward, DirTargetToOrigin ) ) );
                //if (DirTargetToOrigin.Y < 0)
                //    Angle *= -1f;

                Angle += deltaY;

                if( Angle < 89.50f )
                {
                    var oldTargetPos = new Vector3( TargetPos.X, TargetPos.Y, TargetPos.Z );
                    TargetPos -= Origin;
                    TargetPos = Vector3.Transform( TargetPos, Quaternion.FromAxisAngle( Vector3.Normalize( Vector3.Cross( DirForward, Up ) ), (float) deltaY ) );
                    //TargetPos = Vector3.Transform( TargetPos, Matrix4.CreateFromAxisAngle( Vector3.Normalize( Vector3.Cross( DirForward, Up ) ), (float) deltaY ) );
                    TargetPos += Origin;

                    DirTargetToOrigin = Vector3.Normalize( TargetPos - Origin );
                    DirForward = Vector3.Normalize( new Vector3( DirTargetToOrigin.X, 0f, DirTargetToOrigin.Z ) );
                    Angle = MathHelper.RadiansToDegrees( Math.Acos( Vector3d.Dot( new Vector3d( DirForward.X, DirForward.Y, DirForward.Z ), new Vector3d( DirTargetToOrigin.X, DirTargetToOrigin.Y, DirTargetToOrigin.Z ) ) ) );
                    if( Angle > 89.00f )
                        TargetPos = oldTargetPos;
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