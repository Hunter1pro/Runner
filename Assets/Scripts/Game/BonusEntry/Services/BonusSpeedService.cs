using System;
using System.Threading;
using System.Threading.Tasks;
using Game.Bonus.Views;
using Game.Systems;
using UnityEngine;

namespace Game.Bonus.Services
{
    public class BonusSpeedService : IBonusSpeedService
    {
        private IMoveComponent _moveComponent;
        private BonusEntryView _bonusEntryView;
        private CancellationTokenSource _cancellationTokenSourse = new CancellationTokenSource();

        public BonusSpeedService(IMoveComponent moveComponent, BonusEntryView bonusEntryView)
        {
            _moveComponent = moveComponent;
            _bonusEntryView = bonusEntryView;
        }

        public async void BonusTrigger(GameObject bonus)
        {
            GameObject.Destroy(bonus);
            
            _moveComponent.UpdateSpeed(_bonusEntryView.BonusSpeed);

            await Task.Delay(TimeSpan.FromSeconds(_bonusEntryView.BonusSpeedTime), _cancellationTokenSourse.Token);
            
            _moveComponent.RestoreSpeed();
        }

        public void Dispose()
        {
            _cancellationTokenSourse.Cancel();
            _cancellationTokenSourse?.Dispose();
        }
    }

    public interface IBonusSpeedService : IDisposable
    {
        void BonusTrigger(GameObject bonus);
    }
}