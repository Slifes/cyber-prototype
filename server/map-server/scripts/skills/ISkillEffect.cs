using Godot;

interface ISkillEffect
{
  void SetOwner(IActor actor);

  void SetEffectRotation(Vector3 rotation);

  void SetEffectPosition(Vector3 position);
}
