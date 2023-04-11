using System.Collections.Generic;

public enum RewardType
{
  Money,
  Item,
  Reputation,
}

enum StepType
{
  CollectItem,
  KillNpc,
  TalkToNpc,
  Challenge,
}

struct TargetStep
{
  public int StepTargetID;
  public int Amount;
}

class Step
{
  public int Index;
  public StepType Type;
  public TargetStep[] Goals;
}

class Quest
{
  public int ID;
  public int ActorID;
  public string Title;
  public string Description;
  public int Reputation;
  public int TimeLimit;
  public Step[] Steps;
  public Dictionary<RewardType, int> Rewards;
}
