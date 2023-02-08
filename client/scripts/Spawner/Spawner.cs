using Godot;

partial class Spawner : Node
{
  CharacterSpawner playerSpawner;

  NpcSpawner npcSpawner;

  public override void _Ready()
  {
    playerSpawner = GetNode<CharacterSpawner>("players");
    npcSpawner = GetNode<NpcSpawner>("npcs");
  }

  public IActor GetActor(string id, ActorType actorType)
  {
    switch (actorType)
    {
      case ActorType.Player:
        if (playerSpawner.HasNode(id))
          return playerSpawner.GetNode<IActor>(id);
        break;
      case ActorType.Npc:
        if (npcSpawner.HasNode(id))
          return npcSpawner.GetNode<IActor>(id);
        break;
    }

    return null;
  }

  public void Spawn(Variant id, Variant type, Variant position, Variant yaw, Variant data)
  {
    ActorType _type = (ActorType)(int)type;

    switch (_type)
    {
      case ActorType.Player:
        playerSpawner.Spawn(id, (Vector3)position, (float)yaw, data);
        break;
      case ActorType.Npc:
        npcSpawner.Spawn(id, (Vector3)position, (float)yaw, data);
        break;
    }
  }

  public void Unspawn(Variant id, Variant type)
  {
    ActorType _type = (ActorType)(int)type;

    switch (_type)
    {
      case ActorType.Player:
        playerSpawner.Unspawn(id);
        break;
      case ActorType.Npc:
        npcSpawner.Unspawn(id);
        break;
    }
  }
}
