using System;
using System.Collections.Generic;
using Project.PlayerLogic;
using UnityEngine;

namespace Project.Bot
{
    public class PlayerDestinationService : MonoBehaviour, IPlayerSpawnedListener
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private PlayerZone _zone;

        private List<PlayerZone> _zones = new List<PlayerZone>();

        private PlayerZone _currentZone;
        private Player _player;

        public PlayerZone PlayerZone => _currentZone;
        public Player Player => _player;

        public void Init()
        {
            for (int width = 0; width < 6; width++)
            {
                for (int height = 0; height < 4; height++)
                {
                    Vector3 spawnPos = _pivot.position;

                    spawnPos.x += (width * _zone.transform.localScale.x);
                    spawnPos.y += (height * _zone.transform.localScale.x);

                    PlayerZone zone = Instantiate(_zone, transform);
                    zone.transform.position = spawnPos;

                    zone.PlayerStayed += OnPlayerStayed;
                    _zones.Add(zone);
                }
            }
        }

        private void OnPlayerStayed(PlayerZone zone)
        {
            _currentZone = zone;
        }

        public void PlayerSpawned(Player player)
        {
            _player = player;
        }

        private void OnDestroy()
        {
            foreach (PlayerZone playerZone in _zones) 
                playerZone.PlayerStayed -= OnPlayerStayed;
        }
    }
}