/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 22.07.2016 22:26:29
*/

using System;

namespace Nano3
{
    public interface IUpdatable
    {
        double Interval { get; set; }
        void Update(double time);
    }

    public interface ITickable
    {
        void Tick(double deltaTime);
    }
}
