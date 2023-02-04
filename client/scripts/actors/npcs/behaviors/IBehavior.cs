using Godot;

interface IBehavior
{
  void Start();

  void Handler(double delta);

  void Finish();

  void SetData(Variant data);
}
