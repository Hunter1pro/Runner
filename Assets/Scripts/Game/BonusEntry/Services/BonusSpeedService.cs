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

        private bool _inProgress;

        public BonusSpeedService(IMoveComponent moveComponent, BonusEntryView bonusEntryView)
        {
            _moveComponent = moveComponent;
            _bonusEntryView = bonusEntryView;
        }

        public async void BonusTrigger(GameObject bonus)
        {
            GameObject.Destroy(bonus);

            if (_inProgress)
                await Task.Delay(TimeSpan.FromSeconds(0.1f), _cancellationTokenSourse.Token);
            
            _moveComponent.UpdateSpeed(_bonusEntryView.BonusSpeed);
            _inProgress = true;   
            
            await Task.Delay(TimeSpan.FromSeconds(_bonusEntryView.BonusSpeedTime), _cancellationTokenSourse.Token);
            
            _moveComponent.RestoreSpeed();

            _inProgress = false;
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