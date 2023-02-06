abstract class PhysicalAttack
{
  protected IActor owner;

  Skill skillData;

  protected void Collided(IActor actor)
  {
    actor.TakeDamage(skillData.Damage);
  }
}
