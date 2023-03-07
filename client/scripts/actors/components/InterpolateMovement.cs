// using Godot;

// struct State
// {
//   public Vector3 Position; 
//   public float Yaw;
//   public ulong Tick;
//   public bool Stop;
// }

// class MovementBuffer
// {
//   CharacterActor actor;

//   List<InterpolateMovement> interpolate;

//   public MovementBuffer(CharacterActor actor)
//   {
//     this.actor = actor;
//   }
//   void InterpolateMove(float delta)
//   {
//     var time = NetworkManager.Instance.ServerTick - 100;

//     if(interpolate.Count > 1)
//     {
//       while(interpolate.Count > 2 && time > interpolate[1].Tick)
//       {
//         interpolate.RemoveAt(0);
//       }

//       var factor = Mathf.Clamp((float)(time - interpolate[0].Tick) / (float)(interpolate[1].Tick - interpolate[0].Tick), 0, 1);

//       Position = interpolate[0].Position.Lerp(interpolate[1].Position, factor);
//       Rotation = new Vector3(0, Mathf.Lerp(interpolate[0].Yaw, interpolate[1].Yaw, factor), 0);
//     }

//   }
// }
