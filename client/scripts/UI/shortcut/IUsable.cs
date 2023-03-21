using Godot;

interface IUsable
{
  Node GetData();
  bool IsAvailable();
  void Use();
}
