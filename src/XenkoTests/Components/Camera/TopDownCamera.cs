namespace XenkoTests.Components.Camera
{
    using Xenko.Core.Mathematics;
    using Xenko.Engine;
    using Xenko.Input;
    using Xenko.Rendering.Compositing;

    public class TopDownCameraSettings
    {
        public Keys Forwards = Keys.W;
        public Keys Backwards = Keys.S;
        public Keys Left = Keys.A;
        public Keys Right = Keys.D;
        public Keys RotateLeft = Keys.Q;
        public Keys RotateRight = Keys.E;
    }

    public class TopDownCamera : SyncScript
    {
        public bool InputEnabled { get; set; } = true;

        public TopDownCameraSettings Settings
        {
            get => settings;
            set
            {
                if (settings != value)
                {
                    settings = value;
                    OnSettingsChanges();
                }
            }
        }

        public float ArmLengthMin { get; set; } = 1.0f;
        public float ArmLengthMax { get; set; } = 30.0f;

        public float ArmLength
        {
            get => Arm.Transform.Position.Z;
            set
            {
                if (Arm.Transform.Position.Z != value)
                {
                    Arm.Transform.Position.Z = MathUtil.Clamp(value, ArmLengthMin, ArmLengthMax);
                }
            }
        }

        public CameraComponent CameraComponent { get; private set; }

        public Entity Offset { get; private set; }
        public Entity Arm { get; private set; }
        public SceneCameraSlotId Slot { get; set; }

        private TopDownCameraSettings settings;

        /// <summary>
        /// Initialize a new <see cref="TopDownCamera"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="slot"></param>
        public TopDownCamera(TopDownCameraSettings settings, SceneCameraSlotId slot)
        {
            this.settings = settings;
            this.Slot = slot;
        }

        public override void Start()
        {
            CameraComponent = new CameraComponent { Slot = Slot };

            Arm = new Entity { CameraComponent };
            Arm.Transform.Position = new Vector3(0, 0, 12);

            Offset = new Entity();
            Offset.Transform.Position = new Vector3(0, 1.65f, 0);
            Offset.Transform.Rotation = Quaternion.RotationYawPitchRoll(0, -45, 0);

            Offset.AddChild(Arm);
            Entity.AddChild(Offset);

            base.Start();
        }

        public override void Update()
        {
            if (InputEnabled)
            {
                var (forward, _, right) = Entity.Transform.GetDirection();

                var inputVector = GetInputVector() * 0.1f;
                Entity.Transform.Position += inputVector.X * forward + inputVector.Y * right; ;

                var rotationVector = GetRotateVector() * 0.01f;
                Entity.Transform.Rotation *= Quaternion.RotationY(rotationVector);

                if (Input.MouseWheelDelta != 0)
                {
                    ArmLength = Arm.Transform.Position.Z - Input.MouseWheelDelta;
                }
            }
        }

        protected virtual Vector2 GetInputVector()
        {
            var inputVector = Vector2.Zero;
            if (Input.IsKeyDown(Settings.Forwards)) inputVector.X += 1;
            if (Input.IsKeyDown(Settings.Backwards)) inputVector.X -= 1;
            if (Input.IsKeyDown(Settings.Left)) inputVector.Y -= 1;
            if (Input.IsKeyDown(Settings.Right)) inputVector.Y += 1;
            return inputVector;
        }

        protected virtual float GetRotateVector()
        {
            var rotation = 0.0f;
            if (Input.IsKeyDown(Settings.RotateLeft)) rotation -= 1;
            if (Input.IsKeyDown(Settings.RotateRight)) rotation += 1;
            return rotation;
        }

        protected virtual void OnSettingsChanges()
        {

        }
    }
}
