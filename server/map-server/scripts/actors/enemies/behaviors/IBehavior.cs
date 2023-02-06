using Godot;

interface IBehavior
{
  void Start();

  void Handler(double delta);

  void Finish();

  Variant GetData();
}
