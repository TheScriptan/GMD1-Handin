using System;
using AI;
using Random = System.Random;

public class WanderState : HeroAIState
{
    private Random random;

    public WanderState(HeroAI heroAi) : base(heroAi)
    {
        random = new Random();
    }

    public override Type Tick()
    {
        var preconditionalState = GetPreconditionalState();
        if (preconditionalState != null)
            return preconditionalState;

        return GetRandomTask();
    }

    private Type GetRandomTask()
    {
        var percent = random.Next(0, 100);

        if (percent <= 60)
        {
            return typeof(CaptureMineState);
        }

        if (percent <= 75)
        {
            return typeof(CapturePillarState);
        }

        if (percent <= 99)
        {
            return typeof(VisitTownHallState);
        }

        return typeof(WanderState);
    }
}