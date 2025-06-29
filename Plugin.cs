using System;
using System.Collections.Generic;
using Desfribalator;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MapGeneration;
using MEC;
using UnityEngine;

namespace Defibrillator
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name => "Defibrillator";
        public override string Prefix => "defibrillator";
        public override string Author => "@dzarenafixer";
        public override Version Version { get; } = new Version(1, 4, 3);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public override PluginPriority Priority => PluginPriority.Default;
        public EventHandler EventHandlers;
        public readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        public static Plugin Instance;


        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandler();
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += Pocket;

            CustomItem.RegisterItems();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= Pocket;
            EventHandlers = null;
            Instance = null;

            CustomItem.UnregisterItems();
        }

        private void Pocket(EscapingPocketDimensionEventArgs ev)
        {
            if(ev.TeleportPosition.TryGetRoom(out RoomIdentifier room))
                if (room.Name == RoomName.Pocket)
                    ev.TeleportPosition = Room.Random().Position;
        }
    }
}