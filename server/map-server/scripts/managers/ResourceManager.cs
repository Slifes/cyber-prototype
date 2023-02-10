using Godot;
using System;
using System.Collections.Generic;

class ResourceManager<T> where T : Base
{
  string directory;

  Dictionary<int, T> resources;

  private static ResourceManager<T> _instance;

  public static ResourceManager<T> Instance { get { return _instance; } }

  public static void CreateInstance(string directory)
  {
    _instance = new ResourceManager<T>(directory);
  }

  protected ResourceManager() { }

  ResourceManager(string directory)
  {
    this.directory = directory;

    resources = new();
  }

  public void Load()
  {
    GD.Print(String.Format("Loading {0}...", directory));

    var files = Godot.DirAccess.GetFilesAt("res://resources/" + directory);

    foreach (var res in files)
    {
      var resource = ResourceLoader.Load<T>(String.Format("res://resources/{0}/{1}", directory, res.Replace(".remap", "")));

      resources.Add(resource.ID, resource);

      GD.Print("Skill Loaded ", res);
    }
  }

  public T Get(int id)
  {
    if (!resources.ContainsKey(id))
      return null;

    return (T)resources[id];
  }
}
