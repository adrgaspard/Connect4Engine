using Connect4Engine.Core.Operation;

namespace Connect4Engine.Core.AI
{
    public sealed class MoveAnalyzer
    {
        public MoveClassification Analyze(GameEngine game, IReadOnlyList<MovementDescriptor> movementScores, sbyte chosenMovementScore, sbyte playerPreviousScore)
        {
            sbyte[] orderedPossibleScores = movementScores.Where(descriptor => descriptor.Possible).Select(descriptor => descriptor.Score).OrderByDescending(score => score).ToArray();
            int theoricMaxScore = (game.RemainingMoves / 2) - 1;
            if (orderedPossibleScores.Length <= 1)
            {
                return MoveClassification.OnlyOne; // The only possible movement.
            }
            if (chosenMovementScore == orderedPossibleScores[0])
            {
                if ((Analyze(game, movementScores, orderedPossibleScores[1], playerPreviousScore) & (MoveClassification.Positives | MoveClassification.MissedWin)) == 0 && chosenMovementScore >= 0)
                {
                    if (chosenMovementScore > 0 && ((playerPreviousScore < 0 && chosenMovementScore - playerPreviousScore > theoricMaxScore / 3) || chosenMovementScore - playerPreviousScore > theoricMaxScore / 2))
                    {
                        return MoveClassification.Brilliant; // The only good move and it swings the course of the game !
                    }
                    return MoveClassification.Great; // The only good move !
                }
                return MoveClassification.Best; // The best possible move !
            }
            if (playerPreviousScore < chosenMovementScore)
            {
                if (orderedPossibleScores[0] - chosenMovementScore >= theoricMaxScore / 3)
                {
                    return MoveClassification.MissedWin; // A missed move that could have improved significantly more the position or won, but does not degrade it either.
                }
                return MoveClassification.Excellent; // Excellent : A great move which improves the position, but not quite the best!
            }
            if (playerPreviousScore == chosenMovementScore)
            {
                if (playerPreviousScore > 0)
                {
                    return MoveClassification.Good; // A movement that allows you to stay in a good position without improving it.
                }
                return MoveClassification.MissedWin; // A missed move that could have improved the position or won, but does not degrade it either.
            }
            if (chosenMovementScore >= 0)
            {
                if (chosenMovementScore == 0 || playerPreviousScore - chosenMovementScore >= theoricMaxScore / 2)
                {
                    return MoveClassification.Mistake; // A move that immediately worsens the current position while it was winning.
                }
                return MoveClassification.Inaccuracy; // A weak move that could be much better but it still allows win.
            }
            if (playerPreviousScore - chosenMovementScore >= theoricMaxScore / 2)
            {
                return MoveClassification.Blunder; // A very bad move that significantly worsens the position or causes the loss.
            }
            return MoveClassification.Mistake; // A move that immediately worsens the current position while it was already losing.
        }
    }
}
