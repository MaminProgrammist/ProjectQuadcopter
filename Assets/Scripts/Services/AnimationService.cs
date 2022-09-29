namespace Services
{
    public class AnimationService
    {
        public class Parameters
        {
            public const string MotionX = "MotionX";
            public const string MotionY = "MotionY";
            public const string Side = "Side";
        }

        public class States
        {
            public const string UpStrafe = "UpStrafe";
            public const string DownStrafe = "DownStrafe";
            public const string LeftStrafe = "LeftStrafe";
            public const string RightStrafe = "RightStrafe";
            public const string Death = "Death";
            public const string Idle = "Idle";
            public const string IdleStart = "IdleStart";
            public const string Start = "Start";
            public const string FearOfCollision = "FearOfCollision";
        }
    }
}
