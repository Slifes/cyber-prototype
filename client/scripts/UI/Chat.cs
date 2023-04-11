using Godot;

partial class Chat : Window
{
  ScrollContainer scrollContainer;

  VBoxContainer container;

  TextEdit input;

  public override void _Ready()
  {
    scrollContainer = GetNode<ScrollContainer>("Panel/MarginContainer/ScrollContainer");

    container = GetNode<VBoxContainer>("Panel/MarginContainer/ScrollContainer/VBoxContainer");

    input = GetNode<TextEdit>("Panel/TextEdit");

    input.GuiInput += OnInputGuiInput;
  }

  void OnInputGuiInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton && @event.IsPressed())
    {
      input.Editable = true;
    }

    if (@event.IsActionPressed("ui_accept"))
    {
      var message = input.Text;

      if (message.Length > 0)
      {
        // NetworkManager.Instance.SendPacket(new CMChatMessage { Message = message });
        AddMessage(input.Text.Trim());
        input.Text = "";
      }
    }
  }

  public void AddMessage(string message)
  {
    var label = new Label();
    label.AddThemeFontSizeOverride("font_size", 10);
    label.Text = message;

    container.AddChild(label);

    CallDeferred("SetScrollToEnd");
  }

  void SetScrollToEnd()
  {
    scrollContainer.ScrollVertical = (int)scrollContainer.GetVScrollBar().MaxValue;
  }
}
