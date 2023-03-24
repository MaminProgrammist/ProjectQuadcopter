using UnityEngine;
using Services;
using Components;
using Reactions;

namespace Entities
{
    class WindowGuyFactory : EntityFactory<WindowGuy, GuyConfig>
    {
        public WindowGuyFactory(GuyConfig config) : base(config) { }

        public override WindowGuy GetCreated()
        {
            WindowGuy guy = Object.Instantiate(_config.GuyPrefab);
            Animator animator =  guy.GetComponentInChildren<Animator>();
            animator.keepAnimatorControllerStateOnDisable = true;

            guy.gameObject.AddComponent<Mover>().Receive(_config);

            guy.gameObject
                .AddComponent<Disappearer>()
                .OnDisappear += () => guy.GetComponentInChildren<Animator>().SetFloat(AnimationService.Parameters.Side, 0);

            guy
                .AddReaction<BoxDetector, Quadcopter>(new ShoveOutReaction(guy, _config))
                .Receive(_config);

            guy.gameObject
                .AddComponent<WeaponEquiper>()
                .Receive(_config);

            return guy;
        }
    }
}
