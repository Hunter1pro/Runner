using System.Threading.Tasks;
using DIContainerLib;
using Game.Bonus.Services;
using Game.Bonus.Views;
using Game.Level.Data;
using Game.Systems;
using GameObjectService;
using UnityEngine;

namespace Game.Bonus
{
    public class BonusEntry : BaseSystem
    {
        protected override int _initOrder { get; } = -1;
        
        [SerializeField]
        private BonusEntryView _bonusEntryView;

        private IBonusSpeedService _bonusSpeedService;
        
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public DIContainer ResolveBonusServises(IMoveComponent moveComponent)
        {
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            
            diServiceCollection.RegisterSingleton<IBonusSpeedService, BonusSpeedService>();
            diServiceCollection.RegisterSingleton<IMoveComponent, MoveComponent>(moveComponent);
            diServiceCollection.RegisterSingleton(_bonusEntryView);
            
            var container = diServiceCollection.GenerateContainer();

            _bonusSpeedService = container.GetService<IBonusSpeedService>();

            return container;
        }

        public void BonusTrigger(GameObject bonus, BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.Speed:
                    _bonusSpeedService.BonusTrigger(bonus);
                    break;
            }
        }
    }
}

