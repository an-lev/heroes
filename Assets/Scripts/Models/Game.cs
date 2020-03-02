﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Models
{
    public class Game
    {
        private static Random _random = new Random();

        public Grid Grid { get; }
        public List<Hero> Heroes { get; }

        private Queue<Detachment> _detachments;

        public Game()
        {
            Grid = new Grid(Settings.Grid.Width, Settings.Grid.Height);

            Heroes = new List<Hero>()
            {
                new Hero(),
                new Hero()
            };

            foreach (var hero in Heroes)
                GenerateArmy(hero);

            SetPlayer();
            SetEnemy();

            foreach (var hero in Heroes)
            foreach (var detachment in hero.Detachments)
                _detachments.Enqueue(detachment);

            _detachments.OrderByDescending(x => x.Speed);
        }

        public bool Continue() =>
            Heroes.All(x => x.Detachments.Count != 0);

        public void Play()
        {
            var detachment = _detachments.Dequeue();
            detachment.Step();
            _detachments.Enqueue(detachment);
        }

        private void GenerateArmy(Hero hero)
        {
            for (int i = 0; i < 7; i++)
            {
                int type = _random.Next(5);
                int amount = _random.Next(1, 31);

                hero.Detachments.Add(new Detachment((Unit.Type)type, amount));
            }
        }

        private void SetPlayer()
        {
            int i = 0;
            foreach (var cell in Grid.PlayerSide.Take(Heroes[0].Detachments.Count))
                Grid[cell] = Heroes[0].Detachments[i++];
        }

        private void SetEnemy()
        {
            int i = 0;
            foreach (var cell in Grid.EnemySide.Take(Heroes[1].Detachments.Count))
                Grid[cell] = Heroes[1].Detachments[i++];
        }
    }
}
