namespace GameLogic.BattleDemo
{
    public class PlayerEntity : EntityLogic
    {
        public override ActorEntityType GetActorEntityType()
        {
            return ActorEntityType.Player;
        }

        protected override void OnLogicCreate()
        {
            base.OnLogicCreate();
            ActorData = Attach<ActorData>();
            BuffComponent = Attach<BuffComponent>();
            SkillCaster = Attach<SkillCasterComponent>();
            Attach<MoveComponent>();
        }
    }
}