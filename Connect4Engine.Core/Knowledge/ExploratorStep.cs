namespace Connect4Engine.Core.Knowledge
{
    public enum ExploratorStep
    {
        None,
        Initialization,
        DiscoveringPotentialPositions,
        OrganizingPotentialPositions,
        FetchingPositions,
        OptimizingFetchedPositions,
        End
    }
}
