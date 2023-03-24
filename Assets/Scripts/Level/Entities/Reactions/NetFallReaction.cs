using UnityEngine;
using Entities;

namespace Reactions
{
    public class NetFallReaction : Reaction
    {
        private Animator _animator;

        public NetFallReaction(WindowGuy netGuy) => _animator = netGuy.GetComponent<Animator>();

        public override void React()
        {

        }
    }
}
