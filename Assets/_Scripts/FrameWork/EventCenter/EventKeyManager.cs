﻿using Unity.IO.LowLevel.Unsafe;

namespace FrameWork.EventCenter
{
    public static class PlayerEventKey
    {
        
    }

    public static class UIEventKey
    {
        public static readonly EventKey OnChangeUIPrefab = new("OnChangeUIPrefab");
    }

    public static class StateKey
    {
        public static readonly EventKey OnSceneStateChange = new("OnSceneStateChange");
        public static readonly EventKey OnGameStateChange = new("OnGameStateChange");
    }
}
