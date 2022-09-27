using System.Collections.Generic;
using UnityEngine;
using General;
using Entities;
using Reactions;

namespace Components
{
    public class WeaponEquiper : ConfigReceiver<GuyConfig>
    {
        private List<WeaponPoint> _netPoints = new();
        private List<Weapon> _nets = new();
        private Weapon _equipedNet;

        public Weapon EquipedNet
        {
            get => _equipedNet;

            private set 
            {
                _equipedNet?.gameObject.SetActive(false);
                value.gameObject.SetActive(true);
                _equipedNet = value;
            }
        }

        public override void Receive(GuyConfig config)
        {
            base.Receive(config);
            _netPoints.AddRange(GetComponentsInChildren<WeaponPoint>());

            for (int i = 0; i < _netPoints.Count; i++)
            {
                Weapon net = Instantiate(_config.WeaponPrefab, _netPoints[i].transform);
                net.AddReaction<CollisionDetector, Quadcopter>(new CatchReaction(GetComponent<Guy>(), config));
                _nets.Add(net);
            }
        }   

        public void Equip() => EquipedNet = _nets[Random.Range(0, _nets.Count)];
    }
}
